using IPR_BE.Models;
using System.Text.Json;
using IPR_BE.DataAccess;
using Serilog;

namespace IPR_BE.Services;

public class IMochaService {

    private HttpClient http;
    private readonly IConfiguration config;
    private readonly InterviewBotRepo ibrepo;
    private readonly SMTPService smtp;
    private readonly TestReportDbContext context;
    private readonly ILogger<IMochaService> log;

    public IMochaService(IConfiguration iConfig, InterviewBotRepo ibrepo, SMTPService smtpService, TestReportDbContext dbcontext,
        ILogger<IMochaService> log) {
        config = iConfig;
        this.ibrepo = ibrepo;
        smtp = smtpService;
        context = dbcontext;
        this.log = log;

        //initialize HttpClient and set the BaseAddress and add the X-API-KEY header 
        http = new HttpClient();
        http.DefaultRequestHeaders.Add("X-API-KEY", iConfig.GetValue<string>("IMocha:ApiKey"));
        http.BaseAddress = new Uri(iConfig.GetValue<string>("IMocha:BaseURL") ?? "");
    }

    public async Task<HttpResponseMessage> GetAllTests(int? pageNo = 1, int? pageSize = 100, string? labelsFilter= "Interview Prep Video Tests") {
        
        HttpResponseMessage response = await http.GetAsync($"tests?pageNo={pageNo}&pageSize={pageSize}&labelsFilter={labelsFilter}");
        return response;
    }

    public async Task<HttpResponseMessage> GetTestAttemptById(int testInvitationId){
        HttpResponseMessage response = await http.GetAsync($"reports/{testInvitationId}");
        return response;
    }

    public async Task<TestResultDTO> GetVidTestAttempt(int testInvitationId){
        HttpResponseMessage response = new HttpResponseMessage();
        TestResultDTO result;
        Dictionary<int,decimal> questionIds= new Dictionary<int,decimal>();

        //Getting the scores, this one hurt
        TestDetail test;
        test = ibrepo.GetTestByID(testInvitationId);


        response = await http.PostAsync($"reports/{testInvitationId}/questions", null);

        //Adding logging here instead
        if(response.IsSuccessStatusCode){
            log.LogInformation($"Successfully retrieved report for testInviteId: {testInvitationId}");
        }else{
            log.LogError($"Failed to retrieve test report for {testInvitationId} " + await response.Content.ReadAsStringAsync());
        }

        string str = await response.Content.ReadAsStringAsync();
        result = JsonSerializer.Deserialize<TestResultDTO>(str) ?? new TestResultDTO();

        try{
             //Adding the average score
            foreach(Result res in result.result){
                res.average = test.scoreSum;
                
                var matchingTest = test.questions.FirstOrDefault(x => x.questionId == res.questionId);

                if(matchingTest != null){
                    //Adding the score
                    res.score = (double)matchingTest.score;

                    //Adding the answers as well
                    res.givenAnswer = matchingTest.givenAnswer;
                    res.correctAnswer = matchingTest.correctAnswer;
                }
            }
        }catch(Exception e){
            log.LogError(e.Message);
        }
        //Adding the individual question scores. 
        return result;
    }

    /// <summary>
    /// Invites Candidates then does a few more things
    /// 1. Pings iMocha for TestInvitationURL
    /// 2. Sends the candidate test invitation email
    /// 3. Saves the testid, attemptid, and status to our own db, for easier time querying 
    /// </summary>
    /// <param name="invite">
    /// JSON object with following properties
    /// testId: int, required
    /// email: string, required,
    /// name: string, required
    /// </param>
    /// <returns>nothing</returns>
    public async Task<HttpResponseMessage> InviteCandidates(CandidateInvitation invite) {
        IMochaCandidateInvitationBody iMochaRequestBody = new IMochaCandidateInvitationBody(config, invite.name, invite.email);
        
        //call iMocha api to get the test invitation link
        JsonContent content = JsonContent.Create<IMochaCandidateInvitationBody>(iMochaRequestBody);

        Log.Information("Inviting candidate to imocha test with following body {requestBody}", iMochaRequestBody);

        HttpResponseMessage response = await http.PostAsync($"tests/{invite.testId}/invite", content);
        string responseStr = await response.Content.ReadAsStringAsync();

        if(response.IsSuccessStatusCode) {
            IMochaTestInviteResponse responseBody = JsonSerializer.Deserialize<IMochaTestInviteResponse>(responseStr)!;
            Log.Information("Inviting candidate was successful {responseBody}", responseBody);
            
            //first, look up if we already have this user in OUR db
            Candidate? candidate = context.Candidates.FirstOrDefault(c => c.email == invite.email && c.name == invite.name);

            //if they don't exist in db, then create new candidate obj
            if(candidate == null) {
                candidate = new Candidate(invite.name, invite.email);
                context.Add(candidate);
                context.SaveChanges();
                context.ChangeTracker.Clear();
            }
            
            //if attempt already exists, we shouldn't save it again
            TestAttempt? attempt = context.TestAttempts.FirstOrDefault(a => a.attemptId == responseBody.testInvitationId);
            
            if(attempt == null) {
                attempt = new TestAttempt {
                    candidateId = candidate.id,
                    testId = invite.testId,
                    attemptId = responseBody.testInvitationId,
                    invitationUrl = responseBody.testUrl,
                    status = "Pending"
                };  
                context.Add(attempt);
            }
            else {
                attempt.modifiedOn = DateTime.UtcNow;
            }
            context.SaveChanges(); 

        }
        else {
            Log.Warning("Imocha responded with error to test invitation {status}: {responseStr}",response.StatusCode, responseStr);
        }
        return response;
    }

}
using IPR_BE.Models;
using System.Text.Json;
using IPR_BE.DataAccess;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;

namespace IPR_BE.Services;

public class IMochaService {

    private HttpClient http;
    private readonly IConfiguration config;
    private readonly InterviewBotRepo ibrepo;
    private readonly SMTPService smtp;
    private readonly TestReportDbContext context;





    public IMochaService(IConfiguration iConfig, InterviewBotRepo ibrepo, SMTPService smtpService, TestReportDbContext dbcontext) {
        config = iConfig;
        this.ibrepo = ibrepo;
        smtp = smtpService;
        context = dbcontext;


        //initialize HttpClient and set the BaseAddress and add the X-API-KEY header 
        http = new HttpClient();
        http.DefaultRequestHeaders.Add("X-API-KEY", iConfig.GetValue<string>("IMocha:ApiKey"));
        http.BaseAddress = new Uri(iConfig.GetValue<string>("IMocha:BaseURL") ?? "");
    }

    public async Task<IMochaTestDTO> GetAllTests(int? pageNo = 1, int? pageSize = 100, string? labelsFilter= "Interview Prep Video Tests") {

        string response = await http.GetStringAsync($"tests?pageNo={pageNo}&pageSize={pageSize}&labelsFilter={labelsFilter}");
        return JsonSerializer.Deserialize<IMochaTestDTO>(response) ?? new IMochaTestDTO();
    }

    public async Task<CandidateTestReport> GetTestAttemptById(int testInvitationId){
        string str = await http.GetStringAsync($"reports/{testInvitationId}");
        CandidateTestReport report = JsonSerializer.Deserialize<CandidateTestReport>(str) ?? new CandidateTestReport();
        TestDetail ibotTestScore = ibrepo.GetTestByID(testInvitationId);
        report.score = ibotTestScore.scoreSum;
        return report;
    }

    public async Task<TestResultDTO> GetVidTestAttempt(int testInvitationId){
        HttpResponseMessage response = new HttpResponseMessage();
        TestResultDTO result;
        Dictionary<int,decimal> questionIds= new Dictionary<int,decimal>();

        //Getting the scores, this one hurt
        TestDetail test;
        test = ibrepo.GetTestByID(testInvitationId);

        response = await http.PostAsync($"reports/{testInvitationId}/questions", null);

        string str = await response.Content.ReadAsStringAsync();
        result = JsonSerializer.Deserialize<TestResultDTO>(str) ?? new TestResultDTO();

        try{
             //Adding the average score
            foreach(Result res in result.result){
                res.average = test.scoreSum;
                
                var matchingTest = test.questions.FirstOrDefault(x => x.questionId == res.questionId);

                if(matchingTest != null){
                    res.score = (double)matchingTest.score;
                }
            }
        }catch(Exception e){

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
    public async Task InviteCandidates(CandidateInvitation invite) {
        //call iMocha api to get the test invitation link
        JsonContent content = JsonContent.Create<IMochaCandidateInvitationBody>(new IMochaCandidateInvitationBody {
            email = invite.email,
            name = invite.name,
            callbackUrl = config.GetValue<string>("IMocha:InviteCallBackURL")!
        });

        HttpResponseMessage response = await http.PostAsync($"tests/{invite.testId}/invite", content);
        
        //once we have that, send our custom email via SMTPService
        IMochaTestInviteResponse responseBody = JsonSerializer.Deserialize<IMochaTestInviteResponse>(await response.Content.ReadAsStringAsync())!;
        MailMessage msg = new MailMessage("no-reply@revature.com", invite.email){
            Subject = "iMocha Test Invitation",
            Body = $"Hi {invite.name},\nHere is your test invite link: \n {responseBody.testUrl}"
        };

        smtp.SendEmail(msg);

        //first, look up if we already have this user in OUR db
        Candidate? candidate = context.Candidates.FirstOrDefault(c => c.email == invite.email && c.name == invite.name);

        //if they don't exist in db, then create new candidate obj
        if(candidate == null) {
            candidate = new Candidate{
                name = invite.name,
                email = invite.email
            };
            context.Add(candidate);
            context.SaveChanges();
            context.ChangeTracker.Clear();
        }

        
        //if attempt already exists, we shouldn't save it again
        TestAttempt? attempt = context.TestAttempts.FirstOrDefault(a => a.attemptId == responseBody.testInvitationId);
        
        //This is a new invitation
        if(attempt == null) {
            attempt = new TestAttempt {
                candidateId = candidate.id,
                testId = invite.testId,
                attemptId = responseBody.testInvitationId,
                status = "Pending"
            };
            context.Add(attempt);
            context.SaveChanges();
        }
    }

}
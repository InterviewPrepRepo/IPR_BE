using IPR_BE.Models;
using System.Text.Json;
using IPR_BE.DataAccess;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace IPR_BE.Services;

public class IMochaService {

    private HttpClient http;
    private readonly IConfiguration configuration;
    private readonly InterviewBotRepo ibrepo;
    private readonly MailchimpService mcs;
    private readonly TestReportDbContext context;
    private readonly ILogger<IMochaService> log;

    public IMochaService(IConfiguration iConfig, InterviewBotRepo ibrepo, TestReportDbContext dbcontext,
        ILogger<IMochaService> log, MailchimpService mcs) {
        this.ibrepo = ibrepo;
        configuration = iConfig;
        context = dbcontext;
        this.log = log;
        this.mcs = mcs;

        //initialize HttpClient and set the BaseAddress and add the X-API-KEY header 
        http = new HttpClient();
        http.DefaultRequestHeaders.Add("X-API-KEY", iConfig.GetValue<string>("IMocha:ApiKey"));
        http.BaseAddress = new Uri(iConfig.GetValue<string>("IMocha:BaseURL") ?? "");
    }

    /// <summary>
    /// Just sends off a request to the IMocha API for the test information, not exposed in controller.
    /// </summary>
    /// <param name="testId"></param>
    /// <returns></returns>
    public async Task<HttpResponseMessage> GetATest(long testId){
        HttpResponseMessage response= await http.GetAsync($"tests/{testId}");
        return response;
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
    /// <returns>HTTP Response Message from iMocha</returns>
    public async Task<HttpResponseMessage> InviteCandidates(string origin, string host, CandidateInvitation invite) {
        string redirectUrl = origin + configuration.GetValue<string>("IMocha:RedirectUrlPath");
        IMochaCandidateInvitationBody iMochaRequestBody = new IMochaCandidateInvitationBody(host, redirectUrl, invite.name, invite.email);

        //call iMocha api to get the test invitation link
        JsonContent content = JsonContent.Create<IMochaCandidateInvitationBody>(iMochaRequestBody);

        Log.Information("Inviting candidate to imocha test with following body {requestBody}", iMochaRequestBody);

        HttpResponseMessage response = await http.PostAsync($"tests/{invite.testId}/invite", content);
        string responseStr = await response.Content.ReadAsStringAsync();

        if(response.IsSuccessStatusCode) {
            IMochaTestInviteResponse responseBody = JsonSerializer.Deserialize<IMochaTestInviteResponse>(responseStr)!;
            Log.Information("Inviting candidate was successful {responseBody}", responseBody);

            //Replacing the URL
            responseBody.testUrl = responseBody.testUrl.Replace("test.imocha.io", "coding.revature.com");
            
            //Get a test info so we can get the name of the test
            string responseBodyString = await (await GetATest(invite.testId)).Content.ReadAsStringAsync();
            IMochaTest? testInfo = JsonSerializer.Deserialize<IMochaTest>(responseBodyString);

            //we have testID, name, email, attemptId, attemptURL
            //What we don't have is test name, start/end date
            await mcs.sendMailchimpMessageAsync("today: please fix", "a week from now: please fix", responseBody.testUrl, iMochaRequestBody.name, iMochaRequestBody.email, testInfo?.testName ?? "", false);
            
            Candidate candidate = UpdateCandidateInfo(invite.email, invite.name, invite.currentRole, invite.yearsExperience, invite.skills);
            
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

    private Candidate UpdateCandidateInfo(string email, string name, string? currentRole, int? yearsExperience, List<string>? skills) {
        //find skills off the db, and create new ones if needed
            List<Skill> allSkills = context.Skills.ToList();
            List<Skill> candidateSkill = new();
            if(skills != null) {
                foreach(string sk in skills) {
                    Skill? skill = allSkills.FirstOrDefault(s => s.name.ToLower() == sk.ToLower());
                    candidateSkill.Add(skill ?? new Skill{name = sk});
                }
            }
            //first, look up if we already have this user in OUR db by email
            Candidate? candidate = context.Candidates.Include(candidate => candidate.Skill).FirstOrDefault(c => c.email == email);

            //if they don't exist in db, then create new candidate obj
            if(candidate == null) {
                candidate = new Candidate(name, email) {
                    currentRole = currentRole,
                    yearsExperience = yearsExperience,
                    Skill = candidateSkill
                };
                context.Add(candidate);
            }
            //we already have the user, we are update their info
            else {
                candidate.name = String.IsNullOrWhiteSpace(name) ? name : candidate.name;
                candidate.Skill = candidateSkill.Count <= 0 ? candidateSkill : candidate.Skill;
                candidate.currentRole = String.IsNullOrWhiteSpace(currentRole) ? currentRole : candidate.currentRole;
                candidate.yearsExperience = yearsExperience != 0 ? yearsExperience : candidate.yearsExperience;
                context.Update(candidate);
            }
            context.SaveChanges();
            context.ChangeTracker.Clear();

            return candidate;
    }

    public async Task<HttpResponseMessage> ReattemptTestById(string origin, string host, int testInvitationId, ReattemptRequest req){

        req.setCallBackUrl(host);
        req.redirectUrl = origin + configuration.GetValue(typeof(string), "IMocha:RedirectUrlPath");

        JsonContent content = JsonContent.Create<ReattemptRequest>(req);

        Log.Information(await content.ReadAsStringAsync());

        HttpResponseMessage response = await http.PostAsync($"invitations/{testInvitationId}/reattempt", content);
        string responseStr = await response.Content.ReadAsStringAsync();

        if(response.IsSuccessStatusCode){
            ReattemptDTO resp = JsonSerializer.Deserialize<ReattemptDTO>(responseStr)!;
            Log.Information($"Obtained reattempt for id {testInvitationId}, the new id is {resp.testInvitationId}");
            
            //Replacing the URL
            resp.testUrl = resp.testUrl.Replace("test.imocha.io", "coding.revature.com");
            
            HttpResponseMessage testResponse = await GetTestAttemptById((int) testInvitationId);

            if(!testResponse.IsSuccessStatusCode){
                log.LogError("MailchimpService - Failed to retrieve test information from IMocha, email sequence aborted.");
            }

            CandidateTestReport report = JsonSerializer.Deserialize<CandidateTestReport>(await testResponse.Content.ReadAsStreamAsync()) ?? new CandidateTestReport();

            //Sending the mailchimp message
            await mcs.sendMailchimpMessageAsync(req.startDateTime.ToString() ?? "", req.endDateTime.ToString() ?? "", resp.testUrl, report.candidateName, report.candidateEmail, report.testName, true);

            return response;
        }
        else{
            Log.Error($"Imocha responded with an error getting re-attempt for id {testInvitationId}", response.StatusCode, responseStr);
            Log.Error(responseStr);
            IMochaError resp = JsonSerializer.Deserialize<IMochaError>(responseStr)!;
            return response;
        }

    }

}
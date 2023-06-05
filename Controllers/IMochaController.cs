using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using IPR_BE.Services;
using IPR_BE.Models;
using IPR_BE.Models.TestReqResp;
using IPR_BE.Models.Invite;
using IPR_BE.Models.InviteDB;
using IPR_BE.DataAccess;
using System.Text.Json;

namespace IPR_BE.Controllers;

[ApiController]
[Route("[controller]")]
public class IMochaController : ControllerBase {
    private readonly SMTPService smtp;
    private readonly InterviewBotService ibService;
    private readonly IMochaService imochaService;
    private readonly TestReportDbContext context;
    private InterviewBotRepo ibot;
    private readonly IConfiguration config;
    private HttpClient http;
    public IMochaController(IConfiguration iConfig, SMTPService smtpService, TestReportDbContext dbcontext, InterviewBotRepo interviewBot, InterviewBotService ibService, IMochaService imochaService) {
        context = dbcontext;
        //grabbing appropriate configuration from appsettings.json
        config = iConfig;
        smtp = smtpService;
        ibot = interviewBot;
        this.ibService = ibService;
        this.imochaService = imochaService;

        //initialize HttpClient and set the BaseAddress and add the X-API-KEY header 
        http = new HttpClient();
        http.DefaultRequestHeaders.Add("X-API-KEY", iConfig.GetValue<string>("IMocha:ApiKey"));
        http.BaseAddress = new Uri(iConfig.GetValue<string>("IMocha:BaseURL") ?? "");
    }

    /// <summary>
    /// Gets all tests from iMocha API, by default it gets all Interview Prep Video Tests, up to 100.
    /// </summary>
    /// <param name="pageNo">Not Required, default 1</param>
    /// <param name="pageSize">Not Required, default 1000</param>
    /// <param name="labelsFilter">Not Required, default "Interview Prep Video Tests"</param>
    /// <returns>List of iMocha tests retrieved</returns>
    [HttpGet("tests")]
    public async Task<IMochaTestDTO> GetAllTests(int? pageNo = 1, int? pageSize = 100, string? labelsFilter = "Interview Prep Video Tests") {
        return await imochaService.GetAllTests();
    }

    /// <summary>
    /// Gets A test by Id
    /// </summary>
    /// <param name="testId">Required route parameter</param>
    /// <returns>IMochaTestDetail Object</returns>
    [HttpGet("tests/{testId}")]
    public async Task<IMochaTestDetailDTO> GetTestById(int testId) {
        string response = await http.GetStringAsync($"tests/{testId}");
        return JsonSerializer.Deserialize<IMochaTestDetailDTO>(response) ?? new IMochaTestDetailDTO();
    }

    [HttpPost("tests/attempts")]
    public async Task<IActionResult> GetTestAttempts([FromBody] TestAttemptRequestBody reqBody) {
        HttpResponseMessage response = await http.PostAsync("candidates/testattempts?state=completed", JsonContent.Create<TestAttemptRequestBody>(reqBody));
        if(response.IsSuccessStatusCode) {
            var responsebody = await response.Content.ReadAsStringAsync();
            TestAttemptsListResponseBody deserialized = JsonSerializer.Deserialize<TestAttemptsListResponseBody>(await response.Content.ReadAsStringAsync());

            await ibService.ProcessNewTestAttempts(deserialized.result.testAttempts);

            return Ok(deserialized.result.testAttempts);
        }
        else {
            Console.WriteLine("What happened?");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Get CandidateTestReport by testInvitationId
    /// </summary>
    /// <param name="testInvitationId"></param>
    /// <returns></returns>
    [HttpGet("reports/{testInvitationId}")]
    public async Task<CandidateTestReport> GetTestAttempt(int testInvitationId){
        return await imochaService.GetTestAttemptById(testInvitationId);
    }

    /// <summary>
    /// Gets the video url using hidden imocha endpoint. very fun. sends a post.
    /// </summary>
    /// <param name="testInvitationId"></param>
    /// <returns></returns>
    [HttpGet("reports/{testInvitationId}/questions")]
    public async Task<TestResultDTO> GetVidTestAttempt(int testInvitationId){
        HttpResponseMessage response = new HttpResponseMessage();
        TestResultDTO result;
        Dictionary<int,decimal> questionIds= new Dictionary<int,decimal>();

        //Getting the scores, this one hurt
        TestDetail test;
        test = ibot.GetTestByID(testInvitationId);

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
    [HttpPost("invite")]
    public async Task InviteCandidates([FromBody] CandidateInvitation invite) {
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
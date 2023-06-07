using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using IPR_BE.Services;
using IPR_BE.Models;
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
        var responsebody = await response.Content.ReadAsStringAsync();
        if(response.IsSuccessStatusCode) {
            TestAttemptsListResponseBody deserialized = JsonSerializer.Deserialize<TestAttemptsListResponseBody>(await response.Content.ReadAsStringAsync());

            await ibService.ProcessNewTestAttempts(deserialized.result.testAttempts);

            return Ok(deserialized.result.testAttempts);
        }
        else {
            return StatusCode(((int)response.StatusCode), responsebody);
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
        return await imochaService.GetVidTestAttempt(testInvitationId);
    }

    /// <summary>
    /// Invites Candidates then does a few more things
    /// 1. Pings iMocha for TestInvitationURL (prompts iMocha to send email on behalf of us)
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
        await imochaService.InviteCandidates(invite);
    }
}
using Microsoft.AspNetCore.Mvc;
using IPR_BE.Services;
using IPR_BE.Models;
using IPR_BE.DataAccess;
using System.Text.Json;
using Serilog;

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
    private readonly ILogger<IMochaController> log;

    public IMochaController(IConfiguration iConfig, SMTPService smtpService, TestReportDbContext dbcontext, InterviewBotRepo interviewBot, InterviewBotService ibService, IMochaService imochaService, ILogger<IMochaController> log) {
        context = dbcontext;
        //grabbing appropriate configuration from appsettings.json
        config = iConfig;
        smtp = smtpService;
        ibot = interviewBot;
        this.ibService = ibService;
        this.imochaService = imochaService;
        this.log = log;

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
    public async Task<IActionResult> GetAllTests(int? pageNo = 1, int? pageSize = 100, string? labelsFilter = "Interview Prep Video Tests") {
        //Getting direct response from IMocha API
        HttpResponseMessage response = await imochaService.GetAllTests();
    
        //Reading it as a string 
        var responseBody = await response.Content.ReadAsStringAsync();

        //If it's not some error, we deserailize the object and send it.
        if(response.IsSuccessStatusCode) {
            IMochaTestDTO? tests = JsonSerializer.Deserialize<IMochaTestDTO>(await response.Content.ReadAsStringAsync());
            log.LogInformation("Retrieved {@Count} tests from IMocha", tests.tests.Count);
            return Ok(tests);
        }else{
            log.LogError("Failed to retrieve tests from IMocha", responseBody);
            return StatusCode(((int)response.StatusCode), responseBody);
        }
    }

    /// <summary>
    /// Gets A test by Id
    /// </summary>
    /// <param name="testId">Required route parameter</param>
    /// <returns>IMochaTestDetail Object</returns>
    [HttpGet("tests/{testId}")]
    public async Task<IActionResult> GetTestById(int testId) {
        HttpResponseMessage response = await http.GetAsync($"tests/{testId}");
        string responseBody = await response.Content.ReadAsStringAsync();

        if(response.IsSuccessStatusCode){
            IMochaTestDetailDTO? testDetail = JsonSerializer.Deserialize<IMochaTestDetailDTO>(await response.Content.ReadAsStringAsync());
            log.LogInformation("Retrieved details for testID: {@TestDetail}", testDetail);
            return Ok(testDetail);
        }else{
            log.LogError($"Failed to retrieved details for testID: {testId}, " + responseBody);
            return StatusCode(((int)response.StatusCode), responseBody);
        }
    }

    [HttpPost("tests/attempts")]
    public async Task<IActionResult> GetTestAttempts([FromBody] TestAttemptRequestBody reqBody) {
        HttpResponseMessage response = await http.PostAsync("candidates/testattempts?state=completed", JsonContent.Create<TestAttemptRequestBody>(reqBody));

        var responseBody = await response.Content.ReadAsStringAsync();
        if(response.IsSuccessStatusCode) {
            
            TestAttemptsListResponseBody deserialized = JsonSerializer.Deserialize<TestAttemptsListResponseBody>(await response.Content.ReadAsStringAsync());
            await ibService.ProcessNewTestAttempts(deserialized.result.testAttempts);

            log.LogInformation("Retrieved {Count} test attempts.", deserialized.result.testAttempts.Count);

            return Ok(deserialized.result.testAttempts);
        }
        else {
            log.LogError("Failed to retrieve tests, " + responseBody);
            return StatusCode(((int)response.StatusCode), responseBody);

        }
    }

    /// <summary>
    /// Get CandidateTestReport by testInvitationId
    /// </summary>
    /// <param name="testInvitationId"></param>
    /// <returns></returns>
    [HttpGet("reports/{testInvitationId}")]
    public async Task<ActionResult<CandidateTestReport>> GetTestAttempt(int testInvitationId){
        HttpResponseMessage response = await imochaService.GetTestAttemptById(testInvitationId);
        var responseBody = await response.Content.ReadAsStringAsync();

        if(response.IsSuccessStatusCode){
            CandidateTestReport report = JsonSerializer.Deserialize<CandidateTestReport>(await response.Content.ReadAsStreamAsync()) ?? new CandidateTestReport();
            TestDetail ibotTestScore = ibot.GetTestByID(testInvitationId);
            report.score = ibotTestScore.scoreSum;
            log.LogInformation($"Retrieved report for testInvite: {testInvitationId}");
            return Ok(report);
        }else {
            log.LogError("Failed to retrieve test " + responseBody);
            return StatusCode(((int)response.StatusCode), responseBody);
        }
        //return await imochaService.GetTestAttemptById(testInvitationId);
    }

    /// <summary>
    /// Gets the video url using hidden imocha endpoint. very fun. sends a post.
    /// Logging for this one is done in the IMochaService GetVidTestAttempt method.
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
    /// <returns>whatever iMocha responds with</returns>
    [HttpPost("invite")]
    public async Task<IActionResult> InviteCandidates([FromBody] CandidateInvitation invite) {
        Log.Information("Inviting candidate to imocha with the following body {0}", invite);

        HttpResponseMessage imochaResponse = await imochaService.InviteCandidates(invite);
        IMochaTestInviteResponse responseBody = JsonSerializer.Deserialize<IMochaTestInviteResponse>(await imochaResponse.Content.ReadAsStringAsync()) ?? new();
        Log.Information("iMocha response: {0}", responseBody);
        return StatusCode((int) imochaResponse.StatusCode, responseBody);
    }
}
using Microsoft.AspNetCore.Mvc;
using IPR_BE.Models.DTO;
using IPR_BE.DataAccess;
using IPR_BE.Models.TestReport;

namespace IPR_BE.Controllers;

[ApiController]
[Route("[controller]")]
public class InterviewBotController : ControllerBase {
    private HttpClient http;
    private TestReportDbContext context;

    private readonly InterviewBotRepo ibRepo;
    public InterviewBotController(IConfiguration iConfig, TestReportDbContext context, InterviewBotRepo ibRepo) {

        //initialize HttpClient and set the BaseAddress
        http = new HttpClient();
        http.BaseAddress = new Uri(iConfig.GetValue<string>("InterviewBot:BaseURL") ?? "");
        this.ibRepo = ibRepo;
        this.context = context; 
    }

    // {
        // testAttemptId: 134239847,
        // testAttemptScore: ___
        // questions: [
        //     {
        //         questionId: 213293874,
        //         score: 92
        //     },
        //     {
        //         questionId: 2348,
        //         score: 33
        //     }
        // ]
    // }
    [HttpGet("{attemptId}")]
    public void GetQuestionScoresByAttemptId(int attemptId) {
        this.ibRepo.GetTheMostRecent(10);
    }

    

    /// <summary>
    /// Callback url from interview bot, whenever the processing of the video is done
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    [HttpPost("postprocessing")]
    public void ProcessInterviewBotResponse([FromBody] Object obj) {
        InterviewBotLog log = new InterviewBotLog(){
            message = "Got Callback from interview bot" + obj.ToString()
        };
        this.context.Add(log);
        this.context.SaveChanges();
    }

    /// <summary>
    /// this endpoint will be provided as callback url when inviting candidates to imocha test. Once candidate starts/completes the test, then imocha will call this endpoint which we can subsequently process the result
    /// </summary>
    /// <param name="cb">Callbackbody object Imocha provides for us</param>
    /// <returns></returns>
    [HttpPost("imocha")]
    public void ProcessIMochaResponse([FromBody] Object obj) {
        InterviewBotLog log = new InterviewBotLog(){
            message = "Got Callback from imocha api" + obj.ToString()
        };
        this.context.Add(log);
        this.context.SaveChanges();
        // if(cb.Status == "Complete") {
        //     Tuple<int, string> contentBody = new Tuple<int, string>(cb.TestInvitationId, cb.CandidateEmailId); 
        //     JsonContent content = JsonContent.Create<Tuple<int, string>>(contentBody);
        //     HttpResponseMessage response = await http.PostAsync("", content);

        //     Console.WriteLine(await response.Content.ReadAsStringAsync());
        // }
    }
}
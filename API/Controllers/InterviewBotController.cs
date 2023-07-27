using Microsoft.AspNetCore.Mvc;
using IPR_BE.DataAccess;
using IPR_BE.Models;
using IPR_BE.Services;
using Serilog;


namespace IPR_BE.Controllers;

[ApiController]
[Route("[controller]")]
public class InterviewBotController : ControllerBase {
    private HttpClient http;
    private TestReportDbContext context;

    private InterviewBotService ibService;
    private readonly InterviewBotRepo ibRepo;
    public InterviewBotController(IConfiguration iConfig, TestReportDbContext context, InterviewBotRepo ibRepo, InterviewBotService ibService) {

        //initialize HttpClient and set the BaseAddress
        http = new HttpClient();
        http.BaseAddress = new Uri(iConfig.GetValue<string>("InterviewBot:BaseURL") ?? "");
        this.ibRepo = ibRepo;
        this.ibService = ibService;
        this.context = context; 
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
    public async Task ProcessIMochaResponse([FromBody] IMochaCallbackBody cbBody) {
        Log.Information("Got callback from imocha {response}", cbBody.ToString());
        InterviewBotLog log = new InterviewBotLog(){
            message = "Got Callback from imocha api" + cbBody.ToString()
        };
        this.context.Add(log);
        this.context.SaveChanges();

        if(cbBody.Status == "Complete"){
            Dictionary<long, string> contentBody = new();
            contentBody.Add(cbBody.TestInvitationId, cbBody.CandidateEmailId);
            
            await ibService.SendProcessRequest(contentBody);
        }
    }
}
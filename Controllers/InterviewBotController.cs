using Microsoft.AspNetCore.Mvc;
using IPR_BE.Models.DTO;

namespace IPR_BE.Controllers;

[ApiController]
[Route("[controller]")]
public class InterviewBotController : ControllerBase {
    private HttpClient http;
    public InterviewBotController(IConfiguration iConfig) {

        //initialize HttpClient and set the BaseAddress
        http = new HttpClient();
        http.BaseAddress = new Uri(iConfig.GetValue<string>("InterviewBot:BaseURL") ?? "");
    }

    /// <summary>
    /// Callback url from interview bot, whenever the processing of the video is done
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    [HttpPost("postprocessing")]
    public async Task ProcessInterviewBotResponse([FromBody] Object obj) {
        Console.WriteLine("got a callback from interview bot resposne");
    }

    /// <summary>
    /// this endpoint will be provided as callback url when inviting candidates to imocha test. Once candidate starts/completes the test, then imocha will call this endpoint which we can subsequently process the result
    /// </summary>
    /// <param name="cb">Callbackbody object Imocha provides for us</param>
    /// <returns></returns>
    [HttpPost("imocha")]
    public async Task ProcessIMochaResponse([FromBody] IMochaCallbackBody cb) {
        Console.WriteLine(cb);
        /*
        1. check if the test has been completed (if it's not, then we do nothing)
        2. take the invidation id and candidate email id and send it to the Interview Bot API
        3. ???
        4. Profit
        
        */
        
        if(cb.Status == "Complete") {
            Tuple<int, string> contentBody = new Tuple<int, string>(cb.TestInvitationId, cb.CandidateEmailId); 
            JsonContent content = JsonContent.Create<Tuple<int, string>>(contentBody);
            HttpResponseMessage response = await http.PostAsync("", content);

            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
    }


}
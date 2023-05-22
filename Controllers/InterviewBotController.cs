using Microsoft.AspNetCore.Mvc;

namespace IPR_BE.Controllers;

[ApiController]
[Route("[controller]")]
public class InterviewBotController : ControllerBase {
    private readonly IConfiguration configuration;
    private HttpClient http;
    public InterviewBotController(IConfiguration iConfig) {
        //grabbing appropriate configuration from appsettings.json
        configuration = iConfig;

        //initialize HttpClient and set the BaseAddress
        http = new HttpClient();
        http.BaseAddress = new Uri(configuration.GetValue<string>("InterviewBot:BaseURL") ?? "");
    }
}
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;

namespace IPR_BE.Controllers;

[ApiController]
[Route("[controller]")]
public class IMochaController : ControllerBase {
    private readonly IConfiguration configuration;
    public IMochaController(IConfiguration iConfig) {
        configuration = iConfig;
    }
    [HttpGet]
    public async Task GetAllTests() {
        HttpClient http = new HttpClient();
        http.DefaultRequestHeaders.Add("X-API-KEY", configuration.GetValue<string>("IMocha:ApiKey"));
        string str = await http.GetStringAsync("https://apiv3.imocha.io/v3/tests");
        Console.WriteLine(str);
    }
}
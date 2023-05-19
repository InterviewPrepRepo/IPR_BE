using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using IPR_BE.Models;
using System.Text.Json;

namespace IPR_BE.Controllers;

[ApiController]
[Route("[controller]")]
public class IMochaController : ControllerBase {
    private readonly IConfiguration configuration;
    private HttpClient http;
    public IMochaController(IConfiguration iConfig) {
        //grabbing appropriate configuration from appsettings.json
        configuration = iConfig;

        //initialize HttpClient and set the BaseAddress and add the X-API-KEY header 
        http = new HttpClient();
        http.DefaultRequestHeaders.Add("X-API-KEY", configuration.GetValue<string>("IMocha:ApiKey"));
        http.BaseAddress = new Uri(configuration.GetValue<string>("IMocha:BaseURL"));
    }

    [HttpGet]
    public async Task<IMochaTestDTO> GetAllTests() {
        string str = await http.GetStringAsync("tests");
        return JsonSerializer.Deserialize<IMochaTestDTO>(str) ?? new IMochaTestDTO();
    }
}
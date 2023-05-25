using IPR_BE.Models.DTO;
using System.Text.Json;

namespace IPR_BE.Services;

public class IMochaService {

    private HttpClient http;
    private readonly IConfiguration config;

    public IMochaService(IConfiguration iConfig) {
        config = iConfig;

        //initialize HttpClient and set the BaseAddress and add the X-API-KEY header 
        http = new HttpClient();
        http.DefaultRequestHeaders.Add("X-API-KEY", iConfig.GetValue<string>("IMocha:ApiKey"));
        http.BaseAddress = new Uri(iConfig.GetValue<string>("IMocha:BaseURL") ?? "");
    }

    public async Task<IMochaTestDTO> GetAllTests(int? pageNo = 1, int? pageSize = 100, string? labelsFilter= "Interview Prep Video Tests") {

        string response = await http.GetStringAsync($"tests?pageNo={pageNo}&pageSize={pageSize}&labelsFilter={labelsFilter}");
        return JsonSerializer.Deserialize<IMochaTestDTO>(response) ?? new IMochaTestDTO();
    }
}
using IPR_BE.Models;
using System.Text.Json;
using IPR_BE.DataAccess;

namespace IPR_BE.Services;

public class IMochaService {

    private HttpClient http;
    private readonly IConfiguration config;
    private readonly InterviewBotRepo ibrepo;

    public IMochaService(IConfiguration iConfig, InterviewBotRepo ibrepo) {
        config = iConfig;
        this.ibrepo = ibrepo;

        //initialize HttpClient and set the BaseAddress and add the X-API-KEY header 
        http = new HttpClient();
        http.DefaultRequestHeaders.Add("X-API-KEY", iConfig.GetValue<string>("IMocha:ApiKey"));
        http.BaseAddress = new Uri(iConfig.GetValue<string>("IMocha:BaseURL") ?? "");
    }

    public async Task<IMochaTestDTO> GetAllTests(int? pageNo = 1, int? pageSize = 100, string? labelsFilter= "Interview Prep Video Tests") {

        string response = await http.GetStringAsync($"tests?pageNo={pageNo}&pageSize={pageSize}&labelsFilter={labelsFilter}");
        return JsonSerializer.Deserialize<IMochaTestDTO>(response) ?? new IMochaTestDTO();
    }

    public async Task<CandidateTestReport> GetTestAttemptById(int testInvitationId){
        string str = await http.GetStringAsync($"reports/{testInvitationId}");
        CandidateTestReport report = JsonSerializer.Deserialize<CandidateTestReport>(str) ?? new CandidateTestReport();
        TestDetail ibotTestScore = ibrepo.GetTestByID(testInvitationId);
        report.score = ibotTestScore.scoreSum;
        return report;
    }
}
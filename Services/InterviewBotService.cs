using System.Text.Json;
using IPR_BE.DataAccess;
using IPR_BE.Models;
using Serilog;

namespace IPR_BE.Services;

public class InterviewBotService {
    private readonly InterviewBotRepo ibrepo;
    private readonly IMochaService imocha;
    private IConfiguration config;
    private readonly TestReportDbContext context;
    public InterviewBotService(InterviewBotRepo ibrepo, IConfiguration iconfig, IMochaService imocha, TestReportDbContext dbcontext) {
        this.ibrepo = ibrepo;
        config = iconfig;
        this.imocha = imocha;
        context = dbcontext;
    }

    //This method is called whenever we're getting all completed test attempts from iMocha. We first filter the test attempts to have only the completed attempts that are associated with video tests. Then we compare that list with interview bot db's list, and any new attempts we see from imocha, we'll send it over to interviewbot to be analyzed.
    public async Task ProcessNewTestAttempts(List<TestAttemptsListResponseBody.TestAttemptShortened> attemptsFromiMocha) {
        //First, filter only the attempts associated with the video tests
        HttpResponseMessage response = await imocha.GetAllTests();
        
        IMochaTestDTO? tests = JsonSerializer.Deserialize<IMochaTestDTO>(await response.Content.ReadAsStringAsync());

        List<IMochaTest> videoTests = tests.tests;

        HashSet<long> videoTestsId = new();
        foreach(IMochaTest test in videoTests) {
            videoTestsId.Add(test.testId);
        }

        //Filtering only the test attempt ids that are associated with video tests 
        HashSet<long> allVideoTestAttemptIds = attemptsFromiMocha.Where(a => a.teststatus == "Complete" && videoTestsId.Contains(a.testId)).Select(a => a.testInvitationId).ToHashSet();
        
        //next, get the ids that already exist in interview bot db
        HashSet<long> ibAttempts = ibrepo.GetAllUniqueTestAttemptIds();

        HashSet<long> attemptsToProcess = GetUnprocessedAttemptIds(allVideoTestAttemptIds, ibAttempts);

        //Assemble the json the way API wants, and Send it!
        Dictionary<long, string> contentBody = new();

        foreach(long attemptId in attemptsToProcess) {
            string candidateEmail = attemptsFromiMocha.FirstOrDefault(a => a.testInvitationId == attemptId).email;
            contentBody.Add(attemptId, candidateEmail);

            LogToDB($"Sending AttemptId: {attemptId} Email: {candidateEmail}");
        }
        
        await SendProcessRequest(contentBody);
    }


    /// <summary>
    /// This method sends processing request to interview bot application. This is used in two places, 1. where when we get all test attempts in admin view we filter for all attempts that haven't gotten processed and send the request over, 2. when we get a callback from iMocha that the test has been completed, we use this to let interview bot know
    /// </summary>
    /// <param name="contentBody">Dictionary of testAttemptId(int64) as key and userEmail(string) as value</param>
    /// <returns></returns>
    public async Task SendProcessRequest(Dictionary<long, string> contentBody) {
        HttpClient http = new(){
            BaseAddress = new Uri(config.GetValue<string>("InterviewBot:BaseURL") ?? "")
        };

        Log.Information("sending interview bot processing request with the following body {contentBody}", contentBody);

        await http.PostAsync("", JsonContent.Create(contentBody));
    }

    private HashSet<long> GetUnprocessedAttemptIds(HashSet<long> imochas, HashSet<long> interviewbots) {
        imochas.ExceptWith(interviewbots);
        return imochas;
    }

    public void LogToDB(string msg) {
        InterviewBotLog log = new InterviewBotLog(){
            message = msg
        };
        this.context.Add(log);
        this.context.SaveChanges();
    }

}
using IPR_BE.DataAccess;
using IPR_BE.Models;
using IPR_BE.Models.InviteDB;
using IPR_BE.Models.TestReqResp;

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
        List<IMochaTest> videoTests = (await imocha.GetAllTests()).tests;
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

        HttpClient http = new(){
            BaseAddress = new Uri(config.GetValue<string>("InterviewBot:BaseURL") ?? "")
        };

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
using IPR_BE.Models;
using System.Text.Json;
using IPR_BE.DataAccess;
using Microsoft.EntityFrameworkCore;
using Serilog;
using IPR_BE.Migrations;

namespace IPR_BE.Services;

public class GradingService {

    private HttpClient http;
    private readonly IConfiguration configuration;
    private readonly InterviewBotRepo ibrepo;
    private readonly TestReportDbContext context;
    private readonly ILogger<IMochaService> log;

    public GradingService(IConfiguration iConfig, InterviewBotRepo ibrepo, TestReportDbContext dbcontext,
        ILogger<IMochaService> log) {
        this.ibrepo = ibrepo;
        configuration = iConfig;
        context = dbcontext;
        this.log = log;

        //initialize HttpClient and set the BaseAddress and add the X-API-KEY header 
        http = new HttpClient();
        http.DefaultRequestHeaders.Add("X-API-KEY", iConfig.GetValue<string>("IMocha:ApiKey"));
        http.BaseAddress = new Uri(iConfig.GetValue<string>("IMocha:BaseURL") ?? "");
    }

    public List<GradedQuestion> ManualGrade(List<GradedQuestion> manualGrades){
        
        try {
            log.LogInformation("Persisting user updated grade to db, {0}", manualGrades);
            // persisting to DB
            
            context.UpdateRange(manualGrades);
            context.SaveChanges();
            context.ChangeTracker.Clear();

            //return updated objects to front end
            return manualGrades;
        }
        catch (DbUpdateException ex) {
            // any error
            log.LogError(ex.ToString());
            throw ex;
        }
         
    }

    public List<GradedQuestion> GetGradedQuestions(long testAttemptId){
        try {
            return context.GradedQuestions.Where(q => q.testAttempt == testAttemptId).ToList();
        }
        catch (Exception ex) {
            // any error
            log.LogError(ex.ToString());
            throw ex;
        }
    }
}
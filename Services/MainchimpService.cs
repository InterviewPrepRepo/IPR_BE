using System.Net;
using System.Text.Json;
using IPR_BE.Models;

namespace IPR_BE.Services;

public class MailchimpService {

    private readonly string key;
    private readonly IConfiguration config;
    private string? supportEmail;
    private HttpClient http;
    private readonly ILogger<MailchimpService> log;
    private readonly IMochaService ims;

    public MailchimpService(IConfiguration iConfig, ILogger<MailchimpService> log, IMochaService ims){
        config = iConfig;
        
        //Configuring stuff from appsettings.json
        http = new HttpClient();
        this.supportEmail = config.GetValue<string?>("MailChimp:SupportEmail" ?? "");
        this.http.BaseAddress = new Uri(iConfig.GetValue<string>("MailChimp:BaseURI") ?? "");

        //getting mailchimp key from appsettings
        this.key = config.GetValue<string?>("MailChimp:Key") ?? "";

        //Adding logging
        this.log = log;

        //Adding imocha service
        this.ims = ims;
    }
    public void sendInviteMessage(long testAttemptId){
        
    }

    /// <summary>
    /// Called from iMochaService.ReattemptTestById to send a mailchimp email using the 
    /// reattempt template. 
    /// </summary>
    /// <param name="testAttemptId"></param>
    /// <param name="startDateTime"></param>
    /// <param name="endDateTime"></param>
    /// <param name="testUrl"></param>
    /// <returns></returns>
    public async Task sendReattemptMessageAsync(long testAttemptId, string startDateTime, string endDateTime, string testUrl){
        //Getting testinfo from imocha api using our service
        HttpResponseMessage iMochaResponse = await ims.GetTestAttemptById((int) testAttemptId);
        //Pulling this testreport to get some more info
        CandidateTestReport report = JsonSerializer.Deserialize<CandidateTestReport>(await iMochaResponse.Content.ReadAsStreamAsync()) ?? new CandidateTestReport();
        
        //Creating bare MailchimpRequest and response objects
        MailchimpRequest mailReq = new MailchimpRequest();
        
        //Setting Mailchimp req obj fields
        mailReq.key = this.key;
        mailReq.template_name = config.GetValue<string>("MailChimp:ReattemptTemplate");
        mailReq.template_content.Add(new TemplateContent("student_first_name", report.candidateName));
        mailReq.template_content.Add(new TemplateContent("assessment_name", report.testName));
        mailReq.template_content.Add(new TemplateContent("assessment_link", testUrl));
        mailReq.template_content.Add(new TemplateContent("start_datetime", startDateTime));
        mailReq.template_content.Add(new TemplateContent("end", endDateTime));
        mailReq.template_content.Add(new TemplateContent("support_email", this.supportEmail));
        mailReq.message.to.Add(new MailchimpTo(report.candidateEmail, report.candidateName));

        //testing delete later
        Console.WriteLine(mailReq.ToString());

        var mailchimpResponse = await http.PostAsJsonAsync<MailchimpRequest>("messages/send-template", mailReq);

        if(mailchimpResponse.IsSuccessStatusCode){
            Console.WriteLine(await mailchimpResponse.Content.ReadAsStringAsync());
        }
    }

}
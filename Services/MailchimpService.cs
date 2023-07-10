using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using IPR_BE.Models;

namespace IPR_BE.Services;

public class MailchimpService {

    private readonly string key;
    private readonly IConfiguration config;
    private string supportEmail;
    private HttpClient http;
    private readonly ILogger<MailchimpService> log;

    public MailchimpService(IConfiguration iConfig, ILogger<MailchimpService> log){
        config = iConfig;
        
        //Configuring stuff from appsettings.json
        http = new HttpClient();
        this.supportEmail = config.GetValue<string?>("Mailchimp:SupportEmail" ?? "");
        this.http.BaseAddress = new Uri(iConfig.GetValue<string>("Mailchimp:BaseURI") ?? "");

        //getting mailchimp key from appsettings
        this.key = config.GetValue<string?>("Mailchimp:Key") ?? "";

        //Adding logging
        this.log = log;

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
    public async Task sendMailchimpMessageAsync(string startDateTime, string endDateTime, string testUrl, string candidateName, string candidateEmail, string testName, bool isReattempt){
    //Creating MailchimpRequest
        MailchimpRequest mailReq;

        //ToDo: remove start/end datetime
        if(isReattempt)
        {
            mailReq = new MailchimpRequest(this.key, config.GetValue<string>("Mailchimp:ReattemptTemplate") ?? "", candidateName,
                candidateEmail, testName, testUrl, startDateTime, endDateTime, this.supportEmail);
        }else{
            mailReq = new MailchimpRequest(this.key, config.GetValue<string>("Mailchimp:InviteTemplate") ?? "", candidateName,
                candidateEmail, testName, testUrl, startDateTime, endDateTime, this.supportEmail);
        }

        //Sending the email request to mailchimp API
        JsonContent json = JsonContent.Create<MailchimpRequest>(mailReq);
        log.LogInformation("Sending http request to mailchimp with the following body {0}", await json.ReadAsStringAsync());
        var mailchimpResponse = await http.PostAsJsonAsync("messages/send-template", json);

        //Appropriate logging
        if(mailchimpResponse.IsSuccessStatusCode){
            log.LogInformation($"Successfully sent a request email to {candidateEmail} using mailchimp");
        } else {
            log.LogError($"Failed to send a request email to {candidateEmail} using mailchimp");
            log.LogError(mailchimpResponse.Content.ToString());
        }
    }

}
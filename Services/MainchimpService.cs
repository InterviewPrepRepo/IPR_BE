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
        this.supportEmail = config.GetValue<string?>("MailChimp:SupportEmail" ?? "");
        this.http.BaseAddress = new Uri(iConfig.GetValue<string>("MailChimp:BaseURI") ?? "");

        //getting mailchimp key from appsettings
        this.key = config.GetValue<string?>("MailChimp:Key") ?? "";

        //Adding logging
        this.log = log;

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
    public async Task sendReattemptMessageAsync(long testAttemptId, string startDateTime, string endDateTime, string testUrl, string candidateName, string candidateEmail, string testName){
        //Creating MailchimpRequest
        MailchimpRequest mailReq = new MailchimpRequest(this.key, config.GetValue<string>("MailChimp:ReattemptTemplate"), candidateName,
            candidateEmail, testName, testUrl, startDateTime, endDateTime, this.supportEmail);
        
        //testing delete later
        Console.WriteLine(JsonSerializer.Serialize(mailReq).ToString());

        //Sending the email request to mailchimp API
        JsonContent json = JsonContent.Create<MailchimpRequest>(mailReq);

        var mailchimpResponse = await http.PostAsJsonAsync("messages/send-template", mailReq);

        //Appropriate logging
        if(mailchimpResponse.IsSuccessStatusCode){
            Console.WriteLine(await mailchimpResponse.Content.ReadAsStringAsync());
            log.LogInformation($"Successfully sent a reattempt request email to {candidateEmail} using mailchimp");
        } else {
            log.LogError($"Failed to send a re-attempt email to {candidateEmail} using mailchimp");
            log.LogError(mailchimpResponse.Content.ToString());
        }
    }

}
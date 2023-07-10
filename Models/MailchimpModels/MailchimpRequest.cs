using System.Text.Json.Serialization;

namespace IPR_BE.Models;

/// <summary>
/// Used to send the mailchimp send template email request
/// </summary>
public class MailchimpRequest 
{
    public string? key {get; set;}
    public string? template_name {get; set;}

    [JsonRequired]
    public TemplateContent[] template_content { get; set; }
    public MailchimpMessage message {get; set;}

    public MailchimpRequest(string key, string template_name, string candidateName, string candidateEmail, 
    string testName, string testUrl, string startDateTime, string endDateTime, string support_email)
    {
        this.key = key;
        this.template_name = template_name;

        this.message = new MailchimpMessage(candidateEmail, candidateName, testName, testUrl, startDateTime,
            endDateTime, support_email);
        
        template_content = new TemplateContent[1];
    }

}

public class GlobalMergeVars
{
    public string? name {get; set;}
    public string? content {get; set;}
    public GlobalMergeVars(string name, string content){
        this.name = name;
        this.content = content;
    }
}

public class TemplateContent
{
    public string? name {get; set;}
    public string? content {get; set;}
    public TemplateContent(string name, string content){
        this.name = name;
        this.content = content;
    }
}



public class MailchimpMessage 
{
    public MailchimpTo[] to {get; set;}
    public GlobalMergeVars[] global_merge_vars {get; set;}
    
    [JsonRequired]
    public bool merge { get; set; } = true;
    
    [JsonRequired]
    public bool track_opens { get; set; } = true;

    [JsonRequired]
    public bool track_clicks { get; set; } = true;

    public MailchimpMessage(string candidateEmail, string candidateName, string testName, string testUrl, string startDateTime,
        string endDateTime, string support_email)
    {
        this.to = new MailchimpTo[1] {new MailchimpTo( candidateEmail, candidateName)};
        List<GlobalMergeVars> tempList = new List<GlobalMergeVars>
        {
            new GlobalMergeVars("student_first_name", candidateName),
            new GlobalMergeVars("assessment_name", testName),
            new GlobalMergeVars("assessment_link", testUrl),
            new GlobalMergeVars("start_datetime", startDateTime),
            new GlobalMergeVars("end_datetime", endDateTime),
            new GlobalMergeVars("support_email", support_email)
        };

        this.global_merge_vars = tempList.ToArray();
    }

}


public class MailchimpTo
{
    public string? email { get; set; }
    public string? name { get; set; }

    public MailchimpTo(string email, string name){
        this.email = email;
        this.name = name;
    }
}
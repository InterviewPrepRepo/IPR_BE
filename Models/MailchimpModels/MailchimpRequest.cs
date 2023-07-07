namespace IPR_BE.Models;

/// <summary>
/// Used to send the mailchimp send template email request
/// </summary>
public class MailchimpRequest
{
    public string? key {get; set;}
    public string? template_name {get; set;}
    public TemplateContent[] template_content {get; set;}
    public MailchimpMessage message {get; set;}

    public MailchimpRequest(string key, string template_name, string candidateName, string candidateEmail, 
    string testName, string testUrl, string startDateTime, string endDateTime, string support_email)
    {
        this.key = key;
        this.template_name = template_name;

        List<TemplateContent> tempList = new List<TemplateContent>
        {
            new TemplateContent("student_first_name", candidateName),
            new TemplateContent("assessment_name", testName),
            new TemplateContent("assessment_link", testUrl),
            new TemplateContent("start_datetime", startDateTime),
            new TemplateContent("end_datetime", endDateTime),
            new TemplateContent("support_email", support_email)
        };

        this.template_content = tempList.ToArray();

        this.message = new MailchimpMessage(candidateEmail, candidateName);
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
    public bool track_opens = true;
    public bool track_clicks = true;

    public MailchimpMessage(string candidateEmail, string candidateName){
        this.to = new MailchimpTo[1] {new MailchimpTo( candidateEmail, candidateName)};
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
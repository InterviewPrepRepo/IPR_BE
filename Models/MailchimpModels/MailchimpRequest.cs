namespace IPR_BE.Models;

/// <summary>
/// Used to send the mailchimp send template email request
/// </summary>
public class MailchimpRequest
{
    public string? key {get; set;}
    public string? template_name {get; set;}
    public List<TemplateContent>? template_content = new List<TemplateContent>();
    public MailchimpMessage message = new MailchimpMessage();

    public MailchimpRequest(string key, string template_name, string candidateName, string candidateEmail, 
    string testName, string testUrl, string startDateTime, string endDateTime, string support_email)
    {
        this.key = key;
        this.template_name = template_name;
        this.template_content.Add(new TemplateContent("student_first_name", candidateName));
        this.template_content.Add(new TemplateContent("assessment_name", testName));
        this.template_content.Add(new TemplateContent("assessment_link", testUrl));
        this.template_content.Add(new TemplateContent("start_datetime", startDateTime));
        this.template_content.Add(new TemplateContent("end_datetime", endDateTime));
        this.template_content.Add(new TemplateContent("support_email", support_email));
        this.message.to.Add(new MailchimpTo(candidateEmail, candidateName));
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
    public List<MailchimpTo> to = new List<MailchimpTo>();
    public bool track_opens = true;
    public bool track_clicks = true;
}

public class MailchimpTo
{
    public string? email { get; set; }
    public string? name { get; set; }

    public MailchimpTo(string name, string email){
        this.email = email;
        this.name = name;
    }
}
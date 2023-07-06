namespace IPR_BE.Models;

/// <summary>
/// Used to send the mailchimp send template email request
/// </summary>
public class MailchimpRequest
{
    public string? key {get; set;}
    public string? template_name {get; set;}
    public List<TemplateContent>? template_content;
    public MailchimpMessage? message {get; set;}

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
    public List<MailchimpTo>? to {get; set;}
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
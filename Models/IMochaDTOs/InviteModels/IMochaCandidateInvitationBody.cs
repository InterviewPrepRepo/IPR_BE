namespace IPR_BE.Models;

/// <summary>
/// This class is the POST body to invite a new candidate to a test via iMocha api
/// Callback and Redirect URL is being read from appsettings.json file
/// By default we're having imocha send email for us, but can be overwritten to have us send our own custom prompt
/// </summary>
public class IMochaCandidateInvitationBody {
    public IMochaCandidateInvitationBody(IConfiguration config) {
        callbackUrl = config.GetValue<string>("IMocha:InviteCallBackURL")!;
        redirectUrl = config.GetValue<string>("IMocha:InviteRedirectURL")!;
        sendEmail = "yes";
    }

    public IMochaCandidateInvitationBody(IConfiguration config, string name, string email) : this(config) {
        this.email = email;
        this.name = name;
    }
    
    public string email { get; set; } = "";
    public string name { get; set; } = "";
    public string sendEmail { get; set; }
    public string callbackUrl { get; set;}
    public string redirectUrl { get; set; }

    public override string ToString()
    {
        return $"Name: {name}, Email: {email}, sendEmail: {sendEmail} callBackUrl: {callbackUrl} redirectUrl: {redirectUrl}";
    }
}
namespace IPR_BE.Models;

/// <summary>
/// This class is the POST body to invite a new candidate to a test via iMocha api
/// Callback and Redirect URL is being supplied from request headers for dynamic url generation
/// By default we're having imocha send email for us, but can be overwritten to have us send our own custom prompt
/// </summary>
public class IMochaCandidateInvitationBody {
    public IMochaCandidateInvitationBody(string origin, string host, int testId) {
        callbackUrl = "https://" + host + "/interviewbot/imocha";
        // redirectUrl = origin + "/report?testId=" + testId.ToString();
        sendEmail = "no";
    }

    public IMochaCandidateInvitationBody(string origin, string host, int testId, string name, string email) : this(origin, host, testId) {
        this.email = email;
        this.name = name;
    }
    
    public string email { get; set; } = "";
    public string name { get; set; } = "";
    public string sendEmail { get; set; }
    public string callbackUrl { get; set;}
    public string redirectUrl { get; set; } = "";
    public string ProctoringMode { get; set; } = "image";
    public override string ToString()
    {
        return $"Name: {name}, Email: {email}, sendEmail: {sendEmail} callBackUrl: {callbackUrl}, redirectUrl: {redirectUrl}";
    }
}
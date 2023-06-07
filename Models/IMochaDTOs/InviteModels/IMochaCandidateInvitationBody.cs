namespace IPR_BE.Models;

/// <summary>
/// This class is the POST body to invite a new candidate to a test via iMocha api
/// </summary>
public class IMochaCandidateInvitationBody {
    public IMochaCandidateInvitationBody(IConfiguration config) {
        callbackUrl = config.GetValue<string>("IMocha:InviteCallBackURL")!;
        redirectURL = config.GetValue<string>("IMocha:InviteRedirectURL")!;
    }

    public IMochaCandidateInvitationBody(string email, string name, IConfiguration config) : this(config) {
        this.email = email;
        this.name = name;
    }
    
    public string email { get; set; } = "";
    public string name { get; set; } = "";
    public string sendEmail { get; set; } = "yes";
    public string callbackUrl { get; set;}
    public string redirectURL { get; set; }
}
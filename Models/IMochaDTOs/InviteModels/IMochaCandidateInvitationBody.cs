namespace IPR_BE.Models.DTO;

/// <summary>
/// This class is the POST body to invite a new candidate to a test via iMocha api
/// </summary>
public class IMochaCandidateInvitationBody {
    public string email { get; set; } = "";
    public string name { get; set; } = "";
    public string callbackUrl { get; set;} = "";
}
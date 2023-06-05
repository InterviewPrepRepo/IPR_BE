namespace IPR_BE.Models.Invite;

/// <summary>
/// This is a DTO we ask OUR clients to submit to us to invite candidates
/// </summary>
public class CandidateInvitation {
    public int testId { get; set; }
    public string email { get; set; } = "";
    public string name { get; set; } = "";
}

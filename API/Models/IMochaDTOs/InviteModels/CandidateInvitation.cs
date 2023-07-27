namespace IPR_BE.Models;

/// <summary>
/// This is a DTO we ask OUR clients to submit to us to invite candidates
/// By default, it asks iMocha to send email behalf of us
/// </summary>
public class CandidateInvitation {
    public int testId { get; set; }
    public string email { get; set; } = "";
    public string name { get; set; } = "";
    public string? currentRole { get; set; } = "";
    public int? yearsExperience { get; set; } = 0;
    public List<string>? skills { get; set; } = new();
}

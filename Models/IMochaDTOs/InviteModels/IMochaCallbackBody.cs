namespace IPR_BE.Models.Invite;

/// <summary>
/// This is the object IMocha gives us when a candidate either 1. starts the test, or complete/terminate/leaves the test
/// </summary>
public class IMochaCallbackBody {
    public string CandidateEmailId { get; set; } = "";
    public string AttemptedOn { get; set; } = "";
    public int TotalScore { get; set; }
    public int CandidateScore { get; set; } 
    public string? ReportPDFUrl { get; set; }
    public int TestInvitationId { get; set; }
    public string Status { get; set; } = "";
    public string AttemptedOnUtc { get; set; } = "";
    public string? PerformanceCategory { get; set; }
    public string BaseImgUrl { get; set; } = "";
}
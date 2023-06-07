namespace IPR_BE.Models;

/// <summary>
/// Used to save test data to the InterviewDB.
/// Used in the IMochaController's invite POST.
/// </summary>
public class TestAttempt
{
    public long testId { get; set; }
    public long attemptId { get; set; }
    public string status { get; set; } = "";
    public int candidateId { get; set; }
    public string invitationUrl { get; set; } = "";
    public DateTime modifiedOn { get; set; } = DateTime.UtcNow;
}
namespace IPR_BE.Models;

/// <summary>
/// Used to save test data to the InterviewDB.
/// Used in the IMochaController's invite POST.
/// Used to save test attempts to new db.
/// </summary>
public class TestAttempt
{
    //left as long to avoid having to store Guids elsewhere in other objects for now
    public long attemptId { get; set; }
    public int candidateId { get; set; }
    public long testId { get; set; }
    public string status { get; set; } = "";
    public DateTime createdOn {get; set;}
    public DateTime startDate {get; set;}
    public DateTime endDate {get; set;}
    public int totalScore {get; set;}
    public string comment {get; set;} = "";

    //These two are left for now, not part of new models
    public DateTime modifiedOn { get; set; } = DateTime.UtcNow;
    public string invitationUrl { get; set; } = "";
}
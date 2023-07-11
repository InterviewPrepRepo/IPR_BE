namespace IPR_BE.Models;

public class TestAttemptSection {
    public Guid testAttemptSectionId {get; set;}
    public long testAttemptId {get; set;}
    public long testSectionId {get; set;}
    public string comment {get; set;} = "";
}
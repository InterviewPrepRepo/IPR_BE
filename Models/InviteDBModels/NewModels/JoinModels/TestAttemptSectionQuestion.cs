namespace IPR_BE.Models;

public class TestAttemptQuestionSection {
    public int id {get; set;}
    public long testAttemptSectionId {get; set;}
    public long testQuestionId {get; set;}
    public string status {get; set;} = "";
    public int manualScore {get; set;}
    public int autoScore {get; set;}
    public int windowViolation {get; set;}
    public int timeViolation {get; set;}
    public string comment {get; set;} = "";
    
}
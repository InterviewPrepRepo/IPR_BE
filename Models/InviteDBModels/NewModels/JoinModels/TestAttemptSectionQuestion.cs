namespace IPR_BE.Models;

public class TestAttemptQuestionSection {
    public int id {get; set;}
    public TestAttemptSection testAttemptSection {get; set;} = new TestAttemptSection();
    public TestSectionQuestion testSectionQuestion {get; set;} = new TestSectionQuestion();
    public string status {get; set;} = "";
    public int manualScore {get; set;}
    public int autoScore {get; set;}
    public int windowViolation {get; set;}
    public int timeViolation {get; set;}
    public string comment {get; set;} = "";
    
}
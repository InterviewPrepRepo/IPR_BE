namespace IPR_BE.Models;

public class TestAttemptSection {
    public long testAttemptSectionId {get; set;}
    public TestAttempt testAttempt {get; set;} = new TestAttempt();
    public TestSection testSection {get; set;} = new TestSection();
    public string comment {get; set;} = "";
}
namespace IPR_BE.Models;

public class TestSection {
    
    public long testSectionId {get; set;}
    public Test test {get; set;} = new Test();
    public string name {get; set;} = "";
    public List<TestAttemptSection> testAttemptSections = new List<TestAttemptSection>();
    public List<TestSectionQuestion> testSectionQuestions = new List<TestSectionQuestion>();
}
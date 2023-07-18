namespace IPR_BE.Models;

public class Test {
    
    public long testId {get; set;}
    public string name {get; set;} = "";
    public string status {get; set;} = "";

    ICollection<TestSection> testSections { get; set; } = new List<TestSection>();
    ICollection<TestTag> testTags { get; set; } = new List<TestTag>();
    List<TestAttempt> testAttempts {get; set; } = new List<TestAttempt>();
}
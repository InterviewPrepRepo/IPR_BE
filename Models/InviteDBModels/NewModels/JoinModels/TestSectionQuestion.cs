namespace IPR_BE.Models;

public class TestSectionQuestion {
    
    public int id {get; set;}
    public Question question {get; set;} = new Question();
    public TestSection testSection {get; set;} = new TestSection();
    
}
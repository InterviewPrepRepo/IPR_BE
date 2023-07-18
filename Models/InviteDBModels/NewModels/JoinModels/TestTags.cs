namespace IPR_BE.Models;

public class TestTag {
    public long testTagId {get; set;}
    public Test test {get; set;} = new Test();
    public Tag tag {get; set;} = new Tag();
    
}
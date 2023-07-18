namespace IPR_BE.Models;

public class Tag {
    public long tagId {get; set;}
    public string name {get; set;} = "";

    public List<TestTag> testTags{get; set;} = new List<TestTag>();
   //public virtual ICollection<Test> Tests { get; set; } = new List<Test>();
}
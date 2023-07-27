namespace IPR_BE.Models;

public class Skill {
    public int id { get; set;}
    public string name { get; set;} = "";
    public virtual ICollection<Candidate> Candidate { get; set; } = new List<Candidate>();
}
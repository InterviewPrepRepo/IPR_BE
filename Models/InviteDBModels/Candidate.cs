namespace IPR_BE.Models;

/// <summary>
/// Used to track candidate data for the InviteDB. 
/// Used in the IMochaController invite POST
/// Used to track user and User roles, possible rename.
/// </summary>
public class Candidate
{
    public Candidate() {
        id = 0;
        name = "";
        email = "";
        currentRole = "";
    }
    public Candidate (string name, string email) : this() {
        this.name = name;
        this.email = email;
    }
    public int id { get; set; }
    public string name { get; set; }
    public string email { get; set; }
    public string? currentRole { get; set; } = "";
    public int? yearsExperience { get; set; } = 0;
    public virtual ICollection<Skill> Skill { get; set; } = new List<Skill>();
    public List<TestAttempt> testAttempts {get; set;} = new List<TestAttempt>();
}
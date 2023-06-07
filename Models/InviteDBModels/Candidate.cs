namespace IPR_BE.Models;

/// <summary>
/// Used to track candidate data for the InviteDB. 
/// Used in the IMochaController invite POST
/// </summary>
public class Candidate
{
    public Candidate() {
        id = 0;
        name = "";
        email = "";
    }
    public Candidate (string name, string email) : this() {
        this.name = name;
        this.email = email;
    }

    public int id { get; set; }
    public string name { get; set; }
    public string email { get; set; }
}
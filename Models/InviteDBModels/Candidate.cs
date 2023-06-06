namespace IPR_BE.Models;

/// <summary>
/// Used to track candidate data for the InviteDB. 
/// Used in the IMochaController invite POST
/// </summary>
public class Candidate
{
    public int id { get; set; }
    public string name { get; set; } = "";
    public string email { get; set; } = "";
}
namespace IPR_BE.Models;

public class Skill
{
    public double candidateQbScore { get; set; }
    public double qbScore { get; set; }
    public int noOfQue { get; set; }
    public int qbId { get; set; }
    public string? qbName { get; set; }
    public int sectionId { get; set; }
    public string? sectionName { get; set; }
}
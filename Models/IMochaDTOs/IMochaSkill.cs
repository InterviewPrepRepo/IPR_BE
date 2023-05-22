namespace IPR_BE.Models;

public class Skill
{
    public double candidateQbScore { get; set; }
    public double qbScore { get; set; }
    public int noOfQue { get; set; }
    public long qbId { get; set; }
    public string? qbName { get; set; }
    public long sectionId { get; set; }
    public string? sectionName { get; set; }
}
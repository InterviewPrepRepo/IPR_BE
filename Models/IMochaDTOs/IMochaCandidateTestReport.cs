namespace IPR_BE.Models.DTO;

public class CandidateTestReport
{
    public long testInvitationId { get; set; }
    public long testId { get; set; }
    public string candidateEmail { get; set; } = "";
    public string candidateName { get; set; } = "";
    public DateTime attemptedOn { get; set; }
    public string status { get; set; } = "";
    public double score { get; set; }
    public double candidatePoints { get; set; }
    public double totalTestPoints { get; set; }
    public double scorePercentage { get; set; }
    public int timeTaken { get; set; }
    public int testDuration { get; set; }
    public string performanceCategory { get; set; } = "";
    public string testName { get; set; } = "";
    public string pdfReportUrl { get; set; } = "";
    public List<QuesDifficultyAnalysis> quesDifficultyAnalysis { get; set; } = new();
    public List<Section> sections { get; set; } = new();
}
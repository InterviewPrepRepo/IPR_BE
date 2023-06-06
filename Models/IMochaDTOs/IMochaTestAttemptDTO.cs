namespace IPR_BE.Models;

/// <summary>
/// So far, never used. Wrapper for a return? 
/// </summary>
public class IMochaTestAttemptDTO{
    public CandidateTestReport? attempt { get; set; }
}

/// <summary>
/// This models the return from the IMocha API for a specific
/// test attempt by a candidate. 
/// Used in the IMochaService GetTestAttemptById method and
/// the IMochaController endpoint GET "reports/{testInvitationId}" that calls it. 
/// </summary>
public class CandidateTestReport
{
    public long testInvitationId { get; set; }
    public long testId { get; set; }
    public string candidateEmail { get; set; } = "";
    public string candidateName { get; set; } = "";
    public DateTime attemptedOn { get; set; }
    public string status { get; set; } = "";
    public decimal score { get; set; }
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

/// <summary>
/// Only used in the CandidateTestReport class, never
/// instantiated.
/// </summary>
public class Section
{
    public double candidateScore { get; set; }
    public double sectionScore { get; set; }
    public int noOfQue { get; set; }
    public long sectionID { get; set; }
    public string sectionName { get; set; } = "";
    public int sectionTime { get; set; }
    public int sectionTimeTaken { get; set; }
    public double negativeMark { get; set; }
    public int correctQuestions { get; set; }
    public int wrongQuestions { get; set; }
    public int skippedQuestions { get; set; }
    public int notAnsweredQuestions { get; set; }
}
/// <summary>
/// Only used as part of the CandidateTestReport class, 
/// never instantiated.
/// </summary>
public class QuesDifficultyAnalysis
{
    public string difficultyLevel { get; set; } = "";
    public double noOfQuestions { get; set; }
    public double noOfCorrectQuestions { get; set; }
    public double percentage { get; set; }
}
namespace IPR_BE.Models;

/// <summary>
/// This is used to get return from the Official IMocha API's 
/// "candidates/testattempts?state=completed" endpoint.
/// Used in the "reports/{testInvitationId}/questions") GET endpoint. 
/// </summary>
public class TestResultDTO
{
    public List<Result>? result { get; set; }
    public object? errors { get; set; }
}

/// <summary>
/// Used as part of the TestResultDTO class. Only used when unpacking return 
/// in the "reports/{testInvitationId}/questions" GET endpoint in a foreach loop.
/// </summary>
public class Result
{
    public long testInvitationId { get; set; }
    public long testId { get; set; }
    public string? testName { get; set; }
    public int questionId { get; set; }
    public int sectionId { get; set; }
    public string? sectionName { get; set; }
    public int questionBankId { get; set; }
    public string? questionBank { get; set; }
    public string? question { get; set; }
    public double score { get; set; }
    public decimal manualScore { get; set; } = -1;
    public int manualScoreId {get; set;} = 0;
    public decimal? average {get; set;}
    public int timeTaken { get; set; }
    public int windowViolation { get; set; }
    public int timeViolation { get; set; }
    public int questionTypeId { get; set; }
    public int points { get; set; }
    public int orderFlag { get; set; }
    public int difficultyLevel { get; set; }
    public string? questionStatus { get; set; }
    public object? questionFeedback { get; set; }
    public CandidateAnswer? candidateAnswer { get; set; }
    public string? correctAnswer {get; set;}
    public string? givenAnswer {get;set;}
}

/// <summary>
/// Only used as part of the CandidateAnswer class below. Not 
/// instantiated anywhere else. 
/// </summary>
public class VideoAnswer
{
    public string? videoUrl { get; set; }
}

/// <summary>
/// Used as part of the Result class. Not instantiated
/// anywhere. 
/// </summary>
public class CandidateAnswer
{
    public int questionTypeId { get; set; }
    public VideoAnswer? videoAnswer { get; set; }
    public object? singleAnswer { get; set; }
    public object? multipleAnswer { get; set; }
    public object? logicBox { get; set; }
    public object? fillInBlanks { get; set; }
    public object? codingQuestionData { get; set; }
    public object? uploadQuestionData { get; set; }
    public object? mssqlAnswer { get; set; }
    public object? htmlCssJsAnswer { get; set; }
    public object? mySqlAnswer { get; set; }
}
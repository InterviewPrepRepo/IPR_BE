namespace IPR_BE.Models;


/// <summary>
/// This is used to get the test with scores from InterviewBot. 
/// Used in IMochaService GetTestAttemptById.
/// Used in InterviewBotRepo GetTestByID to retrieve test from interviewBot DB.
/// Used in IMochaController "reports/{testInvitationId}/questions" endpoint to get scores in the return to FE.
/// </summary>
public class TestDetail
{
    public long testAttemptId { get; set; }
    public decimal scoreSum {get; set;}
    public List<Question> questions { get; set; } = new();
}

/// <summary>
/// Used primarily to build complete TestDetail object. 
/// InterviewBotRepo instantiates Question objects when pulling question
/// scores from the InterviewBot database in InterviewBotRepo.cs
/// </summary>
public class Question
{    
    public Question(int q_id, decimal sc, string g_a, string c_a)
    {
        questionId = q_id;
        score = sc;
        givenAnswer = g_a;
        correctAnswer = c_a;
    }
    public int? questionId { get; set; }
    public decimal? score { get; set; }
    public string? correctAnswer {get; set;}
    public string? givenAnswer {get;set;}
}
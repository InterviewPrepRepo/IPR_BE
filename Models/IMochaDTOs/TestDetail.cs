namespace IPR_BE.Models;


/// <summary>
/// This is used to get the test with scores from InterviewBot. 
/// Used in IMochaService GetTestAttemptById.
/// Used in InterviewBotRepo GetTestByID to retrieve test from interviewBot DB.
/// Used in IMochaController "reports/{testInvitationId}/questions" endpoint to get scores in the return to FE.
/// </summary>
public class TestDetail
{
    public int testAttemptId { get; set; }
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
    public Question(int q_id, decimal sc)
    {
        questionId = q_id;
        score = sc;
    }
    public int? questionId { get; set; }
    public decimal? score { get; set; }
}
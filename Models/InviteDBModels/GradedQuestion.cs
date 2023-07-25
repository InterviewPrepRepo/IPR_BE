namespace IPR_BE.Models;


/// <summary>
/// This class is used as a quick way to store manually revised/scored question grades
/// and then render them to the front end. We will only store questions that have been manually
/// graded. 
/// </summary>
public class GradedQuestion {
    public int gradedQuestionId {get; set;}
    public long questionId {get; set;}
    public long testAttempt {get; set;}
    public decimal grade { get; set; } 
    
}
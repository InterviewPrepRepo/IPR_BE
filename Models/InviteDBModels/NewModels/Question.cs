namespace IPR_BE.Models;

public class Question {
    public long questionId {get; set;}
    public string prompt {get; set;} = "";
    public QuestionType questionType {get; set;} = new QuestionType();
    public List<TestSectionQuestion> testSectionQuestions{get; set;} = new List<TestSectionQuestion>();
    public int possiblePoints {get; set;}

}
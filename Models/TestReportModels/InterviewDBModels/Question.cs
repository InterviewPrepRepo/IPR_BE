namespace IPR_BE.Models.TestReport;

public class Question
{

    public Question(int q_id, decimal sc){
        questionId = q_id;
        score = sc;
    }
    public int? questionId { get; set; }
    public decimal? score { get; set; }
}
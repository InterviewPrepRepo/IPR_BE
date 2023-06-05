
namespace IPR_BE.Models.TestReport;

public class TestDetail
{
    public int testAttemptId { get; set; }

    public decimal scoreSum {get; set;}
    public List<Question> questions { get; set; } = new();

}

public class Question
    {    public Question(int q_id, decimal sc){
            questionId = q_id;
            score = sc;
        }
        public int? questionId { get; set; }
        public decimal? score { get; set; }
    }

namespace IPR_BE.Models;

public class TestDetail
{
    public int testAttemptId { get; set; }

    public decimal averageScore {get; set;}
    public List<Question> questions { get; set; } = new();
}
namespace IPR_BE.Models.TestReport;

public class TestAttempt {
    public long testId { get; set; }
    public long attemptId { get; set; }
    public string status { get; set; } = "";
    public int candidateId { get; set; }
}
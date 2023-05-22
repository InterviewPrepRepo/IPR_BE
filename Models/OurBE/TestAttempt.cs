namespace IPR_BE.Models.Internal;

public struct TestAttempt {
    public long testId { get; set; }
    public long attemptId { get; set; }
    public string status { get; set; }
    public int candidateId { get; set; }
}
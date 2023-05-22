namespace IPR_BE.Models;

public class IMochaTest {
    public long testId { get; set; }
    public string testName { get; set; } = "";
    public string status { get; set; } = "";
    public int duration { get; set; }
    public List<string> labels { get; set; } = new();
}
namespace IPR_BE.Models;

/// <summary>
/// Used in IMochaService.cs GetAllTests which hits the IMocha API's
/// "tests?pageNo={pageNo}&pageSize={pageSize}&labelsFilter={labelsFilter}"
/// endpoint to get all tests that are labeled with a specific label. 
/// </summary>
public class IMochaTestDTO {
    public List<IMochaTest> tests{ get; set; } = new();
    public int count { get; set; }
}

/// <summary>
/// Used in the ProcessNewTestAttempts method in the InterviewBotService
/// </summary>
public class IMochaTest {
    public long testId { get; set; }
    public string testName { get; set; } = "";
    public string status { get; set; } = "";
    public int duration { get; set; }
    public List<string> labels { get; set; } = new();
}
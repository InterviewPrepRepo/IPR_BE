namespace IPR_BE.Models;
public struct TestAttemptsListResponseBody {

/// <summary>
/// Used in the IMochaController "tests/attempts" POST endpoint
/// </summary>
/// <value></value>
    public TestAttemptList result { get; set; }

/// <summary>
/// Never instantiated. 
/// </summary>
    public struct TestAttemptList {
        public List<TestAttemptShortened> testAttempts { get; set; }
    }

/// <summary>
/// Used in ProcessNewTestAttempts method in InterviewBotService
/// </summary>
    public struct TestAttemptShortened {
        public long testInvitationId { get; set; }
        public long testId {get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string invitationtype { get; set; }
        public string teststatus {get; set; }
    }
}
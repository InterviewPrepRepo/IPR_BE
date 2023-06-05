namespace IPR_BE.Models.TestReqResp;
public struct TestAttemptsListResponseBody {

    public TestAttemptList result { get; set; }

    public struct TestAttemptList {
        public List<TestAttemptShortened> testAttempts { get; set; }
    }

    public struct TestAttemptShortened {
        public long testInvitationId { get; set; }
        public long testId {get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string invitationtype { get; set; }
        public string teststatus {get; set; }
    }
}
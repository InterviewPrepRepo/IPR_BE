namespace IPR_BE.Models;

public struct TestAttemptRequestBody {
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }

    //optional parameter that will return only the test attempts associated with the testId
    public long testId { get; set; }
}
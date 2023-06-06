namespace IPR_BE.Models;

/// <summary>
/// Used to create the request body to send to the IMochaAPI to get all
/// test attempts within a certain time frame. 
/// </summary>
public struct TestAttemptRequestBody {
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }

    //optional parameter that will return only the test attempts associated with the testId
    public long testId { get; set; }
}
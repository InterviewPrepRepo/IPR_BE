namespace IPR_BE.Models;

/// <summary>
/// Used in the IMochaController in the "tests/{testId}" GET endpoint, 
/// which hits the IMocha "tests/{testId}" GET endpoint for a single
/// specific test. 
/// Note: This test isn't a student's attempt of the 
/// test, this is the data for the test that is then given to 
/// students to take. 
/// </summary>
public class IMochaTestDetailDTO {
    public long testId { get; set; }
    public string testName { get; set; } = "";
    public int duration { get; set; }
    public int questions { get; set; }
    public List<IMochaTestSectionDTO> sections { get; set; } = new();
}

/// <summary>
/// Only used as a List field in the IMochaTestDetailDTO, never 
/// instantiated. 
/// </summary>
public class IMochaTestSectionDTO {
    public string sectionName { get; set; } = "";
    public string sectionType { get; set; } = "";
    public int questions { get; set; }
    public int duration { get; set; }
}
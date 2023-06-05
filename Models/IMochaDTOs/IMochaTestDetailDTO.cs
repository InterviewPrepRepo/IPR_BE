namespace IPR_BE.Models;

public class IMochaTestDetailDTO {
    public long testId { get; set; }
    public string testName { get; set; } = "";
    public int duration { get; set; }
    public int questions { get; set; }
    public List<IMochaTestSectionDTO> sections { get; set; } = new();
}

public class IMochaTestSectionDTO {
    public string sectionName { get; set; } = "";
    public string sectionType { get; set; } = "";
    public int questions { get; set; }
    public int duration { get; set; }
}
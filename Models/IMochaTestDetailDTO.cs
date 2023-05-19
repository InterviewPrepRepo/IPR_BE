namespace IPR_BE.Models;

public class IMochaTestDetailDTO {
    public int testId { get; set; }
    public string testName { get; set; } = "";
    public int duration { get; set; }
    public int questions { get; set; }
    public List<IMochaTestSectionDTO> sections { get; set; } = new();
}
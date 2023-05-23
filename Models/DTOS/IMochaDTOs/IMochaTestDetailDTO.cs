namespace IPR_BE.Models.DTO;

public class IMochaTestDetailDTO {
    public long testId { get; set; }
    public string testName { get; set; } = "";
    public int duration { get; set; }
    public int questions { get; set; }
    public List<IMochaTestSectionDTO> sections { get; set; } = new();
}
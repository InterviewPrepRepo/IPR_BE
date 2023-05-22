namespace IPR_BE.Models;

public class IMochaTestSectionDTO {
    public string sectionName { get; set; } = "";
    public string sectionType { get; set; } = "";
    public int questions { get; set; }
    public int duration { get; set; }
}
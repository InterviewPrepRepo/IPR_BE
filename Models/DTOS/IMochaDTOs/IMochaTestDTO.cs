namespace IPR_BE.Models.DTO;

public class IMochaTestDTO {
    public List<IMochaTest> tests{ get; set; } = new();
    public int count { get; set; }
}
namespace IPR_BE.Models;

public class IMochaTestDTO {
    public List<IMochaTest> tests{ get; set; } = new();
    public int count { get; set; }
}
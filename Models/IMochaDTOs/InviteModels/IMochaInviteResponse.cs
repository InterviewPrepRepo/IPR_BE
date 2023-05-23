namespace IPR_BE.Models.DTO;

/// <summary>
/// This is invitation response from imocha API when we invite the candidate to the test
/// </summary>
public class IMochaTestInviteResponse {
    public long testInvitationId { get; set; }
    public string testUrl { get; set; } = "";
}
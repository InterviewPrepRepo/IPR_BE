namespace IPR_BE.Models;

/// <summary>
/// This is invitation response from imocha API when we invite the candidate to the test
/// </summary>
public class IMochaTestInviteResponse {
    public long testInvitationId { get; set; }
    public string testUrl { get; set; } = "";

    public override string ToString()
    {
        return $"InvitationId: {testInvitationId} TestUrl: {testUrl}";
    }
}

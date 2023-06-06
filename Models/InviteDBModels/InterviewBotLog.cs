namespace IPR_BE.Models;

/// <summary>
/// Used for logging things related to the InterviewBot. Used in the InterviewBotController
/// and the InterviewBotService.
/// </summary>
public class InterviewBotLog
{
    public int logId { get; set; }
    public DateTime created { get; set; } = DateTime.UtcNow;
    public string message { get; set; } = "";
}
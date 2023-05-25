namespace IPR_BE.Models.DTO;

public class Result
    {
        public long testInvitationId { get; set; }
        public long testId { get; set; }
        public string? testName { get; set; }
        public int questionId { get; set; }
        public int sectionId { get; set; }
        public string? sectionName { get; set; }
        public int questionBankId { get; set; }
        public string? questionBank { get; set; }
        public string? question { get; set; }
        public double score { get; set; }
        public decimal? average {get; set;}
        public int timeTaken { get; set; }
        public int windowViolation { get; set; }
        public int timeViolation { get; set; }
        public int questionTypeId { get; set; }
        public int points { get; set; }
        public int orderFlag { get; set; }
        public int difficultyLevel { get; set; }
        public string? questionStatus { get; set; }
        public object? questionFeedback { get; set; }
        public CandidateAnswer? candidateAnswer { get; set; }
    }
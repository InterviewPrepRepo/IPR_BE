namespace IPR_BE.Models.DTO;

public class QuesDifficultyAnalysis
    {
        public string difficultyLevel { get; set; } = "";
        public double noOfQuestions { get; set; }
        public double noOfCorrectQuestions { get; set; }
        public double percentage { get; set; }
    }
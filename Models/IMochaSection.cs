namespace IPR_BE.Models;

public class Section
{
        public double candidateScore { get; set; }
        public double sectionScore { get; set; }
        public int noOfQue { get; set; }
        public int sectionID { get; set; }
        public string sectionName { get; set; } = "";
        public int sectionTime { get; set; }
        public int sectionTimeTaken { get; set; }
        public double negativeMark { get; set; }
        public int correctQuestions { get; set; }
        public int wrongQuestions { get; set; }
        public int skippedQuestions { get; set; }
        public int notAnsweredQuestions { get; set; }
}
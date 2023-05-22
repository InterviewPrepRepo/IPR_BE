    namespace IPR_BE.Models;
    
    public class CandidateAnswer
    {
        public int questionTypeId { get; set; }
        public VideoAnswer? videoAnswer { get; set; }
        public object? singleAnswer { get; set; }
        public object? multipleAnswer { get; set; }
        public object? logicBox { get; set; }
        public object? fillInBlanks { get; set; }
        public object? codingQuestionData { get; set; }
        public object? uploadQuestionData { get; set; }
        public object? mssqlAnswer { get; set; }
        public object? htmlCssJsAnswer { get; set; }
        public object? mySqlAnswer { get; set; }
    }
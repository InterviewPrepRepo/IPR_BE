using MySql.Data.MySqlClient;
using IPR_BE.Models;

namespace IPR_BE.DataAccess;

public class InterviewBotRepo {
    private readonly string connectionString;
    public InterviewBotRepo(string connStr) {
        connectionString = connStr;
    }


/// <summary>
///     Gets test information from the Interview Prep database, 
///     including questions and their scores. 
/// </summary>
/// <param name="testId">TestInvitationId</param>
/// <returns> returns test details to front end including average</returns>
    public TestDetail GetTestByID(int testId){
        List<Decimal> scores = new();
        TestDetail test = new();
        using MySqlConnection conn = new(connectionString);
        conn.Open();
        using MySqlCommand command = new("SELECT IM_TEST_INVITATION_ID, QUESTION_ID, SCORE FROM ans_files WHERE IM_TEST_INVITATION_ID = @testId", conn);
        command.Parameters.AddWithValue("@testId", testId);

        test.testAttemptId = testId;

        using MySqlDataReader reader = command.ExecuteReader();

        while(reader.Read()){
            int question_Id = reader.GetInt32(1);
            decimal score = Math.Round(reader.GetDecimal(2), 2);
            scores.Add(Math.Round(score, 2));
            Question q = new(question_Id, score);
            test.questions.Add(q);
        }

        test.averageScore = Math.Round(scores.Average(), 2);

        conn.Close();
        return test;
    }

}
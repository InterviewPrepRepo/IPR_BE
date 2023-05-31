using MySql.Data.MySqlClient;
using IPR_BE.Models.TestReport;

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
        using MySqlCommand command = new("SELECT distinct IM_TEST_INVITATION_ID, QUESTION_ID, SCORE FROM ans_files WHERE IM_TEST_INVITATION_ID = @testId", conn);
        command.Parameters.AddWithValue("@testId", testId);

        test.testAttemptId = testId;

        using MySqlDataReader reader = command.ExecuteReader();

        while(reader.Read()){
            if(reader["SCORE"] != System.DBNull.Value){
                int question_Id = reader.GetInt32(1);
                decimal score = Math.Round(reader.GetDecimal(2), 2);
                scores.Add(Math.Round(score, 2));
                Question q = new(question_Id, score);
                test.questions.Add(q);
            }
        }

        if(scores.Count > 0) {
            test.scoreSum = Math.Round(scores.Sum(), 2);
        }
        else {
            test.scoreSum = -1;
        }
        conn.Close();
        return test;
    }

    public HashSet<long> GetAllUniqueTestAttemptIds() {
        HashSet<long> uniqueAttemptIds = new();
        using MySqlConnection conn = new(connectionString);
        using MySqlCommand command = new MySqlCommand("select distinct IM_TEST_INVITATION_ID from ans_files;", conn);

        conn.Open();
        using MySqlDataReader reader = command.ExecuteReader();
        while(reader.Read()) {
            if(reader["IM_TEST_INVITATION_ID"] != System.DBNull.Value) {
                uniqueAttemptIds.Add((long) reader["IM_TEST_INVITATION_ID"]);
            }
        }
        return uniqueAttemptIds;
    }
}
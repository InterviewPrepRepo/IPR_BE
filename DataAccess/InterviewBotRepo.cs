using MySql.Data.MySqlClient;

namespace IPR_BE.DataAccess;

public class InterviewBotRepo {
    private readonly string connectionString;
    public InterviewBotRepo(string connStr) {
        connectionString = connStr;
    }

    public void GetTheMostRecent(int num) {
        using MySqlConnection conn = new(connectionString);
        using MySqlCommand command = new("Select * from ans_files order by modified_on desc limit @limit", conn);
        command.Parameters.AddWithValue("@limit", num);

        conn.Open();
        using MySqlDataReader reader = command.ExecuteReader();

        while(reader.Read()) {
            Console.WriteLine(reader["ID"]);
        }
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
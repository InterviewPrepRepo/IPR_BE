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
}
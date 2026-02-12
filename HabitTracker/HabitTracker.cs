using Microsoft.Data.Sqlite;

namespace HabitTracker;

class HabitTracker
{
    static void Main()
    {
        using var connection = new SqliteConnection("Data Source=habit.db");
        connection.Open();

        CreateDatabase(connection);
        ReadAllEntries(connection);

        connection.Close();
    }

    static void CreateDatabase(SqliteConnection connection)
    {
        var command = connection.CreateCommand();

        command.CommandText =
            @"
                CREATE TABLE tracker (
                    id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    date DATE NOT NULL,
                    count INTEGER NOT NULL
                )
            ";

        if (!File.Exists("habit.db"))
        {
            command.ExecuteNonQuery();
        }
    }

    static async void ReadAllEntries(SqliteConnection connection)
    {
        using var outputStream = Console.OpenStandardOutput();

        var selectCommand = connection.CreateCommand();
        selectCommand.CommandText = @"SELECT * FROM tracker";

        using var reader = selectCommand.ExecuteReader();

        Console.WriteLine("Reading the database...");

        while (reader.Read())
        {
            var date = reader.GetDateTime(1);
            var count = reader.GetInt16(2);

            Console.WriteLine($"{date:MMM}. {date.Day}, {date.Year}: {count} hours");
        }
    }
}

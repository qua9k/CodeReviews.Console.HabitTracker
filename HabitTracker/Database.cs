using Microsoft.Data.Sqlite;

namespace HabitTracker;

public class Database
{
    public static void CreateDatabase(SqliteConnection connection)
    {
        Console.WriteLine("Creating database...\nDatabase created.");

        var command = connection.CreateCommand();

        command.CommandText =
            @"
                CREATE TABLE tracker (
                    id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    date DATE NOT NULL,
                    habit TEXT NOT NULL,
                    count INTEGER NOT NULL
                )
            ";

        command.ExecuteNonQuery();

        UserInterface.Pause();
    }

    public static void SeedDatabase(SqliteConnection connection)
    {
        var seedCommand = connection.CreateCommand();

        seedCommand.CommandText =
            @"
              INSERT INTO tracker
              VALUES 
              (1, '1901-01-01', 'Rock Climbing', 1),
              (2, '1902-02-02', 'Guitar', 2),
              (3, '1903-03-03', 'Painting', 3)
            ";

        seedCommand.ExecuteNonQuery();
    }

    public static bool CloseConnection(SqliteConnection connection)
    {
        Console.WriteLine("Goodbye.");
        connection.Close();
        return false;
    }
}

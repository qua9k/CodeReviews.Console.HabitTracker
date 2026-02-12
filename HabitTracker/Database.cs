using Microsoft.Data.Sqlite;

namespace HabitTracker;

public class Database
{
    public static bool CloseConnection(SqliteConnection connection)
    {
        Console.WriteLine("Goodbye.");
        connection.Close();
        return false;
    }

    public static void CreateDatabase(SqliteConnection connection)
    {
        Console.WriteLine("Creating database...");
        Console.WriteLine("Database created.");

        var command = connection.CreateCommand();

        command.CommandText =
            @"
                CREATE TABLE tracker (
                    id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    date DATE NOT NULL,
                    count INTEGER NOT NULL
                )
            ";

        command.ExecuteNonQuery();
    }

    public static void SeedDatabase(SqliteConnection connection)
    {
        var seedCommand = connection.CreateCommand();

        seedCommand.CommandText =
            @"
              INSERT INTO tracker
              VALUES 
              (1, '1901-01-01', 1),
              (2, '1902-02-02', 2),
              (3, '1903-03-03', 3)
            ";

        seedCommand.ExecuteNonQuery();
    }
}

using Microsoft.Data.Sqlite;

namespace HabitTracker;

class HabitTracker
{
    static void Main()
    {
        string databaseFile = "habit.db";
        using var connection = new SqliteConnection();

        if (!File.Exists(databaseFile))
        {
            connection.ConnectionString = $"Data Source={databaseFile}";
            connection.Open();
            CreateDatabase(connection);
            SeedDatabase(connection);
            Console.WriteLine("\nPress any key to return to the main menu.");
            Console.ReadKey();
        }
        else
        {
            connection.ConnectionString = $"Data Source={databaseFile}";
            connection.Open();
        }

        bool connected = true;

        while (connected)
        {
            PrintMenuOptions();

            string? input = Console.ReadLine();

            switch (input)
            {
                case "c":
                    CreateEntry(connection);
                    break;
                case "r":
                    ReadEntry(connection);
                    break;
                case "u":
                    UpdateEntry(connection);
                    break;
                case "d":
                    DeleteEntry(connection);
                    break;
                case "x":
                    connected = CloseConnection(connection);
                    break;
                default:
                    PrintInputUnknown();
                    break;
            }

            Console.WriteLine();
        }
    }

    private static void PrintMenuOptions()
    {
        Console.Clear();
        Console.Write(
            """
            Please choose an option and press 'Enter':

            'c': Create entry
            'r': Read entry
            'u': Update entry
            'd': Delete entry
            'x': Exit

            Your choice: 
            """
        );
    }

    private static void PrintInputUnknown()
    {
        Console.Clear();
        Console.WriteLine("Your input was not understood.");
        Console.WriteLine("\nPress any key to return to the main menu.");
        Console.ReadKey();
    }

    private static bool CloseConnection(SqliteConnection connection)
    {
        Console.WriteLine("Goodbye.");
        connection.Close();
        return false;
    }

    private static void CreateDatabase(SqliteConnection connection)
    {
        Console.WriteLine($"Creating database...");

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

    private static void SeedDatabase(SqliteConnection connection)
    {
        var seedCommand = connection.CreateCommand();

        seedCommand.CommandText =
            @"
              INSERT INTO tracker
              VALUES 
              (1, '2026-01-01', 1),
              (2, '2026-01-02', 2),
              (3, '2026-01-03', 3)
            ";

        seedCommand.ExecuteNonQuery();
    }

    private static void CreateEntry(SqliteConnection connection)
    {
        Console.Clear();

        Console.Write("Enter the date (YYYY-mm-dd): ");
        string? date = Console.ReadLine();

        Console.Write("Enter the habit count: ");
        string? count = Console.ReadLine();

        var seedCommand = connection.CreateCommand();

        seedCommand.CommandText =
            $@"
              INSERT INTO tracker(date, count)
              VALUES 
              ('{date}', {count})
            ";

        seedCommand.ExecuteNonQuery();
        Console.WriteLine("\nPress any key to return to the main menu.");
        Console.ReadKey();
    }

    private static void UpdateEntry(SqliteConnection connection)
    {
        Console.Clear();
        Console.Write("Enter the id of the entry to update: ");
        Console.WriteLine("Enter the updated habit count: ");
        Console.WriteLine("\nPress any key to return to the main menu.");
        Console.ReadKey();
    }

    private static void DeleteEntry(SqliteConnection connection)
    {
        Console.Clear();
        Console.Write("Enter the id of the entry to delete (or '*' for all entries): ");

        string? pKey = Console.ReadLine();
        var deleteCommand = connection.CreateCommand();

        deleteCommand.CommandText = $"DELETE FROM tracker";

        if (pKey != "*")
        {
            deleteCommand.CommandText = $" WHERE id = {pKey}";
        }

        deleteCommand.ExecuteNonQuery();

        Console.WriteLine("\nPress any key to return to the main menu.");
        Console.ReadKey();
    }

    private static void ReadEntry(SqliteConnection connection)
    {
        Console.Clear();
        Console.Write("Enter the id of the entry to read (or '*' for all entries): ");

        string? pKey = Console.ReadLine();
        var selectCommand = connection.CreateCommand();

        selectCommand.CommandText = "SELECT * FROM tracker";

        if (pKey != "*")
        {
            selectCommand.CommandText += $" WHERE id = {pKey}";
        }

        Console.Clear();
        Console.WriteLine("Your query results:\n");

        using var reader = selectCommand.ExecuteReader();

        while (reader.Read())
        {
            var id = reader.GetInt16(0);
            var date = reader.GetDateTime(1);
            var count = reader.GetInt16(2);

            Console.WriteLine($"{id}.) {date:MMM}. {date.Day}, {date.Year}: {count}");
        }

        Console.WriteLine("\nPress any key to return to the main menu.");
        Console.ReadKey();
    }
}

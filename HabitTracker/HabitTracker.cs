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
            Pause();
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
              (1, '1901-01-01', 1),
              (2, '1902-02-02', 2),
              (3, '1903-03-03', 3)
            ";

        seedCommand.ExecuteNonQuery();
    }

    private static void CreateEntry(SqliteConnection connection)
    {
        Console.Clear();

        Console.Write("Enter the date (YYYY-mm-dd): ");
        string? date = Console.ReadLine();

        while (!DateTime.TryParse(date, out DateTime _))
        {
            Console.Clear();
            Console.WriteLine("The date must be in YYYY-mm-dd format.");
            Console.Write("Please re-enter the date: ");
            date = Console.ReadLine();
        }

        Console.Write("Enter the habit count: ");
        string? count = Console.ReadLine();

        while (!uint.TryParse(count, out uint _))
        {
            Console.Clear();
            Console.WriteLine("Habit count must be a number greater than 0.");
            Console.Write("Please re-enter the habit count: ");
            count = Console.ReadLine();
        }

        var seedCommand = connection.CreateCommand();

        seedCommand.CommandText =
            $@"
              INSERT INTO tracker(date, count)
              VALUES 
              ('{date}', {count})
            ";

        seedCommand.ExecuteNonQuery();

        Pause();
    }

    // [[todo]] :: implement
    private static void UpdateEntry(SqliteConnection connection)
    {
        string? primaryKey = GetId(CrudOps.Update);
        var updateCommand = connection.CreateCommand();
        Pause();
    }

    private static void DeleteEntry(SqliteConnection connection)
    {
        string? primaryKey = GetId(CrudOps.Delete);
        var deleteCommand = connection.CreateCommand();

        deleteCommand.CommandText = $"DELETE FROM tracker";

        if (primaryKey != "*")
        {
            deleteCommand.CommandText += $" WHERE id = {primaryKey}";
        }

        deleteCommand.ExecuteNonQuery();

        Pause();
    }

    private static string GetId(string crudOp)
    {
        string message = $"Enter the id of the entry to {crudOp}";

        Console.Clear();

        if (crudOp == CrudOps.Update)
        {
            Console.Write($"{message}: ");
        }
        else
        {
            // [[note]] :: only Delete and Read can target all entries
            Console.Write($"{message} ('*' for all entries): ");
        }

        string? id = Console.ReadLine();

        if (id == "*" && (crudOp == CrudOps.Delete || crudOp == CrudOps.Read))
        {
            return id;
        }

        while (!int.TryParse(id, out int _))
        {
            Console.Clear();
            Console.WriteLine("The id must be an integer greater than 0.");
            Console.Write("Please re-enter the id: ");
            id = Console.ReadLine();
        }

        return id;
    }

    private static void ReadEntry(SqliteConnection connection)
    {
        string? primaryKey = GetId(CrudOps.Read);
        var selectCommand = connection.CreateCommand();

        selectCommand.CommandText = "SELECT * FROM tracker";

        if (primaryKey != "*")
        {
            selectCommand.CommandText += $" WHERE id = {primaryKey}";
        }

        using var reader = selectCommand.ExecuteReader();

        if (!reader.HasRows)
        {
            Console.Clear();
            Console.WriteLine($"Your query returned no results.");
        }
        else
        {
            Console.Clear();
            Console.WriteLine("Your query results:\n");

            while (reader.Read())
            {
                var id = reader.GetInt16(0);
                var date = reader.GetDateTime(1);
                var count = reader.GetInt16(2);

                Console.WriteLine($"{id}.) {date:MMM}. {date.Day}, {date.Year}: {count}");
            }
        }

        Pause();
    }

    private static void Pause()
    {
        Console.WriteLine("\nPress any key to return to the main menu.");
        Console.ReadKey();
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
        Pause();
    }
}

static class CrudOps
{
    internal static readonly string Create = "create";
    internal static readonly string Read = "read";
    internal static readonly string Update = "update";
    internal static readonly string Delete = "delete";
}

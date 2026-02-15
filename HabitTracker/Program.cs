using Microsoft.Data.Sqlite;

namespace HabitTracker;

class Program
{
    static void Main()
    {
        var databaseFile = "habit.db";
        using var connection = new SqliteConnection();

        if (!File.Exists(databaseFile))
        {
            connection.ConnectionString = $"Data Source={databaseFile}";
            connection.Open();
            Database.CreateDatabase(connection);
            Database.SeedDatabase(connection);
        }
        else
        {
            connection.ConnectionString = $"Data Source={databaseFile}";
            connection.Open();
        }

        bool connected = true;

        while (connected)
        {
            UserInterface.PrintMenuOptions();

            var input = Console.ReadLine();

            switch (input)
            {
                case "c":
                    Crud.CreateEntry(connection);
                    break;
                case "r":
                    Crud.ReadEntry(connection);
                    break;
                case "u":
                    Crud.UpdateEntry(connection);
                    break;
                case "d":
                    Crud.DeleteEntry(connection);
                    break;
                case "x":
                    connected = Database.CloseConnection(connection);
                    break;
                default:
                    UserInterface.PrintInputUnknown();
                    break;
            }

            Console.WriteLine();
        }
    }
}

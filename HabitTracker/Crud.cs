using HabitTracker;
using Microsoft.Data.Sqlite;

static class CrudOps
{
    internal static readonly string Create = "create";
    internal static readonly string Read = "read";
    internal static readonly string Update = "update";
    internal static readonly string Delete = "delete";
}

class Crud : ICrudController
{
    public static void CreateEntry(SqliteConnection connection)
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

        UserInterface.Pause();
    }

    // [[todo]] :: implement
    public static void UpdateEntry(SqliteConnection connection)
    {
        string? primaryKey = GetId(CrudOps.Update);
        var updateCommand = connection.CreateCommand();

        DateTime newDate = DateTime.Now;
        int newCount = 99;

        Console.WriteLine($"the date: {newDate}");

        updateCommand.CommandText =
            @$"
                UPDATE tracker
                SET date = '{newDate}',
                    count = {newCount}
                WHERE
                    id = {primaryKey}
            ";

        UserInterface.Pause();
    }

    public static void DeleteEntry(SqliteConnection connection)
    {
        string? primaryKey = GetId(CrudOps.Delete);
        var deleteCommand = connection.CreateCommand();

        deleteCommand.CommandText = $"DELETE FROM tracker";

        if (primaryKey != "*")
        {
            deleteCommand.CommandText += $" WHERE id = {primaryKey}";
        }

        deleteCommand.ExecuteNonQuery();

        UserInterface.Pause();
    }

    public static string GetId(string crudOp)
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

    public static void ReadEntry(SqliteConnection connection)
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

        UserInterface.Pause();
    }
}

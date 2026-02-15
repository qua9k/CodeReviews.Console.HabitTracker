namespace HabitTracker;

class Validator
{
    public static string ValidateField(string fieldType, string? value)
    {
        if (fieldType == TableFields.Date)
        {
            while (!DateTime.TryParse(value, out DateTime _))
            {
                PrintValidatorMessage(fieldType);
                value = Console.ReadLine();
            }
        }
        else if (fieldType == TableFields.Count || fieldType == TableFields.Id)
        {
            while (!uint.TryParse(value, out uint _))
            {
                PrintValidatorMessage(fieldType);
                value = Console.ReadLine();
            }
        }

        return value;
    }

    public static void PrintValidatorMessage(string field)
    {
        Console.Clear();

        switch (field)
        {
            case TableFields.Id:
                Console.WriteLine("The id must be an integer greater than 0.");
                Console.Write("Please re-enter the id: ");
                break;
            case TableFields.Date:
                Console.WriteLine("The date must be valid and in YYYY-mm-dd format.");
                Console.Write("Please re-enter the date: ");
                break;
            case TableFields.Count:
                Console.WriteLine("Habit count must be a number greater than 0.");
                Console.Write("Please re-enter the habit count: ");
                break;
            default:
                break;
        }
    }
}

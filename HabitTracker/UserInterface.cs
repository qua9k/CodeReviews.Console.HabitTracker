namespace HabitTracker;

static class UserInterface
{
    public static void Pause()
    {
        Console.WriteLine("\nPress any key to return to the main menu.");
        Console.ReadKey();
    }

    public static void PrintMenuOptions()
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

    public static void PrintInputUnknown()
    {
        Console.Clear();
        Console.WriteLine("Your input was not understood.");
        Pause();
    }
}

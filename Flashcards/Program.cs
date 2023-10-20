namespace Flashcards;

internal static class Program
{
    static void Main()
    {
        var screen = UI.MainMenu.Get();
        screen.Show();
        Console.Clear();
    }
}

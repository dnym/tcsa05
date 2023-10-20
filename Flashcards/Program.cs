using Flashcards.Models;

namespace Flashcards;

internal static class Program
{
    public static readonly List<Stack> Stacks = new();
    public static readonly List<Flashcard> Flashcards = new();
    public static readonly List<StudySession> StudySessions = new();

    static void Main()
    {
        var screen = UI.MainMenu.Get();
        screen.Show();
        Console.Clear();
    }
}

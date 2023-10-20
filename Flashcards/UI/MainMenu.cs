using TCSAHelper.Console;

namespace Flashcards.UI;

internal static class MainMenu
{
    public static Screen Get()
    {
        const string header = "Flashcards";
        const string body = @"1. [S]tudy Session
2. [M]anage Stacks & Flashcards
3. View Study [H]istory
0. [Q]uit";
        const string footer = "Select by pressing a number or letter.";

        var screen = new Screen(header: (_, _) => header, body: (_, _) => body, footer: (_, _) => footer);

        void StudySession() => Console.WriteLine("StudySessionMenu.Get().Show()");
        void ManageStacks() => Console.WriteLine("ManageStacksMenu.Get().Show()");
        void StudyHistory() => Console.WriteLine("StudyHistoryMenu.Get().Show()");

        screen.AddAction(ConsoleKey.S, StudySession);
        screen.AddAction(ConsoleKey.D1, StudySession);
        screen.AddAction(ConsoleKey.M, ManageStacks);
        screen.AddAction(ConsoleKey.D2, ManageStacks);
        screen.AddAction(ConsoleKey.H, StudyHistory);
        screen.AddAction(ConsoleKey.D3, StudyHistory);
        screen.AddAction(ConsoleKey.Q, screen.ExitScreen);
        screen.AddAction(ConsoleKey.D0, screen.ExitScreen);

        return screen;
    }
}

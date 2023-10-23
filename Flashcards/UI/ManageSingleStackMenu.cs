using TCSAHelper.Console;

namespace Flashcards.UI;

internal static class ManageSingleStackMenu
{
    public static Screen Get(int stackId)
    {
        var stack = Program.Stacks.Find(s => s.Id == stackId)!;

        var screen = new Screen(header: (_, _) => $"Manage Stack: {stack.ViewName}",
            body: (_, _) => @"1. [C]reate New Flashcards in Stack
2. [B]rowse Flashcards
3. [R]ename Stack
4. [D]elete Stack
0. Go Back to [S]tacks Overview",
            footer: (_, _) => "Select by pressing a number or letter,\nor press [Esc] to go back.");
        screen.AddAction(ConsoleKey.C, () => Console.WriteLine("Create Flashcards"));
        screen.AddAction(ConsoleKey.D1, () => Console.WriteLine("Create Flashcards"));

        screen.AddAction(ConsoleKey.B, () => ManageFlashcardsMenu.Get().Show());
        screen.AddAction(ConsoleKey.D2, () => ManageFlashcardsMenu.Get().Show());

        screen.AddAction(ConsoleKey.R, () => CreateOrRenameStackMenu.Get(stack.ViewName).Show());
        screen.AddAction(ConsoleKey.D3, () => CreateOrRenameStackMenu.Get(stack.ViewName).Show());

        void DeleteStack()
        {
            Program.Flashcards.RemoveAll(f => f.Stack == stack);
            Program.Stacks.Remove(stack);
            screen.ExitScreen();
        }
        screen.AddAction(ConsoleKey.D, DeleteStack);
        screen.AddAction(ConsoleKey.D4, DeleteStack);

        screen.AddAction(ConsoleKey.S, screen.ExitScreen);
        screen.AddAction(ConsoleKey.D0, screen.ExitScreen);
        screen.AddAction(ConsoleKey.Escape, screen.ExitScreen);
        return screen;
    }
}

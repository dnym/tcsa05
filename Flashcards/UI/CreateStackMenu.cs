using Flashcards.Models;
using TCSAHelper.Console;

namespace Flashcards.UI;

internal static class CreateStackMenu
{
    public static Screen Get()
    {
        const string header = "Create Stack";
        const string footer = "Press [Enter] to create the stack\nor [Esc] to cancel.";
        string newStackMessage = string.Empty;
        string error = string.Empty;

        var screen = new Screen(header: (_, _) => header, body: (_, _) => $"{error}Enter the name of the stack: {newStackMessage}", footer: (_, _) =>
        {
            if (string.IsNullOrEmpty(newStackMessage))
            {
                return footer;
            }
            else
            {
                return "Press any key to go back.";
            }
        });
        screen.AddAction(ConsoleKey.Escape, screen.ExitScreen);
        screen.SetPromptAction((userInput) =>
        {
            var newStack = new Stack(userInput);

            var otherStack = Program.Stacks.Find(s => s.SortName == newStack.SortName);

            if (otherStack != null)
            {
                error = $"Your chosen stack name clashes with the existing stack \"{otherStack.ViewName}\".\n\n";
            }
            else if (string.IsNullOrEmpty(newStack.SortName))
            {
                error = "The stack name cannot be empty.\n\n";
            }
            else
            {
                error = string.Empty;
                Program.Stacks.Add(newStack);
                newStack.Id = ++Program.CurrentStackId;
                newStackMessage = $"{newStack.ViewName}\n\nCreated stack \"{newStack.ViewName}\".";
                screen.SetPromptAction(null);
                screen.SetAnyKeyAction(screen.ExitScreen);
            }
        });

        return screen;
    }
}

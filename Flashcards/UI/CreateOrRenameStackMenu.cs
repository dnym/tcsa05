using Flashcards.Models;
using TCSAHelper.Console;

namespace Flashcards.UI;

internal static class CreateOrRenameStackMenu
{
    public static Screen Get(string? oldViewName = null)
    {
        string newStackMessage = string.Empty;
        string error = string.Empty;

        var screen = new Screen(header: (_, _) =>
        {
            if (string.IsNullOrEmpty(oldViewName))
            {
                return "Create Stack";
            }
            else
            {
                return $"Rename Stack: {oldViewName}";
            }
        }, body: (_, _) => $"{error}Enter the name of the stack: {newStackMessage}", footer: (_, _) =>
        {
            if (string.IsNullOrEmpty(newStackMessage))
            {
                return $"Press [Enter] to {(string.IsNullOrEmpty(oldViewName) ? "create" : "rename")} the stack\nor [Esc] to cancel.";
            }
            else
            {
                return "Press any key to go back.";
            }
        });
        screen.AddAction(ConsoleKey.Escape, screen.ExitScreen);
        screen.SetPromptAction((userInput) =>
        {
            if (string.IsNullOrEmpty(oldViewName))
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
            }
            else
            {
                var newSortName = Stack.CreateSortName(userInput);
                if (string.IsNullOrEmpty(newSortName))
                {
                    error = "The stack name cannot be empty.\n\n";
                }
                else if (userInput == oldViewName)
                {
                    error = string.Empty;
                    newStackMessage = $"{oldViewName}\n\nNo name change.";
                    screen.SetPromptAction(null);
                    screen.SetAnyKeyAction(screen.ExitScreen);
                }
                else
                {
                    var otherStack = Program.Stacks.Find(s => s.SortName == newSortName);
                    if (otherStack != null && otherStack.SortName == newSortName && otherStack.ViewName != oldViewName)
                    {
                        error = $"Your chosen stack name clashes with the existing stack \"{otherStack.ViewName}\".\n\n";
                    }
                    else
                    {
                        error = string.Empty;
                        var stack = Program.Stacks.Find(s => s.ViewName == oldViewName)!;
                        stack.ViewName = userInput;
                        newStackMessage = $"{stack.ViewName}\n\nRenamed stack \"{oldViewName}\" to \"{stack.ViewName}\".";
                        screen.SetPromptAction(null);
                        screen.SetAnyKeyAction(screen.ExitScreen);
                    }
                }
            }
        });

        return screen;
    }
}

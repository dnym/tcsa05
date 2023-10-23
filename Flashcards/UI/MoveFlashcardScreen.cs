using Flashcards.Models;
using TCSAHelper.Console;

namespace Flashcards.UI;

internal static class MoveFlashcardScreen
{
    public static Screen Get(int flashcardId)
    {
        var card = Program.Flashcards.Find(f => f.Id == flashcardId) ?? throw new ArgumentException($"No flashcard with ID {flashcardId} exists.");
        string error = string.Empty;

        Screen screen = new(header: (_, _) => "Move Flashcard", body: (_, _) => $"{error}The card is currently in the \"{card.Stack.ViewName}\" stack.\n\nEnter another stack's name: ", footer: (_, _) => "Press [Esc] to cancel.");
        screen.AddAction(ConsoleKey.Escape, screen.ExitScreen);
        screen.SetPromptAction((userInput) =>
        {
            var otherStackName = Stack.CreateSortName(userInput);
            var otherStack = Program.Stacks.Find(s => s.SortName == otherStackName);
            if (string.IsNullOrEmpty(userInput))
            {
                error = "Enter a stack name.\n\n";
            }
            else if (otherStack == null)
            {
                error = "There is no stack with that name.\n\n";
            }
            else if (otherStack.SortName == card.Stack.SortName)
            {
                error = "The card is already in that stack.\n\n";
            }
            else
            {
                card.Stack.Cards--;
                card.Stack = otherStack;
                card.Stack.Cards++;

                screen.ExitScreen();
            }
        });

        return screen;
    }
}

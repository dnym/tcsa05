using Flashcards.Models;
using TCSAHelper.Console;

namespace Flashcards.UI;

internal static class CreateOrEditFlashcard
{
    public static Screen Get(int stackId, int? flashcardId = null)
    {
        string? front = null;
        string? back = null;
        var stack = Program.Stacks.Find(s => s.Id == stackId) ?? throw new ArgumentException($"No stack with ID {stackId} exists.");
        var card = Program.Flashcards.Find(f => f.Id == (flashcardId ?? -1));
        int cardsCreated = 0;

        Screen screen = new(header: (_, _) =>
        {
            if (card == null)
            {
                return $"Creating Flashcards in Stack: {stack.ViewName}";
            }
            else
            {
                return $"Editing Flashcard in Stack: {stack.ViewName}";
            }
        }, body: (_, _) =>
        {
            if (string.IsNullOrEmpty(front))
            {
                return "Enter a front side question: ";
            }
            else if (string.IsNullOrEmpty(back))
            {
                return $"Enter a front side question: {front}\n\nEnter a back side answer: ";
            }
            else
            {
                return "Card updated.";
            }
        }, footer: (_, _) =>
        {
            if (flashcardId == null)
            {
                return $"{cardsCreated} cards created.\nPress [Esc] to go back to the stack.";
            }
            else if (front == null || back == null)
            {
                return "Press [Esc] to go back.";
            }
            else
            {
                return "Press any key to go back.";
            }
        });
        screen.AddAction(ConsoleKey.Escape, screen.ExitScreen);

        screen.SetDefaultUserInput(card?.Front);
        screen.SetPromptAction((userInput) =>
        {
            if (!string.IsNullOrEmpty(userInput))
            {
                if (string.IsNullOrEmpty(front))
                {
                    front = userInput;
                    screen.SetDefaultUserInput(card?.Back);
                }
                else if (string.IsNullOrEmpty(back))
                {
                    back = userInput;
                }

                if (front != null && back != null)
                {
                    if (card == null)
                    {
                        var newCard = new Flashcard(front, back, stack)
                        {
                            Id = ++Program.CurrentFlashcardId
                        };
                        Program.Flashcards.Add(newCard);
                        cardsCreated++;
                        front = null;
                        back = null;
                    }
                    else
                    {
                        card.Front = front;
                        card.Back = back;
                        screen.SetPromptAction(null);
                        screen.SetAnyKeyAction(screen.ExitScreen);
                    }
                }
            }
            else
            {
                Console.Beep();
            }
        });

        return screen;
    }
}

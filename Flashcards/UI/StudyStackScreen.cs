using ConsoleTableExt;
using Flashcards.DataAccess;
using TCSAHelper.Console;

namespace Flashcards.UI;

internal static class StudyStackScreen
{
    internal static Screen Get(IDataAccess dataAccess, int stackId, string stackName)
    {
        string header = $"Studying Stack: {stackName}";
        var flashcards = dataAccess.GetFlashcardListAsync(stackId).Result ?? new();
        Random rnd = new();
        DataAccess.DTOs.ExistingFlashcard? flashcard = null;
        if (flashcards.Count > 0)
        {
            flashcard = flashcards[rnd.Next(flashcards.Count)];
        }
        string? userAnswer = null;

        var screen = new Screen(header: (_, _) => header,
            body: (usableWidth, _) =>
            {
                int maxWidth = usableWidth - 2;
                if (flashcard != null)
                {
                    var tableData = new List<List<object>>
                    {
                        new List<object>() { TCSAHelper.General.Utils.LimitWidth(flashcard.Front.Replace("\\n", "\n"), maxWidth) }
                    };
                    string prompt;
                    if (userAnswer == null)
                    {
                        tableData.Add(new List<object>() { "???" });
                        prompt = "\n\nEnter your answer:\n  ";
                    }
                    else
                    {
                        tableData.Add(new List<object>() { TCSAHelper.General.Utils.LimitWidth(flashcard.Back.Replace("\\n", "\n"), maxWidth) });
                        if (userAnswer == flashcard.Back)
                        {
                            prompt = "\n\nYou were correct!";
                        }
                        else
                        {
                            prompt = "\n\nBetter luck next time!";
                        }
                    }
                    return ConsoleTableBuilder.From(tableData)
                        .WithCharMapDefinition(CharMapDefinition.FramePipDefinition)
                        .Export()
                        + prompt;
                }
                else
                {
                    return "There are no cards to study in this stack.";
                }
            },
            footer: (_, _) =>
            {
                if (userAnswer == null)
                {
                    return "Press [Esc] to go back to the stack.";
                }
                else
                {
                    return "Press [Esc] to go back to the stack,\nor any other key to go to the next question.";
                }
            });

        screen.SetPromptAction((string userInput) =>
        {
            userAnswer = userInput;
            screen.SetPromptAction(null);
        });
        screen.SetAnyKeyAction(() =>
        {
            if (userAnswer != null)
            {
                flashcard = flashcards[rnd.Next(flashcards.Count)];
                userAnswer = null;
                screen.SetPromptAction((string userInput) =>
                {
                    userAnswer = userInput;
                    screen.SetPromptAction(null);
                });
            }
        });

        screen.AddAction(ConsoleKey.Escape, screen.ExitScreen);

        return screen;
    }
}

using ConsoleTableExt;
using Flashcards.DataAccess;
using Flashcards.DataAccess.DTOs;
using TCSAHelper.Console;

namespace Flashcards.UI;

internal static class StudyStackScreen
{
    internal static Screen Get(IDataAccess dataAccess, int stackId, string stackName)
    {
        string header = $"Studying Stack: {stackName}";
        var flashcards = dataAccess.GetFlashcardListAsync(stackId).Result ?? new();
        Random rnd = new();
        ExistingFlashcard? currentFlashcard = null;
        if (flashcards.Count > 0)
        {
            currentFlashcard = flashcards[rnd.Next(flashcards.Count)];
        }
        string? userAnswer = null;

        NewHistory sessionHistory = new() { StackId = stackId, StartedAt = DateTime.UtcNow };

        var screen = new Screen(header: (_, _) => header,
            body: (usableWidth, _) =>
            {
                int maxWidth = usableWidth - 2;
                if (currentFlashcard != null)
                {
                    var tableData = new List<List<object>>
                    {
                        new List<object>() { TCSAHelper.General.Utils.LimitWidth(currentFlashcard.Front.Replace("\\n", "\n"), maxWidth) }
                    };
                    string prompt;
                    if (userAnswer == null)
                    {
                        tableData.Add(new List<object>() { "???" });
                        prompt = "\n\nEnter your answer:\n  ";
                    }
                    else
                    {
                        tableData.Add(new List<object>() { TCSAHelper.General.Utils.LimitWidth(currentFlashcard.Back.Replace("\\n", "\n"), maxWidth) });
                        if (userAnswer == currentFlashcard.Back)
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

        void CheckAnswer(string userInput)
        {
            userAnswer = userInput;
            if (currentFlashcard != null)
            {
                var result = new CardStudyDTO() { FlashcardId = currentFlashcard.Id, AnsweredAt = DateTime.UtcNow, WasCorrect = userAnswer == currentFlashcard.Back };
                sessionHistory.Results.Add(result);
            }
            screen.SetPromptAction(null);
        }
        screen.SetPromptAction(CheckAnswer);
        screen.SetAnyKeyAction(() =>
        {
            if (userAnswer != null)
            {
                currentFlashcard = flashcards[rnd.Next(flashcards.Count)];
                userAnswer = null;
                screen.SetPromptAction(CheckAnswer);
            }
        });

        screen.AddAction(ConsoleKey.Escape, () =>
        {
            dataAccess.AddHistoryAsync(sessionHistory).Wait();
            screen.ExitScreen();
        });

        return screen;
    }
}

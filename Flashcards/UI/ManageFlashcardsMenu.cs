using Flashcards.Models;
using System.Text;
using TCSAHelper.Console;

namespace Flashcards.UI;

internal static class ManageFlashcardsMenu
{
    public static Screen Get(int stackId)
    {
        const string promptText = "\nSelect a Flashcard: ";
        Stack? stack = Program.Stacks.Find(s => s.Id == stackId);
        List<Flashcard> flashcards = Program.Flashcards.Where(f => f.Stack == stack).ToList();

        Screen screen = new(header: (_, _) => $"Manage Flashcards for {stack.ViewName}", body: (_, _) =>
        {
            if (flashcards.Any())
            {
                return GetFlashcardList(flashcards) + promptText;
            }
            else
            {
                return "This stack has no flashcards.";
            }
        }, footer: (_, _) => "Press [Esc] to go back.");

        if (flashcards.Any())
        {
            screen.SetPromptAction((_) => { });
        }
        screen.AddAction(ConsoleKey.Escape, screen.ExitScreen);

        return screen;
    }

    private static string GetFlashcardList(List<Flashcard> flashcards)
    {
        var sb = new StringBuilder();
        for (int i = 0; i < flashcards.Count; i++)
        {
            var flashcard = flashcards[i];
            sb.Append(i + 1).Append(": ").Append(flashcard.Front).Append(" -> ").AppendLine(flashcard.Back);
        }
        return sb.ToString();
    }
}

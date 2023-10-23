using static TCSAHelper.Console.Utils;
using Flashcards.Models;
using System.Text;
using TCSAHelper.Console;

namespace Flashcards.UI;

internal static class ManageFlashcardsMenu
{
    public static Screen Get(int stackId)
    {
        const int headerHeight = 1;
        const int footerHeight = Screen.FooterPadding + Screen.FooterSeparatorHeight + 3;
        const string promptText = "\nSelect a Flashcard: ";
        const int promptHeight = 2;
        PaginationResult? paginationResult = null;
        int previouslyUsableHeight = -1;
        int skip = 0;

        Stack? stack = Program.Stacks.Find(s => s.Id == stackId);
        List<Flashcard> flashcards = Program.Flashcards.Where(f => f.Stack == stack).ToList();

        Screen screen = new(header: (_, usableHeight) =>
        {
            if (usableHeight != previouslyUsableHeight)
            {
                previouslyUsableHeight = usableHeight;
                skip = 0;
            }
            int heightAvailableToBody = usableHeight - (headerHeight + footerHeight);
            paginationResult = DeterminePagination(heightAvailableToBody, flashcards.Count, perPageListHeightOverhead: promptHeight, skippedItems: skip);
            if (paginationResult.TotalPages > 1)
            {
                return $"Manage Flashcards for {stack.ViewName} (page {paginationResult.CurrentPage}/{paginationResult.TotalPages})";
            }
            else
            {
                return $"Manage Flashcards for {stack.ViewName}";
            }

        }, body: (_, _) =>
        {
            if (flashcards.Any())
            {
                var take = paginationResult!.ItemsPerPage;
                return GetFlashcardList(flashcards, skip, take) + promptText;
            }
            else if (flashcards.Any() && paginationResult!.TotalPages == 0)
            {
                return "Window is too small to list any flashcards.";
            }
            else
            {
                return "This stack has no flashcards.";
            }
        }, footer: (_, _) =>
        {
            var footerText = "Press ";
            if (paginationResult!.CurrentPage > 1)
            {
                footerText += "[PgUp] to go to the previous page,\n";
            }
            if (paginationResult.CurrentPage < paginationResult.TotalPages)
            {
                footerText += "[PgDown] to go to the next page,\n";
            }
            if (footerText.Length > 6)
            {
                footerText += "or ";
            }
            footerText += "[Esc] to go back.";
            return footerText;
        });

        if (flashcards.Any())
        {
            screen.SetPromptAction((userInput) =>
            {
                if (int.TryParse(userInput, out int flashcardNumber) && flashcardNumber > 0 && flashcardNumber <= paginationResult!.ItemsPerPage)
                {
                    int flashcardIndex = flashcardNumber - 1 + skip;
                    var flashcard = flashcards[flashcardIndex];
                    Console.WriteLine($"ViewFlashcardScreen.Get({flashcard.Id}).Show();");
                }
            });
        }

        screen.AddAction(ConsoleKey.PageUp, () =>
        {
            if (paginationResult!.CurrentPage > 1)
            {
                skip -= paginationResult.ItemsPerPage;
            }
        });
        screen.AddAction(ConsoleKey.PageDown, () =>
        {
            if (paginationResult!.CurrentPage < paginationResult.TotalPages)
            {
                skip += paginationResult.ItemsPerPage;
            }
        });
        screen.AddAction(ConsoleKey.Escape, screen.ExitScreen);

        return screen;
    }

    private static string GetFlashcardList(List<Flashcard> flashcards, int skip, int take)
    {
        var sb = new StringBuilder();
        var subset = flashcards.Skip(skip).Take(take).ToList();
        for (int i = 0; i < subset.Count; i++)
        {
            var flashcard = subset[i];
            sb.Append(i + 1).Append(": ").Append(flashcard.Front).Append(" -> ").AppendLine(flashcard.Back);
        }
        return sb.ToString();
    }
}

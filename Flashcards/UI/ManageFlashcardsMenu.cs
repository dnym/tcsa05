﻿using static TCSAHelper.Console.Utils;
using Flashcards.Models;
using System.Text;
using TCSAHelper.Console;
using Flashcards.DataAccess;
using Flashcards.DataAccess.DTOs;

namespace Flashcards.UI;

internal static class ManageFlashcardsMenu
{
    public static Screen Get(IDataAccess dataAccess, int stackId)
    {
        const int headerHeight = 1;
        const int footerHeight = Screen.FooterPadding + Screen.FooterSeparatorHeight + 3;
        const string promptText = "\nSelect a Flashcard: ";
        const int promptHeight = 2;
        PaginationResult? paginationResult = null;
        int previouslyUsableHeight = -1;
        int skip = 0;

        var stack = dataAccess.GetStackListItemByIdAsync(stackId).Result;
        List<ExistingFlashcard> flashcards = new();

        Screen screen = new(header: (_, usableHeight) =>
        {
            int flashcardsCount = dataAccess.CountFlashcardsAsync(stackId).Result;

            if (usableHeight != previouslyUsableHeight)
            {
                previouslyUsableHeight = usableHeight;
                skip = 0;
            }
            else if (flashcardsCount > 0 && skip >= flashcardsCount)
            {
                skip = flashcardsCount - (paginationResult?.ItemsPerPage ?? 0);
            }
            else if (skip < 0)
            {
                skip = 0;
            }

            int heightAvailableToBody = usableHeight - (headerHeight + footerHeight);
            paginationResult = DeterminePagination(heightAvailableToBody, flashcardsCount, perPageListHeightOverhead: promptHeight, skippedItems: skip);
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
            int flashcardsCount = dataAccess.CountFlashcardsAsync(stackId).Result;

            if (flashcardsCount > 0)
            {
                var take = paginationResult!.ItemsPerPage;
                flashcards = dataAccess.GetFlashcardListAsync(stackId, take, skip).Result;
                return GetFlashcardList(flashcards) + promptText;
            }
            else if (flashcardsCount > 0 && paginationResult!.TotalPages == 0)
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
            if (paginationResult!.CurrentPage < paginationResult.TotalPages)
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

        if (dataAccess.CountFlashcardsAsync(stackId).Result > 0)
        {
            screen.SetPromptAction((userInput) =>
            {
                if (int.TryParse(userInput, out int flashcardNumber) && flashcardNumber > 0 && flashcardNumber <= paginationResult!.ItemsPerPage && flashcardNumber <= flashcards.Count)
                {
                    var flashcard = flashcards[flashcardNumber - 1];
                    ViewFlashcardScreen.Get(dataAccess, flashcard.Id).Show();
                }
                else
                {
                    Console.Beep();
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

    private static string GetFlashcardList(List<ExistingFlashcard> flashcards)
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

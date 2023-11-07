﻿using static TCSAHelper.Console.Utils;
using ConsoleTableExt;
using Flashcards.DataAccess;
using Flashcards.DataAccess.DTOs;
using TCSAHelper.Console;

namespace Flashcards.UI;

internal static class StudyHistoryMenu
{
    internal static Screen Get(IDataAccess dataAccess)
    {
        const int headerHeight = 1;
        const int footerHeight = Screen.FooterPadding + Screen.FooterSeparatorHeight + 3;
        const string promptText = "\nSelect a Session: ";
        const int constantListOverhead = 2;
        const int perItemHeight = 2;
        const int promptHeight = 2;
        PaginationResult? paginationResult = null;
        int previouslyUsableHeight = -1;
        int skip = 0;

        var historyCount = dataAccess.CountHistoryAsync().Result;

        var screen = new Screen(header: (_, usableHeight) =>
        {
            if (usableHeight != previouslyUsableHeight)
            {
                previouslyUsableHeight = usableHeight;
                skip = 0;
            }
            else if (historyCount > 0 && skip >= historyCount)
            {
                skip = historyCount - (paginationResult?.ItemsPerPage ?? 0);
            }
            else if (skip < 0)
            {
                skip = 0;
            }

            int heightAvailableToBody = usableHeight - (headerHeight + footerHeight);
            paginationResult = DeterminePagination(heightAvailableToBody, historyCount, heightPerItem: perItemHeight, perPageListHeightOverhead: constantListOverhead + promptHeight, skippedItems: skip);
            if (paginationResult.TotalPages > 1)
            {
                return $"Study History (page {paginationResult.CurrentPage}/{paginationResult.TotalPages})";
            }
            else
            {
                return "Study History";
            }
        }, body: (_, _) =>
        {
            if (historyCount > 0)
            {
                var take = paginationResult!.ItemsPerPage;
                var historyList = dataAccess.GetHistoryListAsync(take, skip).Result;
                return ListToString(historyList) + promptText;
            }
            else
            {
                return "There is no study history recorded.";
            }
        }, footer: (_, _) =>
        {
            return "Press ...[Esc] to go back.";
        });

        if (historyCount > 0)
        {
            screen.SetPromptAction((userInput) =>
            {
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

    private static string ListToString(List<HistoryListItem> historyList)
    {
        var tableData = new List<List<object>>();
        for (int i = 0; i < historyList.Count; i++)
        {
            var listItem = historyList[i];
            tableData.Add(new List<object>()
            {
                (i + 1).ToString(),
                listItem.StartedAt.ToString(Program.DateTimeFormat),
                listItem.StackViewName,
                listItem.CardsStudied,
                $"{listItem.CorrectAnswers}/{listItem.CardsStudied}"
            });
        }
        return ConsoleTableBuilder.From(tableData)
            .WithColumn("no.", "date", "stack", "cards", "result")
            .WithCharMapDefinition(CharMapDefinition.FramePipDefinition)
            .Export().ToString();
    }
}

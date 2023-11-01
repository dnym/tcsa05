using static TCSAHelper.Console.Utils;
using TCSAHelper.Console;
using System.Text;
using Flashcards.Models;
using Flashcards.DataAccess;

namespace Flashcards.UI;

internal static class ManageStacksMenu
{
    public static Screen Get(IDataAccess dataAccess)
    {
        const int headerHeight = 1;
        // Actual footer height varies, but using a constant simplifies things (including for the user).
        const int footerHeight = Screen.FooterPadding + Screen.FooterSeparatorHeight + 4;
        const string promptText = "\nSelect a Stack: ";
        const int promptHeight = 2;
        PaginationResult? paginationResult = null;
        int previouslyUsableHeight = -1;
        int skip = 0;

        Screen screen = new(header: (_, usableHeight) =>
        {
            if (usableHeight != previouslyUsableHeight)
            {
                // Reset pagination when the window size changes.
                previouslyUsableHeight = usableHeight;
                skip = 0;
            }
            int heightAvailableToBody = usableHeight - (headerHeight + footerHeight);
            paginationResult = DeterminePagination(heightAvailableToBody, dataAccess.CountStacksAsync().Result, perPageListHeightOverhead: promptHeight, skippedItems: skip);
            if (paginationResult.TotalPages > 1)
            {
                return $"Manage Stacks (page {paginationResult.CurrentPage}/{paginationResult.TotalPages})";
            }
            else
            {
                return "Manage Stacks";
            }
        }, body: (_, _) =>
        {
            if (paginationResult!.TotalPages > 0)
            {
                var take = paginationResult.ItemsPerPage;
                return GetStackList(dataAccess, skip, take) + promptText;
            }
            else if (dataAccess.CountStacksAsync().Result > 0 && paginationResult!.TotalPages == 0)
            {
                // Note that this may actually not be true due to reserving space for PageUp and PageDown hints.
                // TODO: Consider whether always printing the PageUp and PageDown hints, to not annoy the user by refusing to print items when there is space.
                return "Window is too small to list any stacks.";
            }
            else
            {
                return "No stacks stored.";
            }
        }, footer: (_, _) =>
        {
            var footerText = "Press [F1] to create a new stack,\n";
            if (paginationResult!.CurrentPage > 1)
            {
                footerText += "[PgUp] to go to the previous page,\n";
            }
            if (paginationResult.CurrentPage < paginationResult.TotalPages)
            {
                footerText += "[PgDown] to go to the next page,\n";
            }
            footerText += "or [Esc] to go back.";
            return footerText;
        });

        void PromptHandler(string userInput)
        {
            var stackName = Stack.CreateSortName(userInput);
            var stack = dataAccess.GetStackListItemBySortNameAsync(stackName).Result;
            if (stack != null)
            {
                ManageSingleStackMenu.Get(dataAccess, stack.Id).Show();
                if (dataAccess.CountStacksAsync().Result == 0)
                {
                    screen.SetPromptAction(null);
                }
            }
            else
            {
                Console.Beep();
            }
        }

        screen.AddAction(ConsoleKey.F1, () =>
        {
            CreateOrRenameStackMenu.Get(dataAccess).Show();

            if (dataAccess.CountStacksAsync().Result > 0)
            {
                screen.SetPromptAction(PromptHandler);
            }
            else
            {
                screen.SetPromptAction(null);
            }
        });
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

        if (dataAccess.CountStacksAsync().Result > 0)
        {
            screen.SetPromptAction(PromptHandler);
        }

        return screen;
    }

    private static string GetStackList(IDataAccess dataAccess, int skip, int take)
    {
        var sb = new StringBuilder();
        // TODO: Determine which sort order to use here.
        foreach (var stack in dataAccess.GetStackListAsync(take, skip).Result)
        {
            sb.Append(stack.ViewName).Append('\t').Append(stack.Cards).Append('\t').AppendLine(stack.LastStudied?.ToString(Program.DateTimeFormat) ?? "(never)");
        }
        return sb.ToString();
    }
}

using static TCSAHelper.Console.Utils;
using TCSAHelper.Console;
using System.Text;
using Flashcards.Models;

namespace Flashcards.UI;

internal static class ManageStacksMenu
{
    public static Screen Get()
    {
        const int headerHeight = 1 + Screen.HeaderSeparatorHeight + Screen.HeaderPadding;
        // Actual footer height varies, but using a constant simplifies things (including for the user).
        const int footerHeight = Screen.FooterPadding + Screen.FooterSeparatorHeight + 4;
        const int promptHeight = 2;
        PaginationResult? paginationResult = null;
        int skip = 0;

        Screen screen = new(header: (_, usableHeight) =>
        {
            int heightAvailableToBody = usableHeight - (headerHeight + footerHeight);
            paginationResult = DeterminePagination(heightAvailableToBody, Program.Stacks.Count, perPageListHeightOverhead: promptHeight, skippedItems: skip);
            if (paginationResult.TotalPages > 0)
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
                return GetStackList(skip, take);
            }
            else
            {
                return "No stacks stored.";
            }
        }, footer: (_, _) =>
        {
            var footerText = "Press [N] to create a new stack,\n";
            if (paginationResult!.CurrentPage > 1)
            {
                footerText += " [PgUp] to go to the previous page,\n";
            }
            if (paginationResult.CurrentPage < paginationResult.TotalPages)
            {
                footerText += " [PgDown] to go to the next page,\n";
            }
            footerText += "or [Esc] to go back.";
            return footerText;
        });

        static void PromptHandler(string _/*userInput*/)
        {
        }

        screen.AddAction(ConsoleKey.N, () =>
        {
            Console.WriteLine("CreateStackMenu.Get().Show()");

            if (Program.Stacks.Count > 0)
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

        if (Program.Stacks.Count > 0)
        {
            screen.SetPromptAction(PromptHandler);
        }

        return screen;
    }

    private static string GetStackList(int skip, int take)
    {
        var sb = new StringBuilder();
        // TODO: Determine which sort order to use here.
        foreach (Stack stack in Program.Stacks.Skip(skip).Take(take))
        {
            sb.Append(stack.ViewName).Append('\t').Append(stack.Cards).Append('\t').AppendLine(GetLastStudyDate(stack));
        }
        return sb.ToString();
    }

    private static string GetLastStudyDate(Stack stack)
    {
        var studySessions = Program.StudySessions.Where(s => s.Id == stack.Id);
        if (studySessions.Any())
        {
            return studySessions.Max(s => s.StartedAt).ToString(Program.DateTimeFormat);
        }
        else
        {
            return "(never)";
        }
    }
}

using TCSAHelper.Console;

namespace Flashcards.UI;

internal static class ViewFlashcardScreen
{
    public static Screen Get(int flashcardId)
    {
        var card = Program.Flashcards.Find(f => f.Id == flashcardId);

        Screen screen = new(header: (_, _) => $"Managing Flashcard in Stack: {card.Stack.ViewName}", body: (_, _) => $"Front side question:\n  {card.Front}\n\nBack side answer:\n  {card.Back}", footer: (_, _) =>
        @"Press [E] to edit the flashcard, [D] to delete,
[M] to move it to a different stack,
or [Esc] to go back to the stack.");

        screen.AddAction(ConsoleKey.E, () => Console.Write("Edit flashcard"));
        screen.AddAction(ConsoleKey.D, () =>
        {
            Program.Flashcards.Remove(card);
            screen.ExitScreen();
        });
        screen.AddAction(ConsoleKey.M, () => Console.Write("Move flashcard"));
        screen.AddAction(ConsoleKey.Escape, screen.ExitScreen);

        return screen;
    }
}

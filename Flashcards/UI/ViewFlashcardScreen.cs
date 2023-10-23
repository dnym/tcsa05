using TCSAHelper.Console;

namespace Flashcards.UI;

internal static class ViewFlashcardScreen
{
    public static Screen Get(int flashcardId)
    {
        var card = Program.Flashcards.Find(f => f.Id == flashcardId) ?? throw new ArgumentException($"No flashcard with ID {flashcardId} exists.");

        Screen screen = new(header: (_, _) => $"Managing Flashcard in Stack: {card.Stack.ViewName}", body: (_, _) => $"Front side question:\n  {card.Front}\n\nBack side answer:\n  {card.Back}", footer: (_, _) =>
        @"Press [E] to edit the flashcard, [D] to delete,
[M] to move it to a different stack,
or [Esc] to go back to the stack.");

        screen.AddAction(ConsoleKey.E, () => CreateOrEditFlashcard.Get(card.Stack.Id, card.Id).Show());
        screen.AddAction(ConsoleKey.D, () =>
        {
            Program.Flashcards.Remove(card);
            card.Stack.Cards--;
            screen.ExitScreen();
        });
        screen.AddAction(ConsoleKey.M, () => MoveFlashcardScreen.Get(card.Id).Show());
        screen.AddAction(ConsoleKey.Escape, screen.ExitScreen);

        return screen;
    }
}

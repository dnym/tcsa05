namespace Flashcards.Models;

internal class CardStudy
{
    public CardStudy(Flashcard card, bool wasCorrect, DateTime answeredAt)
    {
        Card = card;
        WasCorrect = wasCorrect;
        AnsweredAt = answeredAt;
    }

    public Flashcard Card { get; set; }
    public bool WasCorrect { get; set; }
    public DateTime AnsweredAt { get; set; }
}

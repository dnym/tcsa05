namespace Flashcards.DataAccess.DTOs;

public class CardStudyDTO
{
    public int FlashcardId { get; set; }
    public bool WasCorrect { get; set; }
    public DateTime AnsweredAt { get; set; }
}

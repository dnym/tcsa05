namespace Flashcards.Models;

internal class StudySession
{
    public StudySession(DateTime startedAt, Stack stack)
    {
        StartedAt = startedAt;
        Stack = stack;
    }

    public int Id { get; set; } = -1;
    public DateTime StartedAt { get; set; }
    public Stack Stack { get; set; }
    public List<CardStudy> Results { get; set; } = new();
}

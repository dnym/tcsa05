namespace Flashcards.Models;

internal class Flashcard
{
    public Flashcard(string front, string back, Stack stack)
    {
        Front = front;
        Back = back;
        Stack = stack;
    }

    public int Id { get; set; } = -1;
    public string Front { get; set; }
    public string Back { get; set; }
    public Stack Stack { get; set; }

}

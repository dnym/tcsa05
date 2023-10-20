namespace Flashcards.Models;

internal class Stack
{
    public Stack(string viewName)
    {
        ViewName = viewName;
    }

    public int Id { get; set; } = -1;
    public string ViewName { get; set; }
    public string SortName => ViewName.Trim().ToLower();
    public int Cards { get; set; } = 0;
}

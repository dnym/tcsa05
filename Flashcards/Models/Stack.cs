namespace Flashcards.Models;

internal class Stack
{
    public Stack(string viewName)
    {
        ViewName = viewName;
    }

    public int Id { get; set; } = -1;
    public string ViewName { get; set; }
    public string SortName => CreateSortName(ViewName);
    public int Cards { get; set; } = 0;

    public static string CreateSortName(string viewName)
    {
        return viewName.Trim().ToLower();
    }
}

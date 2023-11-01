using Flashcards.DataAccess;
using Flashcards.DataAccess.MockDB;
using Flashcards.Models;
using TCSAHelper.General;

namespace Flashcards;

internal static class Program
{
    public const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

    static void Main()
    {
        IDataAccess _dataAccess = new MockDB();
        AddDummyData(_dataAccess, 12, 12);
        var screen = UI.MainMenu.Get(_dataAccess);
        screen.Show();
        Console.Clear();
    }

    private static void AddDummyData(IDataAccess dataAccess, int numberOfStacks, int cardsPerStack = 0)
    {
        var random = new Random();
        for (int i = 0; i < numberOfStacks; i++)
        {
            var stackName = LanguageName(random);
            var stack = new Stack(stackName);
            if (dataAccess.StackExistsAsync(stack.SortName).Result)
            {
                i--;
                continue;
            }
            dataAccess.CreateStackAsync(new DataAccess.DTOs.NewStack { SortName = stack.SortName, ViewName = stack.ViewName });
        }
        if (cardsPerStack > 0)
        {
            foreach (var stackListItem in dataAccess.GetStackListAsync().Result)
            {
                for (int i = 0; i < cardsPerStack; i++)
                {
                    var front = $"What's the {stackListItem.ViewName} word for {WordGenerator.CreateFakeWord(random)}?";
                    var back = WordGenerator.CreateFakeWord(random);
                    dataAccess.CreateFlashcardAsync(new DataAccess.DTOs.NewFlashcard { StackId = stackListItem.Id, Front = front, Back = back });
                }
            }
        }
    }

    private static string LanguageName(Random random)
    {
        var languageName = WordGenerator.CreateFakeWord(random);
        languageName = languageName[0].ToString().ToUpper() + languageName[1..];
        if (random.NextDouble() < 0.5)
        {
            languageName += "lese";
        }
        else
        {
            languageName += "ish";
        }
        return languageName;
    }
}

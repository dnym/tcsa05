using Flashcards.Models;
using System.Diagnostics;
using System;
using TCSAHelper.General;

namespace Flashcards;

internal static class Program
{
    public static readonly List<Stack> Stacks = new();
    public static readonly List<Flashcard> Flashcards = new();
    public static readonly List<StudySession> StudySessions = new();
    public static int CurrentStackId = -1;
    public static int CurrentFlashcardId = -1;
    public static int CurrentStudySessionId = -1;
    public const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

    static void Main()
    {
        AddDummyData(12, 77);
        var screen = UI.MainMenu.Get();
        screen.Show();
        Console.Clear();
    }

    private static void AddDummyData(int numberOfStacks, int cardsPerStack = 0)
    {
        var random = new Random();
        for (int i = 0; i < numberOfStacks; i++)
        {
            var stackName = LanguageName(random);
            var stack = new Stack(stackName);
            if (Stacks.Any(s => s.SortName == stack.SortName))
            {
                i--;
                continue;
            }
            Stacks.Add(stack);
            stack.Id = ++CurrentStackId;
        }
        if (cardsPerStack > 0)
        {
            foreach (Stack stack in Stacks)
            {
                for (int i = 0; i < cardsPerStack; i++)
                {
                    var front = $"What's the {stack.ViewName} word for {WordGenerator.CreateFakeWord(random)}?";
                    var back = WordGenerator.CreateFakeWord(random);
                    var newCard = new Flashcard(front, back, stack);
                    Flashcards.Add(newCard);
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

using Flashcards.DataAccess.DTOs;
using Flashcards.DataAccess.MockDB.Models;

namespace Flashcards.DataAccess.MockDB;

public class MockDB : IDataAccess
{
    private readonly List<StacksRow> _stacks = new();
    private readonly List<FlashcardsRow> _flashcards = new();
    private readonly List<HistoryRow> _history = new();
    private readonly List<HistoryToFlashcardRow> _historyToFlashcard = new();

    public Task<List<StackListItem>> GetStackListAsync(int take, int skip = 0)
    {
        var output = _stacks.Skip(skip).Take(take).Select(sr => new StackListItem() {
            Id = sr.IdPK,
            ViewName = sr.ViewName,
            Cards = _flashcards.Count(fr => fr.StackIdFK == sr.IdPK),
            LastStudied = _history.Find(h => h.StackIdFK == sr.IdPK)?.StartedAt
        }).ToList();
        return Task.FromResult(output);
    }

    public Task CreateStackAsync(NewStack stack)
    {
        var newStack = new StacksRow(stack.SortName, stack.ViewName);
        _stacks.Add(newStack);
        return Task.CompletedTask;
    }

    public Task<List<ExistingFlashcard>> GetFlashcardListAsync(int stackId)
    {
        var output = _flashcards.Where(fr => fr.StackIdFK == stackId).Select(fr => new ExistingFlashcard()
        {
            Id = fr.IdPK,
            Front = fr.Front,
            Back = fr.Back
        }).ToList();
        return Task.FromResult(output);
    }

    public Task CreateFlashcardAsync(NewFlashcard flashcard)
    {
        var newRow = new FlashcardsRow(flashcard.StackId, flashcard.Front, flashcard.Back);
        _flashcards.Add(newRow);
        return Task.CompletedTask;
    }

    public Task UpdateFlashcardAsync(ExistingFlashcard flashcard)
    {
        var found = _flashcards.Find(fr => fr.IdPK == flashcard.Id);
        if (found != null)
        {
            found.Front = flashcard.Front;
            found.Back = flashcard.Back;
        }
        return Task.CompletedTask;
    }

    public Task MoveFlashcardAsync(int flashcardId, int newStackId)
    {
        var found = _flashcards.Find(fr => fr.IdPK == flashcardId);
        if (found != null && _stacks.Any(sr => sr.IdPK == newStackId))
        {
            found.StackIdFK = newStackId;
        }
        return Task.CompletedTask;
    }

    public Task<List<HistoryListItem>> GetHistoryListAsync()
    {
        var output = _history.ConvertAll(hr => new HistoryListItem()
        {
            Id = hr.IdPK,
            StartedAt = hr.StartedAt,
            StackViewName = _stacks.Find(sr => sr.IdPK == hr.StackIdFK)?.ViewName ?? throw new ApplicationException("bad data: there's a history row with non-existant stack"),
            CardsStudied  = _historyToFlashcard.Count(h2f => h2f.HistoryIdFK == hr.IdPK),
            CorrectAnswers = _historyToFlashcard.Count(h2f => h2f.HistoryIdFK == hr.IdPK && h2f.WasCorrect)
        });
        return Task.FromResult(output);
    }

    public Task AddHistoryAsync(NewHistory history)
    {
        var newRow = new HistoryRow(history.StackId, history.StartedAt);
        _history.Add(newRow);
        foreach (var result in history.Results)
        {
            var newResult = new HistoryToFlashcardRow(newRow.IdPK, result.FlashcardId, result.WasCorrect, result.AnsweredAt);
            _historyToFlashcard.Add(newResult);
        }
        return Task.CompletedTask;
    }
}

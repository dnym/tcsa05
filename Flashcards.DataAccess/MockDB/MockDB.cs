using Flashcards.DataAccess.DTOs;
using Flashcards.DataAccess.MockDB.Models;
using System.Linq;

namespace Flashcards.DataAccess.MockDB;

public class MockDB : IDataAccess
{
    private readonly List<StacksRow> _stacks = new();
    private readonly List<FlashcardsRow> _flashcards = new();
    private readonly List<HistoryRow> _history = new();
    private readonly List<HistoryToFlashcardRow> _historyToFlashcard = new();

    public Task<int> CountStacksAsync(int? take = null, int skip = 0)
    {
        if (take == null)
        {
            return Task.FromResult(_stacks.Skip(skip).Count());
        }
        return Task.FromResult(_stacks.Skip(skip).Take((int)take).Count());
    }

    public Task<bool> StackExistsAsync(string sortName)
    {
        return Task.FromResult(_stacks.Any(sr => sr.SortNameUQ == sortName));
    }

    public Task<List<StackListItem>> GetStackListAsync(int? take = null, int skip = 0)
    {
        if (take == null)
        {
            var output = _stacks.Skip(skip).Select(sr => new StackListItem()
            {
                Id = sr.IdPK,
                ViewName = sr.ViewName,
                Cards = _flashcards.Count(fr => fr.StackIdFK == sr.IdPK),
                LastStudied = _history.Find(h => h.StackIdFK == sr.IdPK)?.StartedAt
            }).ToList();
            return Task.FromResult(output);
        }
        else
        {
            var output = _stacks.Skip(skip).Take((int)take).Select(sr => new StackListItem()
            {
                Id = sr.IdPK,
                ViewName = sr.ViewName,
                Cards = _flashcards.Count(fr => fr.StackIdFK == sr.IdPK),
                LastStudied = _history.Find(h => h.StackIdFK == sr.IdPK)?.StartedAt
            }).ToList();
            return Task.FromResult(output);
        }
    }

    public Task<StackListItem> GetStackListItemByIdAsync(int id)
    {
        var found = _stacks.Find(sr => sr.IdPK == id) ?? throw new ArgumentException($"No stack with ID {id} exists.");
        var output = new StackListItem
        {
            Id = found.IdPK,
            ViewName = found.ViewName,
            Cards = _flashcards.Count(fr => fr.StackIdFK == found.IdPK),
            LastStudied = _history.Find(h => h.StackIdFK == found.IdPK)?.StartedAt
        };
        return Task.FromResult(output);
    }

    public Task<StackListItem?> GetStackListItemBySortNameAsync(string sortName)
    {
        StackListItem? output = null;
        var found = _stacks.Find(sr => sr.SortNameUQ == sortName);
        if (found == null)
        {
            return Task.FromResult(output);
        }
        output = new StackListItem
        {
            Id = found.IdPK,
            ViewName = found.ViewName,
            Cards = _flashcards.Count(fr => fr.StackIdFK == found.IdPK),
            LastStudied = _history.Find(h => h.StackIdFK == found.IdPK)?.StartedAt
        };
        return Task.FromResult<StackListItem?>(output);
    }

    public Task<StackListItem> GetStackListItemByFlashcardIdAsync(int flashcardId)
    {
        var flashcard = _flashcards.Find(fr => fr.IdPK == flashcardId) ?? throw new ArgumentException($"No flashcard with ID {flashcardId} exists.");
        var found = _stacks.Find(sr => sr.IdPK == flashcard.StackIdFK) ?? throw new ApplicationException("Bad data: there's a flashcard with non-existant stack.");
        var output = new StackListItem
        {
            Id = found.IdPK,
            ViewName = found.ViewName,
            Cards = _flashcards.Count(fr => fr.StackIdFK == found.IdPK),
            LastStudied = _history.Find(h => h.StackIdFK == found.IdPK)?.StartedAt
        };
        return Task.FromResult(output);
    }

    public Task CreateStackAsync(NewStack stack)
    {
        var newStack = new StacksRow(stack.SortName, stack.ViewName);
        _stacks.Add(newStack);
        return Task.CompletedTask;
    }

    public Task DeleteStackAsync(int stackId)
    {
        var found = _stacks.Find(sr => sr.IdPK == stackId) ?? throw new ArgumentException($"No stack with ID {stackId} exists.");
        var flashcardsToRemove = _flashcards.FindAll(fr => fr.StackIdFK == stackId);
        var historyRowsToRemove = _history.FindAll(hr => hr.StackIdFK == stackId);
        _historyToFlashcard.RemoveAll(h2f => historyRowsToRemove.Any(hr => hr.IdPK == h2f.HistoryIdFK) || flashcardsToRemove.Any(fr => fr.IdPK == h2f.FlashcardIdFK));
        _history.RemoveAll(hr => historyRowsToRemove.Any(h => h.IdPK == hr.IdPK));
        _flashcards.RemoveAll(fr => flashcardsToRemove.Any(f => f.IdPK == fr.IdPK));
        _stacks.Remove(found);
        return Task.CompletedTask;
    }

    public Task RenameStackAsync(int oldStackId, NewStack updatedStack)
    {
        var found = _stacks.Find(sr => sr.IdPK == oldStackId) ?? throw new ArgumentException($"No stack with ID {oldStackId} exists.");
        found.SortNameUQ = updatedStack.SortName;
        found.ViewName = updatedStack.ViewName;
        return Task.CompletedTask;
    }

    public Task<int> CountFlashcardsAsync(int stackId)
    {
        return Task.FromResult(_flashcards.Count(fr => fr.StackIdFK == stackId));
    }

    public Task<bool> CardInStack(int stackId, int flashcardId)
    {
        return Task.FromResult(_flashcards.Any(fr => fr.IdPK == flashcardId && fr.StackIdFK == stackId));
    }

    public Task<List<ExistingFlashcard>> GetFlashcardListAsync(int stackId, int? take = null, int skip = 0)
    {
        if (take == null)
        {
            var output = _flashcards.Where(fr => fr.StackIdFK == stackId).Skip(skip).Select(fr => new ExistingFlashcard()
            {
                Id = fr.IdPK,
                Front = fr.Front,
                Back = fr.Back
            }).ToList();
            return Task.FromResult(output);
        }
        else
        {
            var output = _flashcards.Where(fr => fr.StackIdFK == stackId).Skip(skip).Take((int)take).Select(fr => new ExistingFlashcard()
            {
                Id = fr.IdPK,
                Front = fr.Front,
                Back = fr.Back
            }).ToList();
            return Task.FromResult(output);
        }
    }

    public Task<ExistingFlashcard> GetFlashcardByIdAsync(int id)
    {
        var found = _flashcards.Find(fr => fr.IdPK == id) ?? throw new ArgumentException($"No flashcard with ID {id} exists.");
        var output = new ExistingFlashcard()
        {
            Id = found.IdPK,
            Front = found.Front,
            Back = found.Back
        };
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

    public Task DeleteFlashcardAsync(int id)
    {
        var found = _flashcards.Find(fr => fr.IdPK == id) ?? throw new ArgumentException($"No flashcard with ID {id} exists.");
        _historyToFlashcard.RemoveAll(h2f => h2f.FlashcardIdFK == id);
        _flashcards.Remove(found);
        return Task.CompletedTask;
    }

    public Task<int> CountHistoryAsync()
    {
        return Task.FromResult(_history.Count);
    }

    public Task<List<HistoryListItem>> GetHistoryListAsync(int? take = null, int skip = 0)
    {
        var output = _history.ConvertAll(hr => new HistoryListItem()
        {
            Id = hr.IdPK,
            StartedAt = hr.StartedAt,
            StackViewName = _stacks.Find(sr => sr.IdPK == hr.StackIdFK)?.ViewName ?? throw new ApplicationException("Bad data: there's a history row with non-existant stack."),
            CardsStudied  = _historyToFlashcard.Count(h2f => h2f.HistoryIdFK == hr.IdPK),
            CorrectAnswers = _historyToFlashcard.Count(h2f => h2f.HistoryIdFK == hr.IdPK && h2f.WasCorrect)
        });
        if (take != null)
        {
            return Task.FromResult(output.Skip(skip).Take((int)take).ToList());
        }
        else
        {
            return Task.FromResult(output.Skip(skip).ToList());
        }
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

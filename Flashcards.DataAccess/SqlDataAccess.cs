using Flashcards.DataAccess.DTOs;

namespace Flashcards.DataAccess;

public class SqlDataAccess : IDataAccess
{
    private readonly string _connectionString;

    public SqlDataAccess(string connectionString)
    {
        _connectionString = connectionString;
    }

    public Task<int> CountStacksAsync(int? take = null, int skip = 0)
    {
        throw new NotImplementedException();
    }

    public Task<bool> StackExistsAsync(string sortName)
    {
        throw new NotImplementedException();
    }

    public Task<List<StackListItem>> GetStackListAsync(int? take = null, int skip = 0)
    {
        throw new NotImplementedException();
    }

    public Task<StackListItem> GetStackListItemByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<StackListItem?> GetStackListItemBySortNameAsync(string sortName)
    {
        throw new NotImplementedException();
    }

    public Task<StackListItem> GetStackListItemByFlashcardIdAsync(int flashcardId)
    {
        throw new NotImplementedException();
    }

    public Task CreateStackAsync(NewStack stack)
    {
        throw new NotImplementedException();
    }

    public Task DeleteStackAsync(int stackId)
    {
        throw new NotImplementedException();
    }

    public Task RenameStackAsync(int oldStackId, NewStack updatedStack)
    {
        throw new NotImplementedException();
    }

    public Task<int> CountFlashcardsAsync(int stackId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> CardInStack(int stackId, int flashcardId)
    {
        throw new NotImplementedException();
    }

    public Task<List<ExistingFlashcard>> GetFlashcardListAsync(int stackId, int? take = null, int skip = 0)
    {
        throw new NotImplementedException();
    }

    public Task<ExistingFlashcard> GetFlashcardByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task CreateFlashcardAsync(NewFlashcard flashcard)
    {
        throw new NotImplementedException();
    }

    public Task UpdateFlashcardAsync(ExistingFlashcard flashcard)
    {
        throw new NotImplementedException();
    }

    public Task MoveFlashcardAsync(int flashcardId, int newStackId)
    {
        throw new NotImplementedException();
    }

    public Task DeleteFlashcardAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<int> CountHistoryAsync()
    {
        throw new NotImplementedException();
    }

    public Task<List<HistoryListItem>> GetHistoryListAsync(int? take = null, int skip = 0)
    {
        throw new NotImplementedException();
    }

    public Task AddHistoryAsync(NewHistory history)
    {
        throw new NotImplementedException();
    }

    public Task<List<ExistingStudyResult>> GetStudyResults(int historyId, int? take = null, int skip = 0)
    {
        throw new NotImplementedException();
    }
}

using Flashcards.DataAccess.DTOs;

namespace Flashcards.DataAccess;

public interface IDataAccess
{
    Task<int> CountStacksAsync(int? take = null, int skip = 0);
    Task<bool> StackExistsAsync(string sortName);
    Task<List<StackListItem>> GetStackListAsync(int take, int skip = 0);
    Task CreateStackAsync(NewStack stack);
    Task<List<ExistingFlashcard>> GetFlashcardListAsync(int stackId);
    Task CreateFlashcardAsync(NewFlashcard flashcard);
    Task UpdateFlashcardAsync(ExistingFlashcard flashcard);
    Task MoveFlashcardAsync(int flashcardId, int newStackId);
    Task<List<HistoryListItem>> GetHistoryListAsync();
    Task AddHistoryAsync(NewHistory history);
}

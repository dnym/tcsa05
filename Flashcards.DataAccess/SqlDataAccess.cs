using Flashcards.DataAccess.DTOs;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;

namespace Flashcards.DataAccess;

public class SqlDataAccess : IDataAccess
{
    private readonly string _connectionString;

    public SqlDataAccess(string connectionString)
    {
        _connectionString = connectionString;

//        using var connection = new SqlConnection(_connectionString);
//        TryOrDie(connection.Open, "drop tables");
//        var cmd = connection.CreateCommand();
//        cmd.CommandText = @"DROP TABLE IF EXISTS StudyResult;
//DROP TABLE IF EXISTS History;
//DROP TABLE IF EXISTS Flashcard;
//DROP TABLE IF EXISTS Stack;";
//        TryOrDie(() => cmd.ExecuteNonQuery(), "drop tables");
//        connection.Close();
    }

    public async Task<int> CountStacksAsync(int? take = null, int skip = 0)
    {
        var output = 0;

        using var connection = new SqlConnection(_connectionString);
        await TryOrDieAsync(connection.OpenAsync, "count stacks");

        var cmd = connection.CreateCommand();
        cmd.CommandText = "dbo.Stack_Count_tr";
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Take", take);
        cmd.Parameters.AddWithValue("@Skip", skip);
        await TryOrDieAsync(async () =>
        {
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                output = reader.GetInt32(0);
            }
        }, "count stacks");

        connection.Close();

        return output;
    }

    public Task<bool> StackExistsAsync(string sortName)
    {
        throw new NotImplementedException();
    }

    public async Task<List<StackListItem>> GetStackListAsync(int? take = null, int skip = 0)
    {
        var output = new List<StackListItem>();

        using var connection = new SqlConnection(_connectionString);
        await TryOrDieAsync(connection.OpenAsync, "get stack list");

        var cmd = connection.CreateCommand();
        cmd.CommandText = "dbo.Stack_GetMultiple_tr";
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Take", take);
        cmd.Parameters.AddWithValue("@Skip", skip);
        await TryOrDieAsync(async () =>
        {
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var stack = new StackListItem
                {
                    Id = reader.GetInt32(0),
                    ViewName = reader.GetString(1),
                    Cards = reader.GetInt32(2)
                };
                if (!reader.IsDBNull(3))
                {
                    stack.LastStudied = reader.GetDateTime(3);
                }
                output.Add(stack);
            }
        }, "get stack list");

        connection.Close();

        return output;
    }

    public async Task<StackListItem> GetStackListItemByIdAsync(int id)
    {
        StackListItem? output = null;

        var connection = new SqlConnection(_connectionString);
        await TryOrDieAsync(connection.OpenAsync, "get stack list item by id");

        var cmd = connection.CreateCommand();
        cmd.CommandText = "dbo.Stack_GetById_tr";
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@StackId", id);
        await TryOrDieAsync(async () =>
        {
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                output = new StackListItem
                {
                    Id = reader.GetInt32(0),
                    ViewName = reader.GetString(1),
                    Cards = reader.GetInt32(2)
                };
                if (!reader.IsDBNull(3))
                {
                    output.LastStudied = reader.GetDateTime(3);
                }
            }
        }, "get stack list item by id");

        connection.Close();

        return output ?? throw new ArgumentException($"No stack with ID {id} exists.");
    }

    public async Task<StackListItem?> GetStackListItemBySortNameAsync(string sortName)
    {
        StackListItem? output = null;

        var connection = new SqlConnection(_connectionString);
        await TryOrDieAsync(connection.OpenAsync, "get stack list item by sort name");

        var cmd = connection.CreateCommand();
        cmd.CommandText = "dbo.Stack_GetBySortName_tr";
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@SortName", sortName);
        await TryOrDieAsync(async () =>
        {
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                output = new StackListItem
                {
                    Id = reader.GetInt32(0),
                    ViewName = reader.GetString(1),
                    Cards = reader.GetInt32(2)
                };
                if (!reader.IsDBNull(3))
                {
                    output.LastStudied = reader.GetDateTime(3);
                }
            }
        }, "get stack list item by sort name");

        connection.Close();

        return output;
    }

    public async Task<StackListItem> GetStackListItemByFlashcardIdAsync(int flashcardId)
    {
        StackListItem? output = null;

        var connection = new SqlConnection(_connectionString);
        await TryOrDieAsync(connection.OpenAsync, "get stack list item by flashcard id");

        var cmd = connection.CreateCommand();
        cmd.CommandText = "dbo.Stack_GetByFlashcardId_tr";
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@FlashcardId", flashcardId);
        await TryOrDieAsync(async () =>
        {
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                output = new StackListItem
                {
                    Id = reader.GetInt32(0),
                    ViewName = reader.GetString(1),
                    Cards = reader.GetInt32(2)
                };
                if (!reader.IsDBNull(3))
                {
                    output.LastStudied = reader.GetDateTime(3);
                }
            }
        }, "get stack list item by flashcard id");

        connection.Close();

        return output ?? throw new ArgumentException($"No stack with flashcard ID {flashcardId} exists.");
    }

    public async Task CreateStackAsync(NewStack stack)
    {
        using var connection = new SqlConnection(_connectionString);
        await TryOrDieAsync(connection.OpenAsync, "create stack");

        var cmd = connection.CreateCommand();
        cmd.CommandText = "dbo.Stack_Create_tr";
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@ViewName", stack.ViewName);
        cmd.Parameters.AddWithValue("@SortName", stack.SortName);
        await TryOrDieAsync(cmd.ExecuteNonQueryAsync, "create stack");

        connection.Close();
    }

    public Task DeleteStackAsync(int stackId)
    {
        throw new NotImplementedException();
    }

    public Task RenameStackAsync(int oldStackId, NewStack updatedStack)
    {
        throw new NotImplementedException();
    }

    public async Task<int> CountFlashcardsAsync(int stackId)
    {
        var output = 0;

        using var connection = new SqlConnection(_connectionString);
        await TryOrDieAsync(connection.OpenAsync, "count flashcards in a stack");

        var cmd = connection.CreateCommand();
        cmd.CommandText = "dbo.Flashcard_Count_tr";
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@StackId", stackId);
        await TryOrDieAsync(async () =>
        {
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                output = reader.GetInt32(0);
            }
        }, "count flashcards in a stack");

        connection.Close();

        return output;
    }

    public Task<bool> CardInStack(int stackId, int flashcardId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<ExistingFlashcard>> GetFlashcardListAsync(int stackId, int? take = null, int skip = 0)
    {
        var output = new List<ExistingFlashcard>();

        using var connection = new SqlConnection(_connectionString);
        await TryOrDieAsync(connection.OpenAsync, "get flashcard list");

        var cmd = connection.CreateCommand();
        cmd.CommandText = "dbo.Flashcard_GetMultiple_tr";
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@StackId", stackId);
        cmd.Parameters.AddWithValue("@Take", take);
        cmd.Parameters.AddWithValue("@Skip", skip);
        await TryOrDieAsync(async () =>
        {
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                output.Add(new ExistingFlashcard
                {
                    Id = reader.GetInt32(0),
                    Front = reader.GetString(1),
                    Back = reader.GetString(2)
                });
            }
        }, "get flashcard list");

        connection.Close();

        return output;
    }

    public async Task<ExistingFlashcard> GetFlashcardByIdAsync(int id)
    {
        ExistingFlashcard? output = null;
        using var connection = new SqlConnection(_connectionString);
        TryOrDie(connection.Open, "get flashcard by id");

        var cmd = connection.CreateCommand();
        cmd.CommandText = "dbo.Flashcard_GetById_tr";
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@FlashcardId", id);
        await TryOrDieAsync(async () =>
        {
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                output = new ExistingFlashcard
                {
                    Id = reader.GetInt32(0),
                    Front = reader.GetString(1),
                    Back = reader.GetString(2)
                };
            }
        }, "get flashcard by id");

        connection.Close();

        return output ?? throw new ArgumentException($"No flashcard with ID {id} exists.");
    }

    public async Task CreateFlashcardAsync(NewFlashcard flashcard)
    {
        using var connection = new SqlConnection(_connectionString);
        await TryOrDieAsync(connection.OpenAsync, "create flashcard");

        var cmd = connection.CreateCommand();
        cmd.CommandText = "dbo.Flashcard_Create_tr";
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@StackId", flashcard.StackId);
        cmd.Parameters.AddWithValue("@Front", flashcard.Front);
        cmd.Parameters.AddWithValue("@Back", flashcard.Back);
        await TryOrDieAsync(cmd.ExecuteNonQueryAsync, "create flashcard");

        connection.Close();
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

    private static void TryOrDie(Action action, string purpose, string? formatError = null)
    {
        if (formatError == null)
        {
            try
            {
                action.Invoke();
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Failed to {purpose}: {ex.Message}\nAborting!");
                Environment.Exit(1);
            }
        }
        else
        {
            try
            {
                action.Invoke();
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Failed to {purpose}: {ex.Message}\nAborting!");
                Environment.Exit(1);
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"{formatError}: {ex.Message}\nAborting!");
                Environment.Exit(1);
            }
        }
    }

    private static async Task TryOrDieAsync(Func<Task> func, string purpose, string? formatError = null)
    {
        if (formatError == null)
        {
            try
            {
                await func.Invoke();
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Failed to {purpose}: {ex.Message}\nAborting!");
                Environment.Exit(1);
            }
        }
        else
        {
            try
            {
                await func.Invoke();
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Failed to {purpose}: {ex.Message}\nAborting!");
                Environment.Exit(1);
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"{formatError}: {ex.Message}\nAborting!");
                Environment.Exit(1);
            }
        }
    }
}

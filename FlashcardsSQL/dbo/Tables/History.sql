CREATE TABLE [dbo].[History]
(
	[HistoryId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [StackId] INT NOT NULL, 
    [StartedAt] DATETIME2 NOT NULL, 
    CONSTRAINT [History_PertainsTo_Stack_fk] FOREIGN KEY ([StackId]) REFERENCES [Stack]([StackId]) ON DELETE CASCADE
)

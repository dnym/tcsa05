CREATE PROCEDURE [dbo].[History_Create_tr]
	@StartedAt datetime2(7),
	@StackId int
AS
	BEGIN
		SET NOCOUNT ON;
		INSERT INTO [dbo].[History] ([StartedAt], [StackId])
		VALUES (@StartedAt, @StackId);
	END
RETURN 0

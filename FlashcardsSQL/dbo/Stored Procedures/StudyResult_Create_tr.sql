CREATE PROCEDURE [dbo].[StudyResult_Create_tr]
	@FlashcardId int,
	@WasCorrect bit,
	@AnsweredAt datetime2(7)
AS
	BEGIN
		SET NOCOUNT ON;
		INSERT INTO [dbo].[StudyResult] ([FlashcardId], [WasCorrect], [AnsweredAt])
		VALUES (@FlashcardId, @WasCorrect, @AnsweredAt);
	END
RETURN 0

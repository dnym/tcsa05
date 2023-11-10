CREATE PROCEDURE [dbo].[Flashcard_GetMultiple_tr]
    @stackId INT,
    @skip INT = NULL,
    @take INT = NULL
AS
    BEGIN
        SET NOCOUNT ON;

        DECLARE @SKIP_FALLBACK INT;
        SET @SKIP_FALLBACK = 0;
        DECLARE @TAKE_FALLBACK INT;
        SET @TAKE_FALLBACK = 2147483647;

        SELECT F.FlashcardId, F.Front, F.Back
        FROM Flashcard AS F
        WHERE F.StackId = @stackId
        ORDER BY F.FlashcardId
        OFFSET ISNULL(@skip, @SKIP_FALLBACK) ROWS
        FETCH NEXT ISNULL(@take, @TAKE_FALLBACK) ROWS ONLY;
    END
RETURN 0

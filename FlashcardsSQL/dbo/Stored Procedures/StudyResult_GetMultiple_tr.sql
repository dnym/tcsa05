CREATE PROCEDURE [dbo].[StudyResult_GetMultiple_tr]
    @historyId INT,
    @skip INT = NULL,
    @take INT = NULL
AS
    BEGIN
        SET NOCOUNT ON;

        DECLARE @SKIP_FALLBACK INT;
        SET @SKIP_FALLBACK = 0;
        DECLARE @TAKE_FALLBACK INT;
        SET @TAKE_FALLBACK = 2147483647;

        SELECT
            ROW_NUMBER() OVER (ORDER BY StudyResultId) AS Ordinal,
            F.Front,
            R.AnsweredAt, R.WasCorrect
        FROM StudyResult AS R
        INNER JOIN Flashcard AS F ON R.FlashcardId = F.FlashcardId
        WHERE R.HistoryId = @historyId
        ORDER BY R.StudyResultId
        OFFSET ISNULL(@skip, @SKIP_FALLBACK) ROWS
        FETCH NEXT ISNULL(@take, @TAKE_FALLBACK) ROWS ONLY;
    END
RETURN 0

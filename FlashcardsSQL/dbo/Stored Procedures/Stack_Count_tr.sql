﻿CREATE PROCEDURE [dbo].[Stack_Count_tr]
    @Skip INT = NULL,
    @Take INT = NULL
AS
	BEGIN
		SET NOCOUNT ON;

        DECLARE @SKIP_FALLBACK INT;
        SET @SKIP_FALLBACK = 0;
        DECLARE @TAKE_FALLBACK INT;
        SET @TAKE_FALLBACK = 2147483647;

		SELECT
			COUNT(S.StackId) as Stacks
		FROM Stack AS S;
	END
RETURN 0
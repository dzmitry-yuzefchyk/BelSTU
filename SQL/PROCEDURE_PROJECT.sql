USE Taskboard;

GO
CREATE OR ALTER PROCEDURE [Project.Create]
	@userId INT,
	@title NVARCHAR(64),
	@about NVARCHAR(256),
	@teamId INT
AS
BEGIN
	IF NOT EXISTS (SELECT TOP 1 id FROM [USER_TOKEN] WHERE userId = @userId)
	BEGIN
		PRINT 'TOKEN EXPIRED';
		RETURN;
	END;

	IF NOT EXISTS (SELECT TOP 1 id FROM TEAM WHERE id = @teamId)
	BEGIN
		PRINT 'TEAM NOT EXISTS';
		RETURN;
	END;

	IF (SELECT TOP 1 "role" FROM [TEAM_USER] WHERE userId = @userId AND id = @teamId) != 'CREATOR'
	BEGIN
		PRINT 'ACCESS DENIED OR SUCH USER NOT EXISTS';
		RETURN;
	END;

	BEGIN TRY
		INSERT INTO PROJECT(title, about, teamId)
			VALUES(@title, @about, @teamId);
	END TRY

	BEGIN CATCH
		SELECT
			ERROR_LINE() AS ErrorLine,
			ERROR_MESSAGE() AS ErrorMessage;
	END CATCH
END;

GO
CREATE OR ALTER PROCEDURE [Project.Delete]
	@userId INT,
	@projectId INT,
	@teamId INT
AS
BEGIN
	DECLARE boardCursor CURSOR FOR
		SELECT id FROM BOARD
			WHERE projectId = @projectId;
	DECLARE @boardToDelete INT;

	IF NOT EXISTS (SELECT TOP 1 id FROM [USER_TOKEN] WHERE userId = @userId)
	BEGIN
		PRINT 'TOKEN EXPIRED';
		RETURN;
	END;

	IF NOT EXISTS (SELECT TOP 1 id FROM PROJECT WHERE id = @projectId)
	BEGIN
		PRINT 'PROJECT NOT EXISTS';
		RETURN;
	END;

	IF (SELECT TOP 1 "role" FROM [TEAM_USER] WHERE userId = @userId AND id = @teamId) != 'CREATOR'
	BEGIN
		PRINT 'ACCESS DENIED OR SUCH USER NOT EXISTS';
		RETURN;
	END;

	BEGIN TRY
		OPEN boardCursor;

		WHILE @@FETCH_STATUS = 0
		BEGIN
			FETCH boardCursor INTO @boardToDelete;

			EXEC [Board.Delete]
				@userId = @userId,
				@boardId = @boardToDelete,
				@teamId = @teamId;
		END;

		DELETE PROJECT WHERE id = @projectId;
		CLOSE boardCursor;
	END TRY

	BEGIN CATCH
		SELECT
			ERROR_LINE() AS ErrorLine,
			ERROR_MESSAGE() AS ErrorMessage;
	END CATCH

	DEALLOCATE boardCursor;
END;

GO
CREATE OR ALTER PROCEDURE [Project.Alter]
	@userId INT,
	@projectId INT,
	@title NVARCHAR(64),
	@about NVARCHAR(256),
	@teamId INT
AS
BEGIN
	IF NOT EXISTS (SELECT TOP 1 id FROM [USER_TOKEN] WHERE userId = @userId)
	BEGIN
		PRINT 'TOKEN EXPIRED';
		RETURN;
	END;

	IF NOT EXISTS (SELECT TOP 1 id FROM PROJECT WHERE id = @projectId)
	BEGIN
		PRINT 'PROJECT NOT EXISTS';
		RETURN;
	END;

	IF (SELECT TOP 1 "role" FROM [TEAM_USER] WHERE userId = @userId AND id = @teamId) != 'CREATOR'
	BEGIN
		PRINT 'ACCESS DENIED OR SUCH USER NOT EXISTS';
		RETURN;
	END;

	BEGIN TRY
		UPDATE PROJECT SET about = @about, title = @title WHERE id = @projectId;
	END TRY

	BEGIN CATCH
		SELECT
			ERROR_LINE() AS ErrorLine,
			ERROR_MESSAGE() AS ErrorMessage;
	END CATCH
END;

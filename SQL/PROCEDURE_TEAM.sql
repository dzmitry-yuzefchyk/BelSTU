USE Taskboard;

GO
CREATE OR ALTER PROCEDURE [Team.Create]
	@userId INT,
	@teamName NVARCHAR(64)
AS
BEGIN
	DECLARE @teamId INT;

	IF NOT EXISTS (SELECT TOP 1 id FROM [USER_TOKEN] WHERE userId = @userId)
	BEGIN
		PRINT 'TOKEN EXPIRED';
		RETURN;
	END;

	BEGIN TRY
		INSERT INTO [TEAM](title)
			VALUES(@teamName);

		SET @teamId = SCOPE_IDENTITY();

		INSERT INTO [TEAM_USER](userId, teamId, "role")
			VALUES(@userId, @teamId, 'CREATOR');
	END TRY

	BEGIN CATCH
		SELECT
			ERROR_LINE() AS ErrorLine,
			ERROR_MESSAGE() AS ErrorMessage;
	END CATCH

END;
GO
CREATE OR ALTER PROCEDURE [Team.AddUserToTeam]
	@creatorId INT,
	@teamId INT,
	@userEmail NVARCHAR(256)
AS
BEGIN
	DECLARE @addUserId INT = (SELECT TOP 1 id FROM [USER] WHERE email = @userEmail); 

	IF NOT EXISTS (SELECT TOP 1 id FROM [USER_TOKEN] WHERE userId = @creatorId)
	BEGIN
	 	PRINT 'TOKEN EXPIRED';
		RETURN;
	END;

	IF NOT EXISTS (SELECT TOP 1 id FROM TEAM WHERE id = @teamId)
	BEGIN
		PRINT 'TEAM NOT EXISTS';
		RETURN;
	END;

	IF (SELECT TOP 1 "role" FROM [TEAM_USER] WHERE userId = @creatorId AND teamId = @teamId) != 'CREATOR'
		AND @addUserId IS NULL
	BEGIN
		PRINT 'ACCESS DENIED OR SUCH USER NOT EXISTS';
		RETURN;
	END;

	IF EXISTS (SELECT TOP 1 userId FROM [TEAM_USER] WHERE userId = @addUserId AND teamId = @teamId)
	BEGIN
		PRINT 'USER ALREADY IN TEAM';
		RETURN;
	END;

	BEGIN TRY
		INSERT INTO [TEAM_USER](userId, teamId, "role")
			VALUES(@addUserId, @teamId, 'USER');
	END TRY

	BEGIN CATCH
		SELECT
			ERROR_LINE() AS ErrorLine,
			ERROR_MESSAGE() AS ErrorMessage;
	END CATCH

END;

GO
CREATE OR ALTER PROCEDURE [Team.RemoveUserFromTeam]
	@creatorId NVARCHAR(256),
	@teamId INT,
	@userEmail NVARCHAR(256)
AS
BEGIN
	DECLARE @userId INT = (SELECT TOP 1 id FROM [USER] WHERE email = @userEmail); 

	IF NOT EXISTS (SELECT TOP 1 id FROM [USER_TOKEN] WHERE userId = @creatorId)
	BEGIN
	 	PRINT 'TOKEN EXPIRED';
		RETURN;
	END;

	IF NOT EXISTS (SELECT TOP 1 id FROM TEAM WHERE id = @teamId)
	BEGIN
		PRINT 'TEAM NOT EXISTS';
		RETURN;
	END;


	IF (SELECT TOP 1 "role" FROM [TEAM_USER] WHERE userId = @creatorId AND teamId = @teamId) != 'CREATOR'
		AND @userId IS NULL
	BEGIN
		PRINT 'ACCESS DENIED OR SUCH USER NOT EXISTS';
		RETURN;
	END;

	IF (SELECT TOP 1 "role" FROM [TEAM_USER] WHERE userId = @userId AND id = @teamId) = 'CREATOR'
	BEGIN
		PRINT 'YOU CANT REMOVE CREATOR FROM TEAM';
		RETURN;
	END;

	IF NOT EXISTS (SELECT TOP 1 userId FROM [TEAM_USER] WHERE userId = @userId AND teamId = @teamId)
	BEGIN
		PRINT 'THERE IS NO SUCH USER';
		RETURN;
	END;

	BEGIN TRY
		DELETE [TEAM_USER] WHERE userId = @userId;
	END TRY

	BEGIN CATCH
		SELECT
			ERROR_LINE() AS ErrorLine,
			ERROR_MESSAGE() AS ErrorMessage;
	END CATCH
END;

GO
CREATE OR ALTER PROCEDURE [Team.Delete]
	@creatorId INT,
	@teamId INT
AS
BEGIN
	DECLARE projectCursor CURSOR FOR
		SELECT id FROM PROJECT 
			WHERE teamId = @teamId;
	DECLARE @teamUserId INT,
			@projectIdToDelete INT;

	IF NOT EXISTS (SELECT TOP 1 id FROM [USER_TOKEN] WHERE userId = @creatorId)
	BEGIN
	 	PRINT 'TOKEN EXPIRED';
		RETURN;
	END;

	IF (SELECT TOP 1 "role" FROM [TEAM_USER] WHERE userId = @creatorId AND teamId = @teamId) != 'CREATOR'
	BEGIN
		PRINT 'ACCESS DENIED OR SUCH USER NOT EXISTS';
		RETURN;
	END;

	BEGIN TRY
		OPEN projectCursor;

		DELETE TEAM_USER WHERE teamId = @teamId;

		WHILE @@FETCH_STATUS = 0
		BEGIN
			FETCH projectCursor INTO @projectIdToDelete;
					
			EXEC [Project.Delete]
				@userId = @creatorId,
				@teamId = @teamId,
				@projectId = @projectIdToDelete;
		END;

		DELETE [TEAM] WHERE id = @teamId;

		PRINT 'TEAM: ' + CAST(@teamId AS NVARCHAR(256)) + ' DELETED';
		CLOSE projectCursor;
	END TRY

	BEGIN CATCH
		SELECT
			ERROR_LINE() AS ErrorLine,
			ERROR_MESSAGE() AS ErrorMessage;
	END CATCH

	DEALLOCATE projectCursor;
END;
GO
CREATE OR ALTER PROCEDURE [Team.GetUserTeams]
	@userId INT,
	@skip INT,
	@take INT
AS
BEGIN
	SELECT teamId, title, "role" FROM [TEAM]
		INNER JOIN [TEAM_USER]
	ON [TEAM].id = [TEAM_USER].teamId
		WHERE [TEAM_USER].userId = @userId
		ORDER BY title
		OFFSET @skip ROWS
		FETCH NEXT @take ROWS ONLY;
END;

GO
CREATE OR ALTER PROCEDURE [Team.GetTeamUsers]
	@teamId INT,
	@skip INT,
	@take INT
AS
BEGIN
	SELECT email, "role" FROM [USER]
		INNER JOIN [TEAM_USER]
	ON [USER].id = [TEAM_USER].userId
		WHERE [TEAM_USER].teamId = @teamId
		ORDER BY email
		OFFSET @skip ROWS
		FETCH NEXT @take ROWS ONLY;
END;
USE Taskboard;

GO
CREATE OR ALTER PROCEDURE [Team.CreateTeam]
	@userEmail NVARCHAR(64),
	@teamName NVARCHAR(64)
AS
BEGIN
	DECLARE @teamId INT,
			@userId INT = (SELECT TOP 1 id FROM [USER] WHERE email = @userEmail);
	IF EXISTS (SELECT TOP 1 id FROM [USER_TOKEN] WHERE userId = @userId)
		BEGIN TRY
			INSERT INTO [TEAM](title)
				VALUES(@teamname);

			SET @teamId = SCOPE_IDENTITY();

			INSERT INTO [TEAM_USER](userId, teamId, "role")
				VALUES(@userId, @teamId, 'CREATOR');

			RETURN(0);
		END TRY

		BEGIN CATCH
			SELECT
				ERROR_LINE() AS ErrorLine,
				ERROR_MESSAGE() AS ErrorMessage;
		END CATCH
	ELSE 
		PRINT 'TOKEN EXPIRED'
	RETURN(1);
END;
GO
CREATE OR ALTER PROCEDURE [Team.AddUserToTeam]
	@creatorEmail NVARCHAR(256),
	@teamId INT,
	@addUserEmail NVARCHAR(256)
AS
BEGIN
	DECLARE @creatorId INT = (SELECT TOP 1 id FROM [USER] WHERE email = @creatorEmail),
			@addUserId INT = (SELECT TOP 1 id FROM [USER] WHERE email = @addUserEmail); 
	IF EXISTS (SELECT TOP 1 id FROM [USER_TOKEN] WHERE userId = @creatorId)

		IF (SELECT TOP 1 "role" FROM [TEAM_USER] WHERE userId = @creatorId AND id = @teamId) = 'CREATOR'
			AND @addUserId IS NOT NULL
			
			IF NOT EXISTS (SELECT TOP 1 userId FROM [TEAM_USER] WHERE userId = @addUserId AND teamId = @teamId)
				BEGIN
					BEGIN TRY
						INSERT INTO [TEAM_USER](userId, teamId, "role")
							VALUES(@addUserId, @teamId, 'USER');
						RETURN(0);
					END TRY

					BEGIN CATCH
						SELECT
							ERROR_LINE() AS ErrorLine,
							ERROR_MESSAGE() AS ErrorMessage;
							RETURN(1);
					END CATCH
				END

				ELSE 
					PRINT 'USER ALREADY IN TEAM';

			ELSE 
				PRINT 'ACCESS DENIED OR SUCH USER NOT EXISTS';

	ELSE 
		PRINT 'TOKEN EXPIRED';

	RETURN(1);
END;

GO
CREATE OR ALTER PROCEDURE [Team.DeleteTeam]
	@creatorEmail NVARCHAR(256),
	@teamId INT
AS
BEGIN
	DECLARE @creatorId INT = (SELECT TOP 1 id FROM [USER] WHERE email = @creatorEmail),
			@teamUserId INT;
	DECLARE	teamUserCursor CURSOR FOR 
				SELECT id FROM TEAM_USER
				WHERE teamId = @teamId;

	IF EXISTS (SELECT TOP 1 id FROM [USER_TOKEN] WHERE userId = @creatorId)

		IF (SELECT TOP 1 "role" FROM [TEAM_USER] WHERE userId = @creatorId AND id = @teamId) = 'CREATOR'

		BEGIN

			OPEN teamUserCursor;
			BEGIN TRY
				WHILE @@FETCH_STATUS = 0
				BEGIN
					FETCH NEXT FROM teamUserCursor INTO @teamUserId;
					DELETE TEAM_USER WHERE id = @teamUserId;
				END

				DELETE [TEAM] WHERE id = @teamId;
				CLOSE teamUserCursor;
				DEALLOCATE teamUserCursor;
				RETURN(0);
			END TRY

			BEGIN CATCH
				SELECT
					ERROR_LINE() AS ErrorLine,
					ERROR_MESSAGE() AS ErrorMessage;
					RETURN(1);
			END CATCH

		END;

		ELSE 
			PRINT 'ACCESS DENIED OR SUCH USER NOT EXISTS';

	ELSE 
		PRINT 'TOKEN EXPIRED';

	RETURN(1);
	DEALLOCATE teamUserCursor;
END;
GO
CREATE OR ALTER PROCEDURE [Team.GetUserTeams]
	@userEmail NVARCHAR(256),
	@skip INT,
	@amount INT
AS
BEGIN
	DECLARE @userId INT = (SELECT TOP 1 id FROM [USER] WHERE email = @userEmail);
	SELECT title FROM [TEAM]
		INNER JOIN [TEAM_USER]
	ON [TEAM].id = [TEAM_USER].teamId
		WHERE [TEAM_USER].userId = @userId
		ORDER BY title
		OFFSET @skip ROWS
		FETCH NEXT @amount ROWS ONLY;
		
END;

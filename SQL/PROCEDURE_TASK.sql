USE Taskboard;

GO
CREATE OR ALTER PROCEDURE [Task.Create]
	@userId INT,
	@teamId INT,
	@boardId INT,
	@title NVARCHAR(64),
	@content NVARCHAR(1024),
	@type NVARCHAR(11),
	@priority NVARCHAR(6),
	@severity NVARCHAR(8),
	@status NVARCHAR(128),
	@asigneeEmail NVARCHAR(256),
	@finishTime DATETIME,
	@filePath VARCHAR(256)
AS
BEGIN
	DECLARE @asigneeId INT = (SELECT TOP 1 id FROM [USER] WHERE email = @asigneeEmail);

	IF NOT EXISTS (SELECT TOP 1 id FROM [USER_TOKEN] WHERE userId = @userId)
	BEGIN
		PRINT 'TOKEN EXPIRED';
		RETURN;
	END;

	IF NOT EXISTS (SELECT TOP 1 id FROM BOARD WHERE id = @boardId)
	BEGIN
		PRINT 'BOARD NOT EXISTS';
		RETURN;
	END;

	IF NOT EXISTS (SELECT TOP 1 id FROM [TEAM_USER] WHERE userId = @userId AND teamId = @teamId)
	BEGIN
		PRINT 'ACCESS DENIED OR SUCH USER NOT EXISTS';
		RETURN;
	END;

	IF NOT EXISTS (SELECT TOP 1 id FROM [TEAM_USER] WHERE userId = @asigneeId AND teamId = @teamId)
	BEGIN
		PRINT 'USER NOT IN TEAM';
		RETURN;
	END;

	BEGIN TRY


		IF (@filePath = '')
		BEGIN
		INSERT INTO TASKS(boardId, title, content, [type], [priority], severity, asigneeId, finishTime, startTime, "status")
				VALUES(@boardId, @title, @content, @type, @priority, @severity, @asigneeId, @finishTime, CURRENT_TIMESTAMP, @status);
		END;

		ELSE 
		BEGIN
			DECLARE @file_stream VARBINARY(MAX);
			DECLARE @command NVARCHAR(1000);
			DECLARE @documentId INT;
			DECLARE @documentExtenstion NVARCHAR(16) = (select reverse(left(reverse(@filePath),CHARINDEX('.',reverse(@filePath))-1)));
			DECLARE @documentName NVARCHAR(128) = REVERSE(LEFT(REVERSE(@filePath),CHARINDEX( '\',REVERSE(@filePath))-1));

			SET @command = N'SELECT @file_stream1 = CAST(bulkcolumn AS varbinary(MAX))
						from OPENROWSET(BULK ''' + @filePath + ''',
						SINGLE_BLOB) ROW_SET'

			EXEC sp_executesql @command, N'@file_stream1 VARBINARY(MAX) OUTPUT', @file_stream1 = @file_stream OUTPUT;
			select @file_stream;


			INSERT INTO DOCUMENT("name", extension, document)
				VALUES(@documentName, @documentExtenstion, @file_stream);

			SET @documentId = SCOPE_IDENTITY();

			INSERT INTO TASKS(boardId, title, content, attachments, [type], [priority], severity, asigneeId, finishTime, startTime, "status")
				VALUES(@boardId, @title, @content, @documentId, @type, @priority, @severity, @asigneeId, @finishTime, CURRENT_TIMESTAMP, @status);
		END;
	END TRY

	BEGIN CATCH
		SELECT
			ERROR_LINE() AS ErrorLine,
			ERROR_MESSAGE() AS ErrorMessage;
	END CATCH
END;
GO
CREATE OR ALTER PROCEDURE [Task.Delete]
	@userId INT,
	@teamId INT,
	@taskId INT
AS
BEGIN
	DECLARE @documentId INT = (SELECT TOP 1 id FROM DOCUMENT WHERE id = (SELECT attachments FROM TASKS WHERE id = @taskId));

	IF NOT EXISTS (SELECT TOP 1 id FROM [USER_TOKEN] WHERE userId = @userId)
	BEGIN
		PRINT 'TOKEN EXPIRED';
		RETURN;
	END;

	IF NOT EXISTS (SELECT TOP 1 id FROM TASKS WHERE id = @taskId)
	BEGIN
		PRINT 'TASK NOT EXISTS';
		RETURN;
	END;

	IF NOT EXISTS (SELECT TOP 1 id FROM [TEAM_USER] WHERE userId = @userId AND teamId = @teamId)
	BEGIN
		PRINT 'ACCESS DENIED OR SUCH USER NOT EXISTS';
		RETURN;
	END;

	BEGIN TRY
		DELETE TASKS WHERE id = @taskId;
		DELETE DOCUMENT WHERE id = @documentId;
	END TRY

	BEGIN CATCH
		SELECT
			ERROR_LINE() AS ErrorLine,
			ERROR_MESSAGE() AS ErrorMessage;
	END CATCH
END;
GO
CREATE OR ALTER PROCEDURE [Task.Get]
	@userId INT,
	@teamId INT,
	@boardId INT,
	@skip INT,
	@take INT
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

	IF NOT EXISTS (SELECT TOP 1 id FROM BOARD WHERE id = @boardId)
	BEGIN
		PRINT 'BOARD NOT EXISTS';
		RETURN;
	END;

	IF NOT EXISTS (SELECT TOP 1 id FROM [TEAM_USER] WHERE userId = @userId AND teamId = @teamId)
	BEGIN
		PRINT 'ACCESS DENIED OR SUCH USER NOT EXISTS';
		RETURN;
	END;

	BEGIN TRY
		SELECT TASKS.id, title, email, content, attachments, "type", "priority", "severity", "status" FROM TASKS
		INNER JOIN [USER] ON TASKS.asigneeId = [USER].id
			WHERE boardId = @boardId
			ORDER BY TASKS.id
			OFFSET @skip ROWS
			FETCH NEXT @take ROWS ONLY;
	END TRY

	BEGIN CATCH
		SELECT
			ERROR_LINE() AS ErrorLine,
			ERROR_MESSAGE() AS ErrorMessage;
	END CATCH
END;
GO
CREATE OR ALTER PROCEDURE [Task.Search]
	@userId INT,
	@teamId INT,
	@boardId INT,
	@searchQuery NVARCHAR(256),
	@skip INT,
	@take INT
AS
BEGIN
	IF NOT EXISTS (SELECT TOP 1 id FROM [USER_TOKEN] WHERE userId = @userId)
	BEGIN
		PRINT 'TOKEN EXPIRED';
		RETURN;
	END;

	IF NOT EXISTS (SELECT TOP 1 id FROM BOARD WHERE id = @boardId)
	BEGIN
		PRINT 'BOARD NOT EXISTS';
		RETURN;
	END;

	IF NOT EXISTS (SELECT TOP 1 id FROM [TEAM_USER] WHERE userId = @userId AND teamId = @teamId)
	BEGIN
		PRINT 'ACCESS DENIED OR SUCH USER NOT EXISTS';
		RETURN;
	END;

	SELECT TASKS.id, title, email, content, attachments, "type", "priority", "severity", "status" FROM TASKS
	INNER JOIN DOCUMENT ON TASKS.attachments = DOCUMENT.id
	INNER JOIN [USER] ON TASKS.asigneeId = [USER].id
		WHERE TASKS.boardId = @boardId AND FREETEXT(DOCUMENT.document, @searchQuery)
		ORDER BY TASKS.id
		OFFSET @skip ROWS
		FETCH NEXT @take ROWS ONLY;;
END;
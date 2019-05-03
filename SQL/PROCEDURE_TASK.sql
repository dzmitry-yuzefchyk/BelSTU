USE Taskboard;

GO
CREATE OR ALTER PROCEDURE [Task.Create]
	@userId INT,
	@teamId INT,
	@boardId INT,
	@title NVARCHAR(64),
	@content NVARCHAR(1024),
	@attachments VARBINARY(3096),
	@type NVARCHAR(11),
	@priority NVARCHAR(6),
	@severity NVARCHAR(8),
	@asigneeId INT,
	@timeToComplete TIME,
	@startTime DATETIME
AS
BEGIN
	IF NOT EXISTS (SELECT TOP 1 id FROM [USER_TOKEN] WHERE userId = @userId)
	BEGIN
		PRINT 'TOKEN EXPIRED';
		RETURN;
	END;

	IF NOT EXISTS (SELECT TOP 1 id FROM PROJECT WHERE id = @boardId)
	BEGIN
		PRINT 'BOARD NOT EXISTS';
		RETURN;
	END;

	IF NOT EXISTS (SELECT TOP 1 id FROM [TEAM_USER] WHERE userId = @userId AND id = @teamId)
	BEGIN
		PRINT 'ACCESS DENIED OR SUCH USER NOT EXISTS';
		RETURN;
	END;

	BEGIN TRY
		INSERT INTO TASKS(boardId, title, content, attachments, [type], [priority], severity, asigneeId, timeToComplete, startTime)
			VALUES(@boardId, @title, @content, @attachments, @type, @priority, @severity, @asigneeId, @timeToComplete, @startTime);
	END TRY

	BEGIN CATCH
		SELECT
			ERROR_LINE() AS ErrorLine,
			ERROR_MESSAGE() AS ErrorMessage;
	END CATCH
END;
GO
CREATE OR ALTER PROCEDURE [Task.Alter]
	@userId INT,
	@teamId INT,
	@boardId INT,
	@title NVARCHAR(64),
	@content NVARCHAR(1024),
	@attachments VARBINARY(3096),
	@type NVARCHAR(11),
	@priority NVARCHAR(6),
	@severity NVARCHAR(8),
	@asigneeId INT,
	@timeToComplete TIME,
	@startTime DATETIME
AS
BEGIN
	IF NOT EXISTS (SELECT TOP 1 id FROM [USER_TOKEN] WHERE userId = @userId)
	BEGIN
		PRINT 'TOKEN EXPIRED';
		RETURN;
	END;

	IF NOT EXISTS (SELECT TOP 1 id FROM PROJECT WHERE id = @boardId)
	BEGIN
		PRINT 'BOARD NOT EXISTS';
		RETURN;
	END;

	IF NOT EXISTS (SELECT TOP 1 id FROM [TEAM_USER] WHERE userId = @userId AND id = @teamId)
	BEGIN
		PRINT 'ACCESS DENIED OR SUCH USER NOT EXISTS';
		RETURN;
	END;

	BEGIN TRY
		UPDATE TASKS SET title = @title, content = @content, attachments = @attachments,
			[type] = @type, [priority] = @priority, severity = @severity, asigneeId = @asigneeId,
			timeToComplete = @timeToComplete, startTime = @startTime
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

	IF NOT EXISTS (SELECT TOP 1 id FROM [TEAM_USER] WHERE userId = @userId AND id = @teamId)
	BEGIN
		PRINT 'ACCESS DENIED OR SUCH USER NOT EXISTS';
		RETURN;
	END;

	BEGIN TRY
		 DELETE TASKS WHERE id = @taskId;
	END TRY

	BEGIN CATCH
		SELECT
			ERROR_LINE() AS ErrorLine,
			ERROR_MESSAGE() AS ErrorMessage;
	END CATCH
END;
GO;
CREATE OR ALTER PROCEDURE [Task.Search]
	@userId INT,
	@teamId INT,
	@searchQuery NVARCHAR(256)
AS
BEGIN
	
END;

USE Taskboard;

DECLARE
	@userId INT,
	@teamId INT,
	@mail NVARCHAR(256) = 'd111.yuzefchik@gmail.com',
	@pwd NVARCHAR(256) = '12345',
	@userName NVARCHAR(256) = 'Dmitry',
	@teamTitle NVARCHAR(256) = 'Team1 Sfff';

BEGIN
	PRINT 'User.Register' 
	SET NOCOUNT ON;
	SET STATISTICS TIME ON;

	EXEC [User.Register]
		@email = @mail,
		@password = @pwd,
		@name = @userName;
	SET STATISTICS TIME OFF;
	PRINT '-----------------------------------';
END

BEGIN
	SET @userId = (SELECT TOP 1 id FROM [USER] WHERE email = @mail); 
	PRINT 'User.Login' 
	SET NOCOUNT ON;
	SET STATISTICS TIME ON;

	EXEC [User.Login]
		@email = @mail,
		@password = @pwd;
	SET STATISTICS TIME OFF;
	PRINT '-----------------------------------';
END

BEGIN
	SET @userId = (SELECT TOP 1 id FROM [USER] WHERE email = @mail);
	PRINT 'Team.Create' 
	SET NOCOUNT ON;
	SET STATISTICS TIME ON;

	EXEC [Team.Create]
		@userId = @userId,
		@teamName = @teamTitle;

	SET STATISTICS TIME OFF;
	PRINT '-----------------------------------';
END

BEGIN
	SET @teamId = (SELECT TOP 1 id FROM TEAM WHERE title = @teamTitle);
	SET @userId = (SELECT TOP 1 id FROM [USER] WHERE email = @mail); 
	PRINT 'Project.Create';
	SET NOCOUNT ON;
	SET STATISTICS TIME ON;
	
	EXEC [Project.Create]
		@userId = @userId,
		@teamId = @teamId,
		@title = 'f',
		@about = 'fff';

	SET STATISTICS TIME OFF;
	PRINT '-----------------------------------';
END;

BEGIN
	SET @teamId = (SELECT TOP 1 id FROM TEAM WHERE title = @teamTitle);
	SET @userId = (SELECT TOP 1 id FROM [USER] WHERE email = @mail); 
	PRINT 'Project.Get';
	SET NOCOUNT ON;
	SET STATISTICS TIME ON;
	
	EXEC [Project.Get]
		@userId = @userId,
		@teamId = @teamId,
		@skip = 0,
		@take = 10;

	SET STATISTICS TIME OFF;
	PRINT '-----------------------------------';
END;

BEGIN
	SET @teamId = (SELECT TOP 1 id FROM TEAM WHERE title = @teamTitle);
	SET @userId = (SELECT TOP 1 id FROM [USER] WHERE email = @mail); 
	PRINT 'Team.Delete';
	SET NOCOUNT ON;
	SET STATISTICS TIME ON;
	
	EXEC [Team.Delete]
		@creatorId = 12,
		@teamId = 8;
		
	SET STATISTICS TIME OFF;
	PRINT '-----------------------------------';
END;
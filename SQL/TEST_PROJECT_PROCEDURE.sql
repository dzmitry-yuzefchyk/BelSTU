--USE Taskboard;

DECLARE
	@userId INT,
	@mail NVARCHAR(256) = 'd.yuzefchik@gmail.com',
	@pwd NVARCHAR(256) = '12345'

BEGIN
	SET NOCOUNT ON;

	EXEC [User.Login]
		@email = @mail,
		@password = @pwd;
	
	SET @userId = (SELECT id FROM [USER] WHERE email = @mail); 

	--EXEC [Team.Create]
	--	@userId = @userId,
	--	@teamName = 'fdsf';

	SELECT * FROM TEAM;

	EXEC [Project.Create]
		@userId = @userId,
		@teamId = 33,
		@title = 'f',
		@about = 'fff';

	SELECT * FROM PROJECT;

END
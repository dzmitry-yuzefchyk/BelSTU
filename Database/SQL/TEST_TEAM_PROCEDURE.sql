USE Taskboard;

BEGIN
	SET NOCOUNT ON;

	EXEC [User.Register]
		@email = 'kate.yuzefchik@gmail.com',
		@password = '12345',
		@name = 'kate';

	EXEC [User.Register]
		@email = 'dmitry.yuzefchik@gmail.com',
		@password = '12345',
		@name = 'dmitry';

	SELECT * FROM [USER];

	EXEC [User.Login]
		@email = 'dmitry.yuzefchik@gmail.com',
		@password = '12345';

	EXEC [Team.Create]
		@userId = 1,
		@teamName = 'SuperTeam';

	SELECT * FROM [USER];
	SELECT * FROM [TEAM];
	SELECT * FROM [TEAM_USER];
	SELECT * FROM [USER_TOKEN];

	EXEC [Team.AddUserToTeam]
		@creatorId = 3,
		@teamId = 1,
		@userEmail = 'kate.yuzefchik@gmail.com';
	
	EXEC [Team.AddUserToTeam]
		@creatorId = 2,
		@teamId = 1,
		@userEmail = 'kate.yuzefchik@gmail.com';

	EXEC [Team.GetUserTeams]
		@userId = 1,
		@skip = 0,
		@take = 2; 

	EXEC [Team.Delete]
		@creatorId = 3,
		@teamId = 3;

	EXEC [Team.Delete]
		@creatorId = 1,
		@teamId = 2;

END;
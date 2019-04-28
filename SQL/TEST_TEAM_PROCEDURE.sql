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

	SELECT email FROM [USER];

	EXEC [User.Login]
		@email = 'dmitry.yuzefchik@gmail.com',
		@password = '12345';

	EXEC [Team.CreateTeam]
		@userEmail = 'dmitry.yuzefchik@gmail.com',
		@teamName = 'SuperTeam'

	SELECT * FROM [USER];
	SELECT * FROM [TEAM];
	SELECT * FROM [TEAM_USER];
	SELECT * FROM [USER_TOKEN];

	EXEC [Team.AddUserToTeam]
		@creatorEmail = 'dmitry.yuzefchik@gmail.com',
		@teamId = 1,
		@addUserEmail = 'kate.yuzefchik@gmail.com';
	
	EXEC [Team.AddUserToTeam]
		@creatorEmail = 'dmitry.yuzefchik@gmail.com',
		@teamId = 1,
		@addUserEmail = 'kate.yuzefchik@gmail.com';

	EXEC [Team.AddUserToTeam]
		@creatorEmail = 'kate.yuzefchik@gmail.com',
		@teamId = 1,
		@addUserEmail = 'kate.yuzefchik@gmail.com';

	EXEC [User.Login]
		@email = 'kate.yuzefchik@gmail.com',
		@password = '12345';

	EXEC [Team.AddUserToTeam]
		@creatorEmail = 'kate.yuzefchik@gmail.com',
		@teamId = 1,
		@addUserEmail = 'kate.yuzefchik@gmail.com';

	EXEC [Team.GetUserTeams]
		@userEmail = 'dmitry.yuzefchik@gmail.com',
		@skip = 0,
		@amount = 2;

	EXEC [Team.DeleteTeam]
		@creatorEmail = 'kate.yuzefchik@gmail.com',
		@teamId = 1;

	EXEC [Team.DeleteTeam]
		@creatorEmail = 'dmitry.yuzefchik@gmail.com',
		@teamId = 1;

END;
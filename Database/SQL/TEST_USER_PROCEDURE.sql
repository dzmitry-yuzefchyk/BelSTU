--USE Taskboard;

DECLARE
	@userId INT,
	@mail NVARCHAR(256) = 'dmitry.yuzefchik@gmail.com',
	@pwd NVARCHAR(256) = '12345'

BEGIN
SET NOCOUNT ON;
	EXEC [User.Register]
		@email = @mail,
		@password = @pwd,
		@name = 'Dmitry'

	EXEC [User.Login]
		@email = @mail,
		@password = @pwd;
	
	SET @userId = (SELECT id FROM [USER] WHERE email = @mail); 

	--EXEC [User.Logout]
	--	@userId;

	----Не сработает, пользователь вышел -> никто не сможет удалить аккаунт
	--EXEC [User.Delete]
	--	@userId;

	--SELECT * FROM USER_TOKEN WHERE userId = @userId;

	--EXEC [User.Login]
	--	@email = @mail,
	--	@password = @pwd;

	--SELECT * FROM USER_TOKEN WHERE userId = @userId;

	--EXEC [User.RenewToken]
	--	@userId = @userId;

	--SELECT * FROM USER_TOKEN WHERE userId = @userId;

	--EXEC [User.Delete]
	--	@userId;

	SELECT * FROM [USER] WHERE email = @mail;
	SELECT * FROM USER_TOKEN WHERE userId = @userId;
END

SELECT *	
	FROM [USER]
	ORDER BY email
	OFFSET 0 ROWS
	FETCH NEXT 10 ROWS ONLY;

SELECT *
	FROM USER_TOKEN;
USE Taskboard;

BEGIN
	SET NOCOUNT ON;
	DECLARE @i INT = 0,
			@mail NVARCHAR(256),
			@pwd NVARCHAR(256),
			@name NVARCHAR(256);
	WHILE @i < 10000
		BEGIN
			SET @mail = 'dmitry' + CAST(@i AS NVARCHAR(10)) + '@gmail.com';
			SET @pwd = CAST(@i AS NVARCHAR(256));
			SET @name = 'Dmitry' + CAST(@i AS NVARCHAR(10));
			EXEC [User.Register]
				@email = @mail,
				@password = @pwd,
				@name = @name; 

			SET @i += 1;
		END;
END;
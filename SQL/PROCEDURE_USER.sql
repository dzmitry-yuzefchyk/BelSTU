USE Taskboard;

-- USER
GO
CREATE OR ALTER PROCEDURE [User.Register]
    @email NVARCHAR(256), 
    @password NVARCHAR(256),
    @name NVARCHAR(64)
AS
BEGIN
    DECLARE @salt UNIQUEIDENTIFIER = NEWID(),
			@userId INT
    BEGIN TRY

        INSERT INTO [USER](email, "password", salt)
			VALUES(@email, HASHBYTES('SHA2_512', @password + CAST(@salt AS NVARCHAR(36))), @salt);
		SET @userId = (SELECT id FROM [USER] where email = @email);

		INSERT INTO [USER_PROFILE]("name", userId)
			VALUES(@name, @userId);

		INSERT INTO [USER_SETTINGS](userId)
			VALUES(@userId);

    END TRY

    BEGIN CATCH
		SELECT
			ERROR_NUMBER() AS ErrorNumber,
			ERROR_STATE() AS ErrorState,
			ERROR_SEVERITY() AS ErrorSeverity,
			ERROR_PROCEDURE() AS ErrorProcedure,
			ERROR_LINE() AS ErrorLine,
			ERROR_MESSAGE() AS ErrorMessage;
    END CATCH
END;

GO
CREATE OR ALTER PROCEDURE [User.Login]
	@email NVARCHAR(256), 
    @password NVARCHAR(256)
AS
BEGIN
	DECLARE @userId INT

	IF EXISTS (SELECT TOP 1 id FROM [USER] WHERE email = @email)
	BEGIN
		
		SET @userId = (SELECT id FROM [USER] WHERE email = @email AND "password" = HASHBYTES('SHA2_512', @password + CAST(salt AS NVARCHAR(36))));

		IF(@userId IS NULL)
			PRINT 'Incorrect password';
		ELSE 
			BEGIN
				IF EXISTS (SELECT TOP 1 id FROM [USER_TOKEN] WHERE userId = @userId)
					BEGIN TRY
						DELETE [USER_TOKEN] WHERE userId = @userId;
					END TRY
					BEGIN CATCH
						SELECT
							ERROR_LINE() AS ErrorLine,
							ERROR_MESSAGE() AS ErrorMessage;
					END CATCH

				BEGIN TRY
					INSERT INTO [USER_TOKEN](token, created, userId)
						VALUES(NEWID(), GETDATE(),@userId);
					PRINT 'User successfully logged in';
				END TRY

				BEGIN CATCH
					SELECT
						ERROR_LINE() AS ErrorLine,
						ERROR_MESSAGE() AS ErrorMessage;
				END CATCH

			END
	END

	ELSE
		PRINT 'Invalid login';
END;
GO
CREATE OR ALTER PROCEDURE [User.Logout]
	@userId INT
AS
BEGIN
	IF EXISTS (SELECT TOP 1 id FROM [USER_TOKEN] WHERE userId = @userId)
					BEGIN TRY
						DELETE [USER_TOKEN] WHERE userId = @userId;
					END TRY
					BEGIN CATCH
						PRINT 'User token already expired'
						SELECT
							ERROR_LINE() AS ErrorLine,
							ERROR_MESSAGE() AS ErrorMessage;
					END CATCH
END
GO
CREATE OR ALTER PROCEDURE [User.RenewToken]
	@userId INT
AS
BEGIN
	DECLARE @lifeTime INT;
	IF EXISTS (SELECT TOP 1 id FROM [USER_TOKEN] WHERE userId = @userId)
					BEGIN TRY
						SET @lifeTime = (SELECT TOP 1 "lifeTime" FROM [USER_TOKEN] where userId = @userId);
						SET @lifeTime += 10;
						UPDATE [USER_TOKEN] SET "lifeTime" = @lifeTime WHERE userId = @userId;
					END TRY
					BEGIN CATCH
						SELECT
							ERROR_LINE() AS ErrorLine,
							ERROR_MESSAGE() AS ErrorMessage;
					END CATCH
	ELSE
		PRINT 'User token already expired';
END
GO
CREATE OR ALTER PROCEDURE [User.Delete] -- MORE DELETE INCOMING
	@userId INT
AS
BEGIN	
	IF EXISTS (SELECT TOP 1 id FROM [USER_TOKEN] WHERE userId = @userId)
		BEGIN TRY
			DELETE [USER_TOKEN] WHERE userId = @userId;
			DELETE [USER_PROFILE] WHERE userId = @userId;
			DELETE [USER_SETTINGS] WHERE userId = @userId;
			DELETE [USER] WHERE id = @userId;
		END TRY

		BEGIN CATCH
			SELECT
				ERROR_LINE() AS ErrorLine,
				ERROR_MESSAGE() AS ErrorMessage;
		END CATCH
END

--GO
--CREATE OR ALTER PROCEDURE [User.Job.DeleteExpiredTokens]
--AS
--BEGIN
--	DECLARE @id INT,
--		@created DATETIME,
--		@lifeTime INT,
--		@extendedCreateTime DATETIME,
--		@currentTime DATETIME;
--	DECLARE	tokenCursor CURSOR FOR
--			SELECT id, created, "lifeTime"
--			FROM USER_TOKEN;

--	OPEN tokenCursor;

--	WHILE @@FETCH_STATUS = 0
--	BEGIN
--		FETCH NEXT FROM tokenCursor
--			INTO @id, @created, @lifeTime;

--		SET @extendedCreateTime = DATEADD(MINUTE, @lifeTime, @created);
--		SET @currentTime = GETDATE();

--		IF @extendedCreateTime < @currentTime
--		BEGIN
--			DELETE USER_TOKEN WHERE id = @id;
--		END;
--	END;

--	CLOSE tokenCursor;
--	DEALLOCATE tokenCursor;
--END;
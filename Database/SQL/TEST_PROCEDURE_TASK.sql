EXEC
	[Task.Create]
	@userId = 1,
	@teamId = 1,
	@boardId = 5,
	@title = 'fff',
	@content = 'fff',
	@type = 'BUG',
	@priority = 'HIGH',
	@severity = 'NORMAL',
	@status = 'CREATED',
	@asigneeEmail = 'd.yuzefchik@gmail.com',
	@finishTime = '2019-05-07 19:56:56.983',
	@filePath=N'D:\Документы\bstu\Новый текстовый документ.txt';

EXEC
	[Task.Search]
	@userId = 1,
	@teamId = 1,
	@boardId = 5,
	@searchQuery = 'Hello',
	@skip=0,
	@take=20;

SELECT * FROM TASKS;
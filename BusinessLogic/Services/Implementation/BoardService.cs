using BusinessLogic.Contstants;
using BusinessLogic.Models.Board;
using BusinessLogic.Services.Interfaces;
using CommonLogic.Logger;
using DataProvider;
using DataProvider.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Implementation
{
    public class BoardService : IBoardService
    {
        private readonly TaskboardContext _context;
        private readonly ILogger _logger;
        private readonly ISecurityService _securityService;

        public BoardService(TaskboardContext context, ILoggerFactory loggerFactory, ISecurityService securityService)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger<FileLogger>();
            _securityService = securityService;
        }

        public async Task<(bool IsDone, string Message)> CreateBoardAsync(Guid userId, CreateBoardModel model)
        {
            try
            {
                var access = await _securityService.GetUserAccessAsync(userId, model.ProjectId);
                if (!access[UserAction.CREATE_BOARD])
                {
                    return (IsDone: false, Message: "Access denied");
                }

                var board = new Board
                {
                    CreatorId = userId,
                    ProjectId = model.ProjectId,
                    Title = model.Title,
                    TaskPrefix = model.Prefix.ToUpper() ?? model.Title.ToUpper().Substring(0, 4),
                    TaskIndex = 0
                };

                await _context.Boards.AddAsync(board);
                await _context.SaveChangesAsync();
                return (IsDone: true, Message: "Done");
            }
            catch (Exception e)
            {
                _logger.LogError("BoardService, CreateBoardAsync", e);
            }

            return (IsDone: false, Message: "Something went wrong, try again later");
        }

        public async Task<(bool IsDone, string Message)> CreateColumnAsync(Guid userId, CreateColumnModel model)
        {
            try
            {
                var access = await _securityService.GetUserAccessAsync(userId, model.ProjectId);
                if (!access[UserAction.UPDATE_BOARD])
                {
                    return (IsDone: false, Message: "Access denied");
                }

                var lastColumn = _context.Columns.Where(x => x.BoardId == model.BoardId).OrderBy(x => x.Position).LastOrDefault();
                var newPosition = lastColumn == null ? 0 : lastColumn.Position + 1;
                var column = new Column
                {
                    BoardId = model.BoardId,
                    Title = model.Title,
                    Position = newPosition
                };

                await _context.Columns.AddAsync(column);
                await _context.SaveChangesAsync();
                return (IsDone: true, Message: "Done");
            }
            catch (Exception e)
            {
                _logger.LogError("BoardService, CreateColumnAsync", e);
            }

            return (IsDone: false, Message: "Something went wrong, try again later");
        }

        public async Task<BoardViewModel> GetBoardAsync(Guid userId, int projectId, int boardId, string searchBy, bool assignedToMe, int orderBy)
        {
            try
            {
                var projectUser = _context.ProjectUsers.SingleOrDefault(x => x.ProjectId == projectId && x.UserId == userId);
                if (projectUser == null)
                {
                    return null;
                }

                Func<TaskView, object> orderByFilter = x => orderBy switch
                {
                    (int)Filter.PRIORITY => x.Priority,
                    (int)Filter.SEVERITY => x.Severity,
                    (int)Filter.TYPE => x.Type,
                    _ => x.Title
                };
                Func<DataProvider.Entities.Task, bool> assigneeFilter = x => assignedToMe
                    ? x.AssigneeId == userId
                    : true;
                Func<DataProvider.Entities.Task, bool> searchFilter = x => searchBy != ""
                    ? x.Title.Contains(searchBy)
                    : true;
                

                var board = await _context.Boards
                    .Where(x => x.Id == boardId)
                    .Include(x => x.Columns)
                    .ThenInclude(x => x.Tasks)
                    .ThenInclude(x => x.Assignee)
                    .ThenInclude(x => x.Profile)
                    .Include(x => x.Columns)
                    .ThenInclude(x => x.Tasks)
                    .ThenInclude(x => x.Creator)
                    .ThenInclude(x => x.Profile)
                    .Select(x => new BoardViewModel
                    {
                        Id = x.Id,
                        TaskIndex = x.TaskIndex,
                        TaskPrefix = x.TaskPrefix,
                        Columns = x.Columns.Select(x => new ColumnView
                        {
                            Id = x.Id,
                            Position = x.Position,
                            Title = x.Title,
                            Tasks = x.Tasks
                            .Where(assigneeFilter)
                            .Where(searchFilter)
                            .Select(x => new TaskView
                            {
                                Id = x.Id,
                                Title = x.Title,
                                Content = x.Content,
                                Type = x.Type,
                                Severity = x.Severity,
                                Priority = x.Priority,
                                AssigneeTag = x.Assignee.Profile.Tag,
                                AssigneeIcon = x.Assignee.Profile.Icon,
                                CreatorTag = x.Assignee.Profile.Tag,
                                CreatorIcon = x.Assignee.Profile.Icon
                            })
                            .OrderBy(orderByFilter)
                        })
                        .OrderBy(x => x.Position)
                    })
                    .ToListAsync();

                return board.FirstOrDefault();
            }
            catch (Exception e)
            {
                _logger.LogError("BoardService, GetBoardAsync", e);
            }

            return null;
        }

        public async Task<UpdateBoardModel> GetBoardSettingsAsync(Guid userId, int boardId)
        {
            try
            {
                var board = await _context.Boards.FindAsync(boardId);
                var access = await _securityService.GetUserAccessAsync(userId, board.ProjectId);
                if (!access[UserAction.UPDATE_BOARD])
                {
                    return null;
                }

                var boardSettings = await _context.Boards
                    .Where(x => x.Id == boardId)
                    .Include(x => x.Columns)
                    .Select(x => new UpdateBoardModel
                    {
                        Id = x.Id,
                        Prefix = x.TaskPrefix,
                        Title = x.Title,
                        Columns = x.Columns.Select(x => new ColumnInfo
                        {
                            Id = x.Id,
                            Title = x.Title,
                            Position = x.Position
                        })
                    })
                    .ToListAsync();

                return boardSettings.FirstOrDefault();
            }
            catch (Exception e)
            {
                _logger.LogError("BoardService, GetBoardSettingsAsync", e);
            }

            return null;
        }

        public async Task<(bool IsDone, string Message)> UpdateBoardAsync(Guid userId, UpdateBoardModel model)
        {
            try
            {
                var board = await _context.Boards.FindAsync(model.Id);
                var access = await _securityService.GetUserAccessAsync(userId, board.ProjectId);
                if (!access[UserAction.UPDATE_BOARD])
                {
                    return (IsDone: false, Message: "Access denied");
                }

                board.Title = model.Title;
                board.TaskPrefix = model.Prefix;
                _context.Boards.Update(board);

                var columnIds = model.Columns.Select(x => x.Id).ToList();
                var columns = await _context.Columns.Where(x => columnIds.Contains(x.Id)).ToListAsync();
                columns.ForEach(x =>
                    {
                        var column = model.Columns.First(c => c.Id == x.Id);
                        x.Position = column.Position;
                        x.Title = column.Title;
                    });

                _context.UpdateRange(columns);
                await _context.SaveChangesAsync();
                return (IsDone: true, Message: "Done");
            }
            catch (Exception e)
            {
                _logger.LogError("BoardService, UpdateBoardAsync", e);
            }

            return (IsDone: false, Message: "Something went wrong, try again later");
        }
    }
}

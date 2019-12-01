using BusinessLogic.Contstants;
using BusinessLogic.Models.Task;
using BusinessLogic.Services.Interfaces;
using CommonLogic.Logger;
using DataProvider;
using DataProvider.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Implementation
{
    public class TaskService : ITaskService
    {
        private readonly TaskboardContext _context;
        private readonly ILogger _logger;
        private readonly ISecurityService _securityService;

        public TaskService(TaskboardContext context, ILoggerFactory loggerFactory, ISecurityService securityService)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger<FileLogger>();
            _securityService = securityService;
        }

        public async Task<(bool IsDone, string Message)> CreateTaskAsync(Guid userId, CreateTaskModel model)
        {
            try
            {
                var access = await _securityService.GetUserAccessAsync(userId, model.ProjectId);
                if (!access[UserAction.CREATE_TASK])
                {
                    return (IsDone: false, Message: "Access denied");
                }

                var assigneeId = _context.Users.SingleOrDefault(x => x.Email == model.AssigneeEmail)?.Id;
                var attachments = await ToAttachments(model.Attachments);
                var Task = new DataProvider.Entities.Task
                {
                    Title = model.Title,
                    ColumnId = model.ColumnId,
                    CreatorId = userId,
                    AssigneeId = assigneeId,
                    Content = model.Content,
                    Priority = model.Priority,
                    Severity = model.Severity,
                    Type = model.Type,
                    Attachments = attachments.ToList()
                };
                await _context.Tasks.AddAsync(Task);
                await _context.SaveChangesAsync();
                return (IsDone: true, Message: "Success");
            }
            catch (Exception e)
            {
                _logger.LogError("TaskService: CreateTaskAsync", e);
            }

            return (IsDone: false, Message: "Something went wrong, try again later");
        }

        public async Task<bool> MoveTaskAsync(Guid userId, int projectId, int taskId, int columnId)
        {
            try
            {
                var access = await _securityService.GetUserAccessAsync(userId, projectId);
                if (!access[UserAction.CREATE_TASK])
                {
                    return false;
                }

                var task = await _context.Tasks.FindAsync(taskId);
                task.ColumnId = columnId;
                _context.Update(task);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("TaskService: MoveTaskAsync", e);
            }

            return false;
        }

        public async Task<TaskViewModel> GetTaskAsync(Guid userId, int taskId, int projectId)
        {
            try
            {
                var projectUser = _context.ProjectUsers.SingleOrDefault(x => x.ProjectId == projectId && x.UserId == userId);
                if (projectUser == null)
                {
                    return null;
                }

                var access = await _securityService.GetUserAccessAsync(userId, projectId);
                var task = await _context.Tasks
                    .Where(x => x.Id == taskId)
                    .Include(x => x.Assignee)
                    .ThenInclude(x => x.Profile)
                    .Include(x => x.Creator)
                    .ThenInclude(x => x.Profile)
                    .Include(x => x.Attachments)
                    .Include(x => x.Comments)
                    .ThenInclude(x => x.Attachments)
                    .Include(x => x.Comments)
                    .ThenInclude(x => x.Creator)
                    .ThenInclude(x => x.Profile)
                    .Select(x => new TaskViewModel
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Content = x.Content,
                        Type = x.Type,
                        Priority = x.Priority,
                        Severity = x.Severity,
                        AssigneeTag = x.Assignee.Profile.Tag,
                        AssigneeIcon = x.Assignee.Profile.Icon,
                        CreatorTag = x.Creator.Profile.Tag,
                        CreatorIcon = x.Creator.Profile.Icon,
                        CanUpdateTask = access[UserAction.UPDATE_TASK],
                        CanComment = access[UserAction.CREATE_COMMENT],
                        Attachments = x.Attachments.Select(x => new AttachmentView
                        {
                            Id = x.Id,
                            Name = x.FileName
                        }),
                        Comments = x.Comments.Select(x => new CommentView
                        {
                            Content = x.Content,
                            CreationDate = x.CreationDate,
                            CreatorIcon = x.Creator.Profile.Icon,
                            CreatorTag = x.Creator.Profile.Tag,
                            Attachments = x.Attachments.Select(x => new AttachmentView
                            {
                                Id = x.Id,
                                Name = x.FileName
                            })
                        })
                    }).SingleOrDefaultAsync();

                return task;
            }
            catch (Exception e)
            {
                _logger.LogError("TaskService: GetTaskAsync", e);
            }

            return null;
        }

        public async Task<bool> UpdateTaskAsync(Guid userId, UpdateTaskModel model)
        {
            try
            {
                var access = await _securityService.GetUserAccessAsync(userId, model.ProjectId);
                if (access[UserAction.UPDATE_TASK])
                {
                    return false;
                }

                var task = await _context.Tasks.FindAsync(model.TaskId);
                if (task == null)
                {
                    return false;
                }

                var user = await _context.Users.SingleOrDefaultAsync(x => x.Email == model.AssigneeEmail);
                var assignee = await _context.ProjectUsers.SingleOrDefaultAsync(x => x.ProjectId == model.ProjectId && x.UserId == user.Id);
                task.Title = model.Title;
                task.Content = model.Content;
                task.Priority = model.Priority;
                task.Type = model.Type;
                task.Severity = model.Severity;
                task.AssigneeId = assignee?.UserId;

                _context.Update(task);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("TaskService: UpdateTaskAsync", e);
            }

            return false;
        }

        public async Task<bool> DeleteTaskAsync(Guid userId, int taskId, int projectId)
        {
            try
            {
                var access = await _securityService.GetUserAccessAsync(userId, projectId);
                if (!access[UserAction.DELETE_TASK])
                {
                    return false;
                }

                var attachments = _context.TaskAttachments.Where(x => x.TaskId == taskId);
                _context.TaskAttachments.RemoveRange(attachments);

                var commentsAttachments = _context.CommentAttachments
                    .Include(x => x.Comment)
                    .Where(x => x.Comment.TaskId == taskId);
                _context.CommentAttachments.RemoveRange(commentsAttachments);

                var comments = _context.Comments.Where(x => x.TaskId == taskId);
                _context.Comments.RemoveRange(comments);

                var task = await _context.Tasks.FindAsync(taskId);
                _context.Tasks.Remove(task);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("TaskService: DeleteTaskAsync", e);
            }

            return false;
        }

        public async Task<bool> DeleteBoardTasksAsync(Guid userId, int boardId, int projectId)
        {
            try
            {
                var access = await _securityService.GetUserAccessAsync(userId, projectId);
                if (!access[UserAction.DELETE_BOARD])
                {
                    return false;
                }

                var board = await _context.Boards.FindAsync(boardId);
                if (board == null)
                {
                    return false;
                }

                var columns = _context.Columns.Where(x => x.BoardId == boardId);
                var tasks = _context.Tasks.Where(x => columns.Select(c => c.Id).Contains(x.ColumnId));
                foreach (var task in tasks)
                {
                    await DeleteTaskAsync(userId, task.Id, projectId);
                }

                _context.RemoveRange(columns);
                _context.Remove(board);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("TaskService: DeleteBoardTasksAsync", e);
            }

            return false;
        }

        public async Task<bool> LeaveCommentAsync(Guid userId, CreateCommentModel model)
        {
            try
            {
                var access = await _securityService.GetUserAccessAsync(userId, model.ProjectId);
                if (access[UserAction.CREATE_COMMENT])
                {
                    return false;
                }
                var task = await _context.Tasks.FindAsync(model.TaskId);
                if (task == null)
                {
                    return false;
                }

                var attachments = await ToCommentAttachments(model.Attachments);
                var comment = new Comment
                {
                    Content = model.Content,
                    CreatorId = userId,
                    CreationDate = DateTime.Now,
                    TaskId = model.TaskId,
                    Attachments = attachments.ToList()
                };

                await _context.Comments.AddAsync(comment);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("TaskService: LeaveCommentAsync", e);
            }

            return false;
        }

        private async Task<IEnumerable<CommentAttachment>> ToCommentAttachments(List<IFormFile> formFiles)
        {
            var files = new List<CommentAttachment>();
            foreach (var file in formFiles)
            {
                await using var memoryStream = new MemoryStream();
                file.CopyTo(memoryStream);
                files.Add(new CommentAttachment
                {
                    File = memoryStream.ToArray(),
                    FileName = file.FileName
                });
            }

            return files;
        }

        private async Task<IEnumerable<TaskAttachment>> ToAttachments(List<IFormFile> formFiles)
        {
            var files = new List<TaskAttachment>();
            foreach (var file in formFiles)
            {
                await using var memoryStream = new MemoryStream();
                file.CopyTo(memoryStream);
                files.Add(new TaskAttachment
                {
                    File = memoryStream.ToArray(),
                    FileName = file.FileName
                });
            }

            return files;
        }
    }
}

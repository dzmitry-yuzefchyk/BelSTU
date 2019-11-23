using BusinessLogic.Contstants;
using BusinessLogic.Models.Notification;
using BusinessLogic.Models.Project;
using BusinessLogic.Services.Interfaces;
using CommonLogic.Logger;
using DataProvider;
using DataProvider.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Implementation
{
    public class ProjectService : IProjectService
    {
        private readonly TaskboardContext _context;
        private readonly ILogger _logger;
        private readonly INotificationService _notificationService;
        private readonly ISecurityService _securityService;

        public ProjectService(TaskboardContext context, ILoggerFactory loggerFactory,
            INotificationService notificationService, ISecurityService securityService)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger<FileLogger>();
            _notificationService = notificationService;
            _securityService = securityService;
        }

        public async Task<(bool IsDone, string Message)> CreateProjectAsync(Guid userId, CreateProjectModel model)
        {
            try
            {
                var projectSettings = new ProjectSettings
                {
                    AccessToChangeProject = (int)UserRole.ADMIN,
                    AccessToChangeBoard = (int)UserRole.ADMIN,
                    AccessToChangeTask = (int)UserRole.MEMBER
                };
                var project = new Project
                {
                    Title = model.Title,
                    Description = model.Description,
                    Settings = projectSettings
                };
                var projectUser = new ProjectUser { Role = (int)UserRole.ADMIN, UserId = userId, Project = project };

                await _context.Projects.AddAsync(project);
                await _context.ProjectUsers.AddAsync(projectUser);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    return (IsDone: true, Message: "Done");
                }
            }
            catch (Exception e)
            {
                _logger.LogError("ProjectService, CreateProjectAsync", e);
            }

            return (IsDone: false, Message: "Something went wrong, please try again later");
        }

        public IEnumerable<ProjectViewModel> GetProjects(Guid userId, int page, int size)
        {
            try
            {
                var projects = _context.ProjectUsers
                    .Where(x => x.UserId == userId)
                    .Select(x => x.Project)
                    .OrderBy(x => x.Title)
                    .Skip(size * page)
                    .Take(size)
                    .Select(x => new ProjectViewModel
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Description = x.Description
                    })
                    .ToList();

                return projects;
            }
            catch (Exception e)
            {
                _logger.LogError("ProjectService, GetProjects", e);
            }

            return null;
        }

        public async Task<ProjectAccessViewModel> GetProjectAsync(Guid userId, int projectId)
        {
            try
            {
                var userAccess = await _securityService.GetUserAccess(userId, projectId);
                var userProject = _context.ProjectUsers
                    .Include(x => x.Project)
                    .Where(x => x.UserId == userId && x.ProjectId == projectId)
                    .Select(x =>
                        new ProjectAccessViewModel()
                        {
                            Id = projectId,
                            Title = x.Project.Title,
                            Description = x.Project.Description,
                            CanUpdateProject = userAccess[UserAction.UPDATE_PROJECT],
                            CanChangeSecurity = userAccess[UserAction.CHANGE_SECURITY],
                            CanDeleteProject = userAccess[UserAction.DELETE_PROJECT],
                            CanCreateBoard = userAccess[UserAction.CREATE_BOARD],
                            CanDeleteBoard = userAccess[UserAction.DELETE_BOARD],
                            CanUpdateBoard = userAccess[UserAction.UPDATE_BOARD],
                            CanCreateTask = userAccess[UserAction.CREATE_TASK],
                            CanUpdateTask = userAccess[UserAction.UPDATE_TASK],
                            CanDeleteTask = userAccess[UserAction.DELETE_TASK],
                            CanComment = userAccess[UserAction.CREATE_COMMENT]
                        }).
                    SingleOrDefault();

                return userProject;
            }
            catch (Exception e)
            {
                _logger.LogError("ProjectService, GetProjectAsync", e);
            }

            return null;
        }

        public async Task<(bool IsDone, string Message)> AddUserToProjectAsync(Guid userId, AddUserModel model)
        {
            try
            {
                var userAccess = await _securityService.GetUserAccess(userId, model.ProjectId);
                if (!userAccess[UserAction.UPDATE_PROJECT])
                {
                    _logger.LogWarning("ProjectService, AddUserToProjectAsync", "User action denied", userId);
                    return (IsDone: false, Message: "Access denied");
                }

                var user = _context.Users.SingleOrDefault(x => x.Email == model.Email);
                if (user == null)
                {
                    return (IsDone: false, Message: "There is no such user");
                }

                var existingProjectUser = _context.ProjectUsers.SingleOrDefault(x => x.ProjectId == model.ProjectId && x.UserId == user.Id);
                if (existingProjectUser != null)
                {
                    return (IsDone: false, Message: "User already in project");
                }

                var projectUser = new ProjectUser { Role = (int)UserRole.MEMBER, User = user, ProjectId = model.ProjectId };
                await _context.ProjectUsers.AddAsync(projectUser);

                var result = await _context.SaveChangesAsync();
                if (result < 1)
                {
                    return (IsDone: false, Message: "Something went wrong, please try again later");
                }

                var notification = new CreateNotificationModel
                {
                    Description = "You were added to project",
                    DirectLink = $"/project/{model.ProjectId}",
                    RecipientEmail = user.Email,
                    Subject = "You were added to project"
                };
                var notificationResult = await _notificationService.CreateNotificationAsync(notification);
                return (IsDone: true, Message: "Success");
            }
            catch (Exception e)
            {
                _logger.LogError("ProjectService, AddUserToProjectAsync", e);
            }

            return (IsDone: false, Message: "Something went wrong, please try again later");
        }

        public async Task<(bool IsDone, string Message)> RemoveUserFromProjectAsync(Guid userId, AddUserModel model)
        {
            try
            {
                var userAccess = await _securityService.GetUserAccess(userId, model.ProjectId);
                if (!userAccess[UserAction.UPDATE_PROJECT])
                {
                    _logger.LogWarning("ProjectService, RemoveUserFromProjectAsync", "User action denied", userId);
                    return (IsDone: false, Message: "Access denied");
                }

                var user = _context.Users.SingleOrDefault(x => x.Email == model.Email);
                if (user == null)
                {
                    return (IsDone: false, Message: "There is no such user");
                }

                var existingProjectUser = _context.ProjectUsers.SingleOrDefault(x =>
                    x.Role != (int)UserRole.ADMIN &&
                    x.ProjectId == model.ProjectId &&
                    x.UserId == user.Id);
                if (existingProjectUser == null)
                {
                    return (IsDone: false, Message: "There is no such user in project, or you're trying to delete project admin");
                }

                _context.ProjectUsers.Remove(existingProjectUser);
                var result = await _context.SaveChangesAsync();
                if (result < 1)
                {
                    return (IsDone: false, Message: "Something went wrong, please try again later");
                }

                var notification = new CreateNotificationModel
                {
                    Description = "You were removed from project",
                    DirectLink = "/projects",
                    RecipientEmail = user.Email,
                    Subject = "You were removed from project"
                };
                var notificationResult = await _notificationService.CreateNotificationAsync(notification);
                return (IsDone: true, Message: "Success");
            }
            catch (Exception e)
            {
                _logger.LogError("ProjectService, RemoveUserFromProjectAsync", e);
            }

            return (IsDone: false, Message: "Something went wrong, please try again later");
        }
    }
}

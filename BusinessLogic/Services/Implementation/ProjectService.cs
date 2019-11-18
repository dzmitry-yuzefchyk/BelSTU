using BusinessLogic.Models.Project;
using BusinessLogic.Services.Interfaces;
using CommonLogic.Logger;
using DataProvider;
using DataProvider.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
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

        public ProjectService(TaskboardContext context,
            ILoggerFactory loggerFactory, INotificationService notificationService)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger<FileLogger>();
            _notificationService = notificationService;
        }

        public async Task<(bool IsDone, string Message)> CreateProjectAsync(CreateProjectModel model)
        {
            try
            {
                var projectSettings = new ProjectSettings
                {
                    AccessToChangeProject = model.AccessToChangeProject,
                    AccessToCreateBoard = model.AccessToCreateBoard,
                    AccessToDeleteBoard = model.AccessToDeleteBoard
                };
                var projectSecuritySettings = new ProjectSecuritySettings { };
                var project = new Project
                {
                    Title = model.Title,
                    Description = model.Description,
                    Settings = projectSettings,
                    ProjectSecuritySettings = projectSecuritySettings
                };

                await _context.Projects.AddAsync(project);
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

        public async Task<IEnumerable<ProjectViewModel>> GetProjectsAsync(Guid userId, int page, int size)
        {
            try
            {
                var projects = await _context.ProjectUsers
                    .Where(x => x.UserId == userId)
                    .Select(x => x.Project)
                    .OrderBy(x => x.Title)
                    .Skip(size * page)
                    .Take(size)
                    .Select(x => new ProjectViewModel
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Description = x.Description,
                        CanUserChangeProject = false,
                        CanUserCreateBoard = false
                    })
                    .ToListAsync();
                return projects;
            }
            catch (Exception e)
            {
                _logger.LogError("ProjectService, GetProjectsAsync", e);
            }

            return null;
        }
    }
}

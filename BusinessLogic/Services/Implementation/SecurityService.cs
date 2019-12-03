using BusinessLogic.Contstants;
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
    public class SecurityService : ISecurityService
    {
        private readonly TaskboardContext _context;
        private readonly ILogger _logger;

        public SecurityService(TaskboardContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger<FileLogger>();
        }

        public async Task<(bool IsDone, string Message)> UpdateAsync(Guid userId, UpdatePoliciesModel model)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                var userAccess = await GetUserAccessAsync(user.Id, model.ProjectId);
                if (!userAccess[UserAction.CHANGE_SECURITY])
                {
                    return (IsDone: false, Message: "You don't have rights");
                }

                foreach (var projectUser in model.Users)
                {
                    var uId = _context.Users.SingleOrDefault(u => u.Email == projectUser.Email).Id;

                    foreach (var action in projectUser.Actions)
                    {
                        var oldPolicy = _context.ProjectSecurityPolicies
                            .SingleOrDefault(x => x.UserId == uId && x.Action == (int)action.Action && x.ProjectSettingsId == model.ProjectId);
                        if (oldPolicy != null)
                        {
                            _context.Remove(oldPolicy);
                            await _context.SaveChangesAsync();
                        }

                        var policy = new ProjectSecurityPolicy
                        {
                            Action = (int)action.Action,
                            IsAllowed = action.Allowed,
                            UserId = uId,
                            ProjectSettingsId = model.ProjectId
                        };
                        await _context.ProjectSecurityPolicies.AddAsync(policy);
                    }
                }

                await _context.SaveChangesAsync();
                return (IsDone: true, Message: "Success");
            }
            catch (Exception e)
            {
                _logger.LogError("SecurityService, UpdateAsync", e);
            }

            return (IsDone: false, Message: "Could not update security settings");
        }

        public async Task<Dictionary<UserAction, bool>> GetUserAccessAsync(Guid userId, int projectId)
        {
            Dictionary<UserAction, bool> access = Enum.GetValues(typeof(UserAction))
                .Cast<UserAction>()
                .Select(x => new { Key = x, Value = false })
                .ToDictionary(x => x.Key, x => x.Value);

            try
            {
                var projectUser = _context.ProjectUsers.SingleOrDefault(x => x.ProjectId == projectId && x.UserId == userId);
                if (projectUser == null)
                {
                    return access;
                }

                var projectSettings = await _context.ProjectSettings.FindAsync(projectId);

                if (projectSettings.UseAdvancedSecuritySettings)
                {
                    access.ToList().ForEach
                        (pair =>
                            {
                                if (projectUser.Role == (int)UserRole.ADMIN)
                                {
                                    access[pair.Key] = true;
                                }
                                else
                                {
                                    var projectPolicy = _context.ProjectSecurityPolicies
                                    .SingleOrDefault(x =>
                                        x.ProjectSettingsId == projectId &&
                                        x.UserId == userId &&
                                        x.Action == (int)pair.Key);
                                    access[pair.Key] = projectPolicy.IsAllowed;
                                }
                            }
                        );
                }
                else
                {
                    var userAccessLevel = projectUser.Role;
                    access.ToList().ForEach
                    (pair =>
                        {

                            var neededLevel = pair.Key switch
                            {
                                UserAction.UPDATE_PROJECT => projectSettings.AccessToChangeProject,
                                UserAction.UPDATE_BOARD => projectSettings.AccessToChangeBoard,
                                UserAction.UPDATE_TASK => projectSettings.AccessToChangeTask,
                                UserAction.CHANGE_SECURITY => (int)AccessLevel.PROJECT_CREATOR,
                                UserAction.CREATE_BOARD => projectSettings.AccessToChangeBoard,
                                UserAction.CREATE_COMMENT => (int)AccessLevel.PROJECT_MEMBER,
                                UserAction.CREATE_TASK => projectSettings.AccessToChangeTask,
                                UserAction.DELETE_BOARD => (int)AccessLevel.PROJECT_CREATOR,
                                UserAction.DELETE_TASK => (int)AccessLevel.PROJECT_CREATOR,
                                UserAction.DELETE_PROJECT => (int)AccessLevel.PROJECT_CREATOR,
                                _ => -1
                            };
                            access[pair.Key] = userAccessLevel <= neededLevel;
                        }
                    );
                }
            }
            catch (Exception e)
            {
                _logger.LogError("SecurityService, CanUserDoAsync", e);
            }

            return access;
        }

        public async Task<PoliciesModel> GetUserAccessesAsync(int projectId, int page, int size)
        {
            try
            {
                var projectSettings = await _context.ProjectSettings.FindAsync(projectId);
                var projectUsers = _context.ProjectUsers
                    .Where(x => x.ProjectId == projectId)
                    .Skip(page * size)
                    .Take(size)
                    .ToList();
                var total = _context.ProjectUsers.Where(x => x.ProjectId == projectId).Count();
                var users = new List<ProjectUserModel>();
                foreach (var projectUser in projectUsers)
                {
                    var user = await _context.Users.FindAsync(projectUser.UserId);
                    var userProfile = await _context.UserProfiles.FindAsync(projectUser.UserId);
                    var actions = await GetUserAccessAsync(projectUser.UserId, projectId);
                    users.Add(new ProjectUserModel
                    {
                        Email = user.Email,
                        Tag = userProfile.Tag,
                        IsAdmin = projectUser.Role == (int)UserRole.ADMIN,
                        ChangingBlocked = !projectSettings.UseAdvancedSecuritySettings,
                        Actions = actions.Select(x => new Actions
                        {
                            Action = x.Key,
                            Allowed = x.Value
                        }).ToList()
                    });
                }
                return new PoliciesModel { Users = users, Total = total };
            }
            catch (Exception e)
            {
                _logger.LogError("SecurityService, GetUserAccessesAsync", e);
            }

            return null;
        }
    }
}

using BusinessLogic.AdvancedSecurity;
using BusinessLogic.Contstants;
using BusinessLogic.Models.Project;
using BusinessLogic.Services.Interfaces;
using CommonLogic.Logger;
using DataProvider;
using DataProvider.Entities;
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

        public async Task<(bool IsDone, string Message)> UpdateAsync(Guid userId, UpdateSecurityModel model)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                var userAccess = await GetUserAccessAsync(user.Id, model.ProjectId);
                if (!userAccess[UserAction.CHANGE_SECURITY])
                {
                    return (IsDone: false, Message: "You don't have rights");
                }

                var policies = model.Policies
                    .Select(x =>
                    {
                        var userId = _context.Users.SingleOrDefault(u => u.Email == x.UserEmail).Id;
                        var oldPolicy = _context.ProjectSecurityPolicies.SingleOrDefault(p => p.UserId == userId);
                        _context.ProjectSecurityPolicies.Remove(oldPolicy);
                        return new ProjectSecurityPolicy
                        {
                            Action = x.Action,
                            IsAllowed = x.IsAllowed,
                            UserId = userId,
                            ProjectSettingsId = model.ProjectId
                        };
                    });
                await _context.SaveChangesAsync();
                await _context.ProjectSecurityPolicies.AddRangeAsync(policies);

                var result = await _context.SaveChangesAsync();
                if (result > 1)
                {
                    return (IsDone: true, Message: "Success");
                }
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
                var projectSettings = await _context.ProjectSettings.FindAsync(projectId);

                if (projectSettings.UseAdvancedSecuritySettings)
                {
                    access.ToList().ForEach
                        (pair =>
                            {
                                var projectPolicy = _context.ProjectSecurityPolicies
                                    .SingleOrDefault(x =>
                                        x.ProjectSettingsId == projectId &&
                                        x.UserId == userId &&
                                        x.Action == (int)pair.Key);
                                access[pair.Key] = projectPolicy.IsAllowed;
                            }
                        );
                }
                else
                {
                    var userAccessLevel = _context.ProjectUsers.SingleOrDefault(x => x.ProjectId == projectId && x.UserId == userId).Role;
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

        public PoliciesModel GetUserAccesses(int projectId, int page, int size)
        {
            try
            {
                var projectUsers = _context.ProjectUsers
                    .Where(x => x.ProjectId == projectId)
                    .Skip(page * size)
                    .Take(size)
                    .ToList();
                var total = _context.ProjectUsers.Where(x => x.ProjectId == projectId).Count();

                var users = projectUsers.Select(async x =>
                {
                    var user = await _context.Users.FindAsync(x.UserId);
                    var userProfile = await _context.UserProfiles.FindAsync(x.UserId);
                    return new ProjectUserModel
                    {
                        Email = user.Email,
                        Tag = userProfile.Tag,
                        Actions = await GetUserAccessAsync(x.UserId, projectId)
                    };
                });
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

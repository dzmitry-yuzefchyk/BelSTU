using BusinessLogic.AdvancedSecurity;
using BusinessLogic.Contstants;
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

        public async Task<(bool IsDone, string Message)> UpdateAsync(UpdateSecurityModel model)
        {
            try
            {
                var user = await _context.Users.FindAsync(model.Email);
                var userAccess = await GetUserAccess(user.Id, model.ProjectId);
                if (!userAccess[UserAction.CHANGE_SECURITY])
                {
                    return (IsDone: false, Message: "You don't have rights");
                }

                var policies = model.Policies
                    .Select(x => new ProjectSecurityPolicy
                    {
                        Action = x.Action,
                        IsAllowed = x.IsAllowed,
                        UserId = _context.Users.SingleOrDefault(x => x.Email == model.Email).Id,
                        ProjectSettingsId = model.ProjectId
                    });
                _context.ProjectSecurityPolicies.AddRange(policies);

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

        public async Task<Dictionary<UserAction, bool>> GetUserAccess(Guid userId, int projectId)
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
    }
}

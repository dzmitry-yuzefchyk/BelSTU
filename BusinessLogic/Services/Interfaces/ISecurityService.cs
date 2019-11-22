using BusinessLogic.AdvancedSecurity;
using BusinessLogic.Contstants;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Interfaces
{
    public interface ISecurityService
    {
        Task<Dictionary<UserAction, bool>> GetUserAccess(Guid userId, int projectId);
        Task<(bool IsDone, string Message)> UpdateAsync(UpdateSecurityModel model);
    }
}

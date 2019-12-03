using BusinessLogic.Contstants;
using BusinessLogic.Models.Project;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Interfaces
{
    public interface ISecurityService
    {
        Task<Dictionary<UserAction, bool>> GetUserAccessAsync(Guid userId, int projectId);
        Task<(bool IsDone, string Message)> UpdateAsync(Guid userId, UpdatePoliciesModel model);
        Task<PoliciesModel> GetUserAccessesAsync(int projectId, int page, int size);
    }
}

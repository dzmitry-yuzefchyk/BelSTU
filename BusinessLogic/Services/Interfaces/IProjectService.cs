using BusinessLogic.Models.Project;
using System;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Interfaces
{
    public interface IProjectService
    {
        Task<(bool IsDone, string Message)> CreateProjectAsync(Guid userId, CreateProjectModel model);
        ProjectsViewModel GetProjects(Guid userId, int page, int size);
        Task<ProjectAccessViewModel> GetProjectAsync(Guid userId, int projectId);
        Task<(bool IsDone, string Message)> AddUserToProjectAsync(Guid userId, AddUserModel model);
        Task<(bool IsDone, string Message)> RemoveUserFromProjectAsync(Guid userId, AddUserModel model);
        Task<UpdateProjectModel> GetSettingsAsync(Guid userId, int projectId);
        Task<(bool IsDone, string Message)> UpdateSettingsAsync(Guid userId, UpdateProjectModel model);
        Task<bool> DeleteProjectAsync(Guid userId, int projectId);
    }
}

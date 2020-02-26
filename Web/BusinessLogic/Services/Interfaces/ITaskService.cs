using BusinessLogic.Models.Task;
using DataProvider.Entities;
using System;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Interfaces
{
    public interface ITaskService
    {
        Task<(bool IsDone, string Message)> CreateTaskAsync(Guid userId, CreateTaskModel model);
        Task<bool> MoveTaskAsync(Guid userId, int projectId, int taskId, int columnId);
        Task<TaskViewModel> GetTaskAsync(Guid userId, int taskId, int projectId);
        Task<bool> UpdateTaskAsync(Guid userId, UpdateTaskModel model);
        Task<bool> DeleteTaskAsync(Guid userId, int taskId, int projectId);
        Task<bool> DeleteBoardTasksAsync(Guid userId, int boardId, int projectId);
        Task<bool> LeaveCommentAsync(Guid userId, CreateCommentModel model);
        Task<TaskAttachment> DownloadAsync(Guid userId, int attachmentId, int projectId);
    }
}

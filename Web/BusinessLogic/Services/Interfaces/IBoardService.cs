using BusinessLogic.Models.Board;
using System;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Interfaces
{
    public interface IBoardService
    {
        Task<(bool IsDone, string Message)> CreateBoardAsync(Guid userId, CreateBoardModel model);
        Task<(bool IsDone, string Message)> CreateColumnAsync(Guid userId, CreateColumnModel model);
        Task<BoardViewModel> GetBoardAsync(Guid userId, int projectId, int boardId, string searchBy, bool assignedToMe, int orderBy);
        Task<UpdateBoardModel> GetBoardSettingsAsync(Guid userId, int boardId);
        Task<(bool IsDone, string Message)> UpdateBoardAsync(Guid userId, UpdateBoardModel model);
    }
}

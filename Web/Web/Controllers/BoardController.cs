using BusinessLogic.Models.Board;
using BusinessLogic.Services.Interfaces;
using CommonLogic.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoardController : ControllerBase
    {
        private readonly IBoardService _boardService;

        public BoardController(IBoardService boardService)
        {
            _boardService = boardService;
        }

        [HttpGet("{boardId}")]
        public async Task<IActionResult> GetBoard([FromRoute]int boardId, [FromQuery]int projectId,
            [FromQuery]string searchBy, [FromQuery]bool assignedToMe, [FromQuery]int orderBy)
        {
            var result = await _boardService.GetBoardAsync(this.UserId(), projectId, boardId, searchBy, assignedToMe, orderBy);
            return result != null ? (IActionResult)Ok(result) : BadRequest(result);
        }

        [HttpGet("Settings")]
        public async Task<IActionResult> GetBoardSettings(int boardId)
        {
            var result = await _boardService.GetBoardSettingsAsync(this.UserId(), boardId);
            return result != null ? (IActionResult)Ok(result) : BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBoard([FromBody]CreateBoardModel model)
        {
            var (IsDone, Message) = await _boardService.CreateBoardAsync(this.UserId(), model);
            return IsDone ? (IActionResult)Ok(Message) : BadRequest(Message);
        }

        [HttpPost("Column")]
        public async Task<IActionResult> CreateColumn([FromBody]CreateColumnModel model)
        {
            var (IsDone, Message) = await _boardService.CreateColumnAsync(this.UserId(), model);
            return IsDone ? (IActionResult)Ok(Message) : BadRequest(Message);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBoard([FromBody]UpdateBoardModel model)
        {
            var (IsDone, Message) = await _boardService.UpdateBoardAsync(this.UserId(), model);
            return IsDone ? (IActionResult)Ok(Message) : BadRequest(Message);
        }
    }
}
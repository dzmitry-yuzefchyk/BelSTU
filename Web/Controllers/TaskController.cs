using BusinessLogic.Models.Task;
using BusinessLogic.Services.Interfaces;
using CommonLogic.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask(CreateTaskModel model)
        {
            var (IsDone, Message) = await _taskService.CreateTaskAsync(this.UserId(), model);
            return IsDone ? (IActionResult)Ok(Message) : BadRequest(Message);
        }

        [HttpPost("Move")]
        public async Task<IActionResult> MoveTask(int projectId, int taskId, int columnId)
        {
            var result = await _taskService.MoveTaskAsync(this.UserId(), projectId, taskId, columnId);
            return result ? (IActionResult)Ok(result) : BadRequest("Something went wrong, please try again later");
        }

        [HttpGet("{taskId}")]
        public async Task<IActionResult> GetTask(int taskId, int projectId)
        {
            var result = await _taskService.GetTaskAsync(this.UserId(), taskId, projectId);
            return result != null ? (IActionResult)Ok(result) : BadRequest("Something went wrong, please try again later");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTask(UpdateTaskModel model)
        {
            var result = await _taskService.UpdateTaskAsync(this.UserId(), model);
            return result ? (IActionResult)Ok(result) : BadRequest("Something went wrong, please try again later");
        }

        [HttpDelete("{taskId}")]
        public async Task<IActionResult> DeleteTask(int taskId, int projectId)
        {
            var result = await _taskService.DeleteTaskAsync(this.UserId(), taskId, projectId);
            return result ? (IActionResult)Ok(result) : BadRequest("Something went wrong, please try again later");
        }

        [HttpPost("Comment")]
        public async Task<IActionResult> LeaveComment(CreateCommentModel model)
        {
            var result = await _taskService.LeaveCommentAsync(this.UserId(), model);
            return result ? (IActionResult)Ok(result) : BadRequest("Something went wrong, please try again later");
        }
    }
}
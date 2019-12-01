using BusinessLogic.AdvancedSecurity;
using BusinessLogic.Models.Project;
using BusinessLogic.Services.Interfaces;
using CommonLogic.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Web.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly ISecurityService _securityService;

        public ProjectController(IProjectService projectService, ISecurityService securityService)
        {
            _projectService = projectService;
            _securityService = securityService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject(CreateProjectModel model)
        {
            var (IsDone, Message) = await _projectService.CreateProjectAsync(this.UserId(), model);
            return IsDone ? (IActionResult)Ok(Message) : BadRequest(Message);
        }

        [HttpGet]
        public IActionResult GetProjects(int page, int size)
        {
            var result = _projectService.GetProjects(this.UserId(), page, size);
            return result != null ? (IActionResult)Ok(result) : BadRequest("Something went wrong, please try again later");
        }

        [HttpGet("{projectId}")]
        public async Task<IActionResult> GetProject(int projectId)
        {
            var result = await _projectService.GetProjectAsync(this.UserId(), projectId);
            return Ok(result);
        }

        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUserToProject([FromBody]AddUserModel model)
        {
            var (IsDone, Message) = await _projectService.AddUserToProjectAsync(this.UserId(), model);
            return IsDone ? (IActionResult)Ok(Message) : BadRequest(Message);
        }

        [HttpDelete("RemoveUser")]
        public async Task<IActionResult> RemoveUserFromProject([FromBody]AddUserModel model)
        {
            var (IsDone, Message) = await _projectService.RemoveUserFromProjectAsync(this.UserId(), model);
            return IsDone ? (IActionResult)Ok(Message) : BadRequest(Message);
        }

        [HttpPut("Settings")]
        public async Task<IActionResult> UpdateSettings([FromBody]UpdateProjectModel model)
        {
            var (IsDone, Message) = await _projectService.UpdateSettingsAsync(this.UserId(), model);
            return IsDone ? (IActionResult)Ok(Message) : BadRequest(Message);
        }

        [HttpGet("GeneralSettings")]
        public async Task<IActionResult> GetSettings(int projectId)
        {
            var result = await _projectService.GetSettingsAsync(this.UserId(), projectId);
            return result != null ? (IActionResult)Ok(result) : BadRequest(result);
        }

        [HttpGet("AccessSettings")]
        public IActionResult GetAccessSettings(int projectId, int page)
        {
            var result = _securityService.GetUserAccesses(projectId, page, 20);
            return result != null ? (IActionResult)Ok(result) : BadRequest(result);
        }

        [HttpPut("AccessSettings")]
        public async Task<IActionResult> UpdateAccessSettings(UpdateSecurityModel model)
        {
            var (IsDone, Message) = await _securityService.UpdateAsync(this.UserId(), model);
            return IsDone ? (IActionResult)Ok(Message) : BadRequest(Message);
        }
    }
}
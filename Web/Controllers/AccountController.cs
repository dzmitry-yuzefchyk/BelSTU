using BusinessLogic.Models.Account;
using BusinessLogic.Services.Interfaces;
using CommonLogic.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp([FromBody]RegistrationModel model)
        {
            var clientAppHost = HttpContext.Request.Host.ToString();
            var (IsDone, Message) = await _accountService.SignUpAsync(model, clientAppHost);
            return IsDone ? (IActionResult)Ok(Message) : BadRequest(Message);
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn([FromBody]LoginModel model)
        {
            var (IsDone, Message, token) = await _accountService.SignInAsync(model);
            if (!IsDone)
            {
                return Unauthorized(Message);
            }

            this.SetCookieSecurityToken(token);
            return Ok(Message);
        }

        [HttpPost("SignOut")]
        public async Task<IActionResult> SignOut()
        {
            var result = await _accountService.SignOutAsync();
            if (!result)
            {
                return Unauthorized();
            }

            this.ResetCookieSecurityToken();
            return Ok();
        }

        [HttpPost("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            var (IsDone, Message) = await _accountService.ConfirmEmailAsync(email, token);
            return IsDone ? (IActionResult)Ok(Message) : BadRequest(Message);
        }

        [HttpPost("ResendEmail")]
        public async Task<IActionResult> ResendEmail(string email)
        {
            var clientAppHost = HttpContext.Request.Host.ToString();
            var (IsDone, Message) = await _accountService.SendConfirmEmailAsync(email, clientAppHost);
            return IsDone ? (IActionResult)Ok(Message) : BadRequest(Message);
        }

        [Authorize]
        [HttpPut("Profile")]
        public async Task<IActionResult> UpdateProfile([FromBody]UpdateProfileModel model)
        {
            var userId = this.UserId();
            var (IsDone, Message) = await _accountService.UpdateProfileAsync(userId, model);
            return IsDone ? (IActionResult)Ok(Message) : BadRequest(Message);
        }

        [Authorize]
        [HttpGet("Profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = this.UserId();
            var (Model, Message) = await _accountService.GetProfile(userId);
            return Model == null ? (IActionResult)BadRequest(Message) : Ok(Model);
        }

        [Authorize]
        [HttpPut("Settings")]
        public async Task<IActionResult> UpdateSettings([FromBody]UpdateSettingsModel model)
        {
            var userId = this.UserId();
            var (IsDone, Message) = await _accountService.UpdateSettingsAsync(userId, model);
            return IsDone ? (IActionResult)Ok(Message) : BadRequest(Message);
        }

        [Authorize]
        [HttpGet("Settings")]
        public async Task<IActionResult> GetSettings()
        {
            var userId = this.UserId();
            var (Model, Message) = await _accountService.GetSettings(userId);
            return Model == null ? (IActionResult)BadRequest(Message) : Ok(Model);
        }

        [HttpGet("IsAuthenticated")]
        public async Task<IActionResult> IsAuthorized()
        {
            return Ok(User.Identity.IsAuthenticated);
        }

    }
}
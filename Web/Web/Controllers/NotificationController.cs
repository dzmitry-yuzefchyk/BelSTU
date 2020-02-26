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
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetNotifications()
        {
            var result = await _notificationService.GetNotificationsAsync(this.UserId());
            return result != null ? (IActionResult)Ok(result) : BadRequest("Something went wrong, please try again lager");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteNotifications()
        {
            var result = await _notificationService.ClearAllAsync(this.UserId());
            return result ? (IActionResult)Ok(result) : BadRequest(result);
        }

    }
}
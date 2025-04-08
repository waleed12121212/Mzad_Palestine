using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mzad_Palestine_Core.DTO_s.Notification;
using Mzad_Palestine_Core.Interfaces.Services;

namespace Mzad_Palestine_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService) => _notificationService = notificationService;

        [HttpGet("user/{userId:int}")]
        public async Task<IActionResult> GetUserNotifications(int userId)
        {
            IEnumerable<NotificationDto> notifications = await _notificationService.GetUserNotificationsAsync(userId);
            return Ok(notifications);
        }
    }
}

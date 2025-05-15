using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mzad_Palestine_Core.DTO_s.Notification;
using Mzad_Palestine_Core.Interfaces.Services;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Mzad_Palestine_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly ILogger<NotificationController> _logger;

        public NotificationController(INotificationService notificationService , ILogger<NotificationController> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
            _jsonOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve ,
                MaxDepth = 32
            };
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetUserNotifications( )
        {
            try
            {
                _logger.LogInformation("Getting user notifications");
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer " , "");
                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogWarning("No authorization token provided");
                    return Unauthorized(new { success = false , error = "الرجاء تسجيل الدخول" });
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("User ID not found in token");
                    return Unauthorized(new { success = false , error = "المستخدم غير موجود" });
                }

                if (!int.TryParse(userId , out int parsedUserId))
                {
                    _logger.LogWarning("Invalid user ID format");
                    return BadRequest(new { success = false , error = "معرف المستخدم غير صالح" });
                }

                var notifications = await _notificationService.GetUserNotificationsAsync(parsedUserId);
                _logger.LogInformation($"Retrieved {notifications.Count()} notifications for user {parsedUserId}");
                return Ok(new { success = true , data = notifications });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex , "Error getting user notifications");
                return BadRequest(new { success = false , error = ex.Message });
            }
        }

        [HttpPut("{id:int}/read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            try
            {
                _logger.LogInformation($"Marking notification {id} as read");
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer " , "");
                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogWarning("No authorization token provided");
                    return Unauthorized(new { success = false , error = "الرجاء تسجيل الدخول" });
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("User ID not found in token");
                    return Unauthorized(new { success = false , error = "المستخدم غير موجود" });
                }

                if (!int.TryParse(userId , out int parsedUserId))
                {
                    _logger.LogWarning("Invalid user ID format");
                    return BadRequest(new { success = false , error = "معرف المستخدم غير صالح" });
                }

                var success = await _notificationService.MarkAsReadAsync(id , parsedUserId);
                if (!success)
                {
                    _logger.LogWarning($"Failed to mark notification {id} as read for user {parsedUserId}");
                    return NotFound(new { success = false , error = "الإشعار غير موجود" });
                }

                _logger.LogInformation($"Successfully marked notification {id} as read for user {parsedUserId}");
                return Ok(new { success = true , message = "تم تحديث حالة الإشعار بنجاح" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex , $"Error marking notification {id} as read");
                return BadRequest(new { success = false , error = ex.Message });
            }
        }

        [HttpPut("read-all")]
        public async Task<IActionResult> MarkAllAsRead( )
        {
            try
            {
                _logger.LogInformation("Marking all notifications as read");
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer " , "");
                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogWarning("No authorization token provided");
                    return Unauthorized(new { success = false , error = "الرجاء تسجيل الدخول" });
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("User ID not found in token");
                    return Unauthorized(new { success = false , error = "المستخدم غير موجود" });
                }

                if (!int.TryParse(userId , out int parsedUserId))
                {
                    _logger.LogWarning("Invalid user ID format");
                    return BadRequest(new { success = false , error = "معرف المستخدم غير صالح" });
                }

                await _notificationService.MarkAllAsReadAsync(parsedUserId);
                _logger.LogInformation($"Successfully marked all notifications as read for user {parsedUserId}");
                return Ok(new { success = true , message = "تم تحديث حالة جميع الإشعارات بنجاح" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex , "Error marking all notifications as read");
                return BadRequest(new { success = false , error = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting notification {id}");
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer " , "");
                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogWarning("No authorization token provided");
                    return Unauthorized(new { success = false , error = "الرجاء تسجيل الدخول" });
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("User ID not found in token");
                    return Unauthorized(new { success = false , error = "المستخدم غير موجود" });
                }

                if (!int.TryParse(userId , out int parsedUserId))
                {
                    _logger.LogWarning("Invalid user ID format");
                    return BadRequest(new { success = false , error = "معرف المستخدم غير صالح" });
                }

                var success = await _notificationService.DeleteAsync(id , parsedUserId);
                if (!success)
                {
                    _logger.LogWarning($"Failed to delete notification {id} for user {parsedUserId}");
                    return NotFound(new { success = false , error = "الإشعار غير موجود" });
                }

                _logger.LogInformation($"Successfully deleted notification {id} for user {parsedUserId}");
                return Ok(new { success = true , message = "تم حذف الإشعار بنجاح" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex , $"Error deleting notification {id}");
                return BadRequest(new { success = false , error = ex.Message });
            }
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> ClearAllNotifications( )
        {
            try
            {
                _logger.LogInformation("Clearing all notifications");
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer " , "");
                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogWarning("No authorization token provided");
                    return Unauthorized(new { success = false , error = "الرجاء تسجيل الدخول" });
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("User ID not found in token");
                    return Unauthorized(new { success = false , error = "المستخدم غير موجود" });
                }

                if (!int.TryParse(userId , out int parsedUserId))
                {
                    _logger.LogWarning("Invalid user ID format");
                    return BadRequest(new { success = false , error = "معرف المستخدم غير صالح" });
                }

                await _notificationService.ClearAllAsync(parsedUserId);
                _logger.LogInformation($"Successfully cleared all notifications for user {parsedUserId}");
                return Ok(new { success = true , message = "تم حذف جميع الإشعارات بنجاح" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex , "Error clearing all notifications");
                return BadRequest(new { success = false , error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationDto dto)
        {
            try
            {
                _logger.LogInformation($"Creating notification for user {dto.UserId}");
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogWarning("No authorization token provided");
                    return Unauthorized(new { success = false, error = "الرجاء تسجيل الدخول" });
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("User ID not found in token");
                    return Unauthorized(new { success = false, error = "المستخدم غير موجود" });
                }

                if (!int.TryParse(userId, out int parsedUserId))
                {
                    _logger.LogWarning("Invalid user ID format");
                    return BadRequest(new { success = false, error = "معرف المستخدم غير صالح" });
                }

                var notification = await _notificationService.CreateNotificationAsync(dto);
                _logger.LogInformation($"Successfully created notification for user {dto.UserId}");
                
                return Ok(new { success = true, data = notification });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error creating notification for user {dto.UserId}");
                return BadRequest(new { success = false, error = ex.Message });
            }
        }
    }
}

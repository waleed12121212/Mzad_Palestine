using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mzad_Palestine_Core.DTO_s.Message;
using Mzad_Palestine_Core.Interfaces.Services;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mzad_Palestine_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly JsonSerializerOptions _jsonOptions;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
            _jsonOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                MaxDepth = 32
            };
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMessageDto dto)
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { success = false, error = "الرجاء تسجيل الدخول" });
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { success = false, error = "المستخدم غير موجود" });
                }

                if (!int.TryParse(userId, out int parsedUserId))
                {
                    return BadRequest(new { success = false, error = "معرف المستخدم غير صالح" });
                }

                if (string.IsNullOrWhiteSpace(dto.Subject))
                {
                    return BadRequest(new { success = false, error = "عنوان الرسالة مطلوب" });
                }

                if (string.IsNullOrWhiteSpace(dto.Content))
                {
                    return BadRequest(new { success = false, error = "محتوى الرسالة مطلوب" });
                }

                if (dto.ReceiverId == parsedUserId)
                {
                    return BadRequest(new { success = false, error = "لا يمكنك إرسال رسالة لنفسك" });
                }

                dto.SenderId = parsedUserId;
                var message = await _messageService.SendAsync(dto);
                return CreatedAtAction(nameof(GetInbox), null, new { success = true, data = message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpGet("inbox")]
        public async Task<IActionResult> GetInbox()
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { success = false, error = "الرجاء تسجيل الدخول" });
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { success = false, error = "المستخدم غير موجود" });
                }

                if (!int.TryParse(userId, out int parsedUserId))
                {
                    return BadRequest(new { success = false, error = "معرف المستخدم غير صالح" });
                }

                var messages = await _messageService.GetInboxAsync(parsedUserId);
                return Ok(new { success = true, data = messages });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpGet("sent")]
        public async Task<IActionResult> GetSentMessages()
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { success = false, error = "الرجاء تسجيل الدخول" });
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { success = false, error = "المستخدم غير موجود" });
                }

                if (!int.TryParse(userId, out int parsedUserId))
                {
                    return BadRequest(new { success = false, error = "معرف المستخدم غير صالح" });
                }

                var messages = await _messageService.GetSentMessagesAsync(parsedUserId);
                return Ok(new { success = true, data = messages });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpGet("conversation/{userId:int}")]
        public async Task<IActionResult> GetConversation(int userId)
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { success = false, error = "الرجاء تسجيل الدخول" });
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var currentUserId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(currentUserId))
                {
                    return Unauthorized(new { success = false, error = "المستخدم غير موجود" });
                }

                if (!int.TryParse(currentUserId, out int parsedCurrentUserId))
                {
                    return BadRequest(new { success = false, error = "معرف المستخدم غير صالح" });
                }

                if (userId == parsedCurrentUserId)
                {
                    return BadRequest(new { success = false, error = "لا يمكنك عرض محادثة مع نفسك" });
                }

                var messages = await _messageService.GetConversationAsync(parsedCurrentUserId, userId);
                return Ok(new { success = true, data = messages });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpPut("{id:int}/read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { success = false, error = "الرجاء تسجيل الدخول" });
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { success = false, error = "المستخدم غير موجود" });
                }

                if (!int.TryParse(userId, out int parsedUserId))
                {
                    return BadRequest(new { success = false, error = "معرف المستخدم غير صالح" });
                }

                var success = await _messageService.MarkAsReadAsync(id, parsedUserId);
                if (!success)
                {
                    return NotFound(new { success = false, error = "الرسالة غير موجودة أو غير مصرح لك بقراءتها" });
                }

                return Ok(new { success = true, message = "تم تحديث حالة القراءة بنجاح" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }
    }
}

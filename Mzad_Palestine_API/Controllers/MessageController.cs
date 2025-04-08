using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mzad_Palestine_Core.DTO_s.Message;
using Mzad_Palestine_Core.Interfaces.Services;

namespace Mzad_Palestine_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        public MessageController(IMessageService messageService) => _messageService = messageService;

        [HttpPost]
        public async Task<IActionResult> Send([FromBody] CreateMessageDto dto)
        {
            var message = await _messageService.SendAsync(dto);
            return CreatedAtAction(nameof(GetInbox) , new { userId = message.ReceiverId } , message);
        }

        [HttpGet("inbox/{userId:int}")]
        public async Task<IActionResult> GetInbox(int userId)
        {
            IEnumerable<MessageDto> messages = await _messageService.GetInboxAsync(userId);
            return Ok(messages);
        }
    }
}

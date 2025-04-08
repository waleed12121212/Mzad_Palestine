using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mzad_Palestine_Core.DTO_s.Customer_Support;
using Mzad_Palestine_Core.Interfaces.Services;

namespace Mzad_Palestine_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupportController : ControllerBase
    {
        private readonly ISupportService _supportService;
        public SupportController(ISupportService supportService) => _supportService = supportService;

        [HttpPost]
        public async Task<IActionResult> CreateTicket([FromBody] CreateSupportTicketDto dto)
        {
            var ticket = await _supportService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetUserTickets) , new { userId = ticket.UserId } , ticket);
        }

        [HttpGet("user/{userId:int}")]
        public async Task<IActionResult> GetUserTickets(int userId)
        {
            IEnumerable<SupportTicketDto> tickets = await _supportService.GetUserTicketsAsync(userId);
            return Ok(tickets);
        }
    }
}

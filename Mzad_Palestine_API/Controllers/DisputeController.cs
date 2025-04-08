using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mzad_Palestine_Core.DTO_s.Dispute;
using Mzad_Palestine_Core.Interfaces.Services;

namespace Mzad_Palestine_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DisputeController : ControllerBase
    {
        private readonly IDisputeService _disputeService;
        public DisputeController(IDisputeService disputeService) => _disputeService = disputeService;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDisputeDto dto)
        {
            var dispute = await _disputeService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetAll) , new { id = dispute.Id } , dispute);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll( )
        {
            IEnumerable<DisputeDto> disputes = await _disputeService.GetAllAsync();
            return Ok(disputes);
        }
    }
}

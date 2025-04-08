using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mzad_Palestine_Core.DTO_s.AutoBid;
using Mzad_Palestine_Core.Interfaces.Services;

namespace Mzad_Palestine_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutoBidController : ControllerBase
    {
        private readonly IAutoBidService _autoBidService;
        public AutoBidController(IAutoBidService autoBidService) => _autoBidService = autoBidService;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAutoBidDto dto)
        {
            var autoBid = await _autoBidService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById) , new { id = autoBid.Id } , autoBid);
        }
    }
}

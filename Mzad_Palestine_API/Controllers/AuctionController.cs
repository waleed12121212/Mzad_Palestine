using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mzad_Palestine_Core.DTO_s.Auction;
using Mzad_Palestine_Core.Interfaces.Services;

namespace Mzad_Palestine_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionController : ControllerBase
    {
        private readonly IAuctionService _auctionService;
        public AuctionController(IAuctionService auctionService) => _auctionService = auctionService;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAuctionDto dto)
        {
            var auction = await _auctionService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById) , new { id = auction.Id } , auction);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var auction = await _auctionService.GetByIdAsync(id);
            if (auction == null)
                return NotFound();
            return Ok(auction);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActive( )
        {
            IEnumerable<AuctionDto> auctions = await _auctionService.GetActiveAsync();
            return Ok(auctions);
        }
    }
}

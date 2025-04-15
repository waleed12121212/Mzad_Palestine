using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mzad_Palestine_Core.DTO_s.Auction;
using Mzad_Palestine_Core.Interfaces.Services;
using Mzad_Palestine_Core.Models;
using System.Security.Claims;

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
            var auction = new Auction
            {
                ListingId = dto.ListingId,
     
                EndTime = dto.EndTime,
            };
            await _auctionService.CreateAuctionAsync(auction);
            return CreatedAtAction(nameof(GetById), new { id = auction.AuctionId }, auction);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var auction = await _auctionService.GetAuctionDetailsAsync(id);
            if (auction == null)
                return NotFound();
            return Ok(auction);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActive()
        {
            var auctions = await _auctionService.GetOpenAuctionsAsync();
            return Ok(auctions);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAuctionDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var auction = new Auction
            {
                AuctionId = id,
     
            };
            await _auctionService.UpdateAuctionAsync(auction, userId);
            return Ok(auction);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await _auctionService.DeleteAuctionAsync(id, userId);
            return NoContent();
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] AuctionSearchDto searchDto)
        {
            var auctions = await _auctionService.SearchAuctionsAsync(searchDto);
            return Ok(auctions);
        }

        [HttpGet("user/{userId:int}")]
        public async Task<IActionResult> GetUserAuctions(int userId)
        {
            var auctions = await _auctionService.GetUserAuctionsAsync(userId);
            return Ok(auctions);
        }

        [HttpGet("open")]
        public async Task<IActionResult> GetOpenAuctions()
        {
            var auctions = await _auctionService.GetOpenAuctionsAsync();
            return Ok(auctions);
        }

        [HttpGet("closed")]
        public async Task<IActionResult> GetClosedAuctions()
        {
            var auctions = await _auctionService.GetClosedAuctionsAsync();
            return Ok(auctions);
        }

        [HttpGet("{id:int}/bids")]
        public async Task<IActionResult> GetAuctionWithBids(int id)
        {
            var auction = await _auctionService.GetAuctionDetailsAsync(id);
            if (auction == null)
                return NotFound();
            return Ok(auction);
        }

        [HttpPost("{id:int}/close")]
        public async Task<IActionResult> CloseAuction(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await _auctionService.CloseAuctionAsync(id, userId);
            return NoContent();
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mzad_Palestine_Core.DTO_s.Bid;
using Mzad_Palestine_Core.Interfaces.Services;

namespace Mzad_Palestine_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BidController : ControllerBase
    {
        private readonly IBidService _bidService;
        public BidController(IBidService bidService) => _bidService = bidService;

        [HttpPost]
        public async Task<IActionResult> PlaceBid([FromBody] CreateBidDto dto)
        {
            var bid = await _bidService.PlaceBidAsync(dto);
            return Ok(bid);
        }

        [HttpGet("auction/{auctionId:int}")]
        public async Task<IActionResult> GetBidsForAuction(int auctionId)
        {
            IEnumerable<BidDto> bids = await _bidService.GetBidsForAuctionAsync(auctionId);
            return Ok(bids);
        }
    }
}

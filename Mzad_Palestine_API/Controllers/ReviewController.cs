using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mzad_Palestine_Core.DTO_s.Review;
using Mzad_Palestine_Core.Interfaces.Services;

namespace Mzad_Palestine_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        public ReviewController(IReviewService reviewService) => _reviewService = reviewService;

        [HttpPost]
        public async Task<IActionResult> CreateReview([FromBody] CreateReviewDto dto)
        {
            var review = await _reviewService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetByListing) , new { listingId = review.ListingId } , review);
        }

        [HttpGet("listing/{listingId:int}")]
        public async Task<IActionResult> GetByListing(int listingId)
        {
            IEnumerable<ReviewDto> reviews = await _reviewService.GetByListingIdAsync(listingId);
            return Ok(reviews);
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mzad_Palestine_Core.DTO_s.Listing;
using Mzad_Palestine_Core.Interfaces.Services;

namespace Mzad_Palestine_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListingController : ControllerBase
    {
        private readonly IListingService _listingService;
        public ListingController(IListingService listingService) => _listingService = listingService;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateListingDto dto)
        {
            var listing = await _listingService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById) , new { id = listing.Id } , listing);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll( )
        {
            IEnumerable<ListingDto> listings = await _listingService.GetAllAsync();
            return Ok(listings);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var listing = await _listingService.GetByIdAsync(id);
            if (listing == null)
                return NotFound();
            return Ok(listing);
        }

        [HttpGet("user/{userId:int}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            IEnumerable<ListingDto> listings = await _listingService.GetByUserIdAsync(userId);
            return Ok(listings);
        }
    }
}

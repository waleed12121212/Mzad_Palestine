using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Mzad_Palestine_Core.Interfaces.Services;
using Mzad_Palestine_Core.DTO_s.Watchlist;

namespace Mzad_Palestine_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly IWatchlistService _watchlistService;
        private readonly IListingService _listingService;
        private readonly IAuctionService _auctionService;

        public WishlistController(
            IWatchlistService watchlistService,
            IListingService listingService,
            IAuctionService auctionService)
        {
            _watchlistService = watchlistService;
            _listingService = listingService;
            _auctionService = auctionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
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

                var watchlistItems = await _watchlistService.GetByUserIdAsync(int.Parse(userId));
                return Ok(new { success = true, data = watchlistItems });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpPost("listing/{listingId:int}")]
        public async Task<IActionResult> AddListing(int listingId)
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

                // التحقق من وجود القائمة
                var listing = await _listingService.GetByIdAsync(listingId);
                if (listing == null)
                {
                    return NotFound(new { success = false, error = "القائمة غير موجودة" });
                }

                var watchlistItem = await _watchlistService.AddListingAsync(int.Parse(userId), listingId);
                return CreatedAtAction(nameof(GetAll), new { success = true, data = watchlistItem });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpPost("auction/{auctionId:int}")]
        public async Task<IActionResult> AddAuction(int auctionId)
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

                // التحقق من وجود المزاد
                var auction = await _auctionService.GetByIdAsync(auctionId);
                if (auction == null)
                {
                    return NotFound(new { success = false, error = "المزاد غير موجود" });
                }

                var watchlistItem = await _watchlistService.AddAuctionAsync(int.Parse(userId), auctionId);
                return CreatedAtAction(nameof(GetAll), new { success = true, data = watchlistItem });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpDelete("listing/{listingId:int}")]
        public async Task<IActionResult> RemoveListing(int listingId)
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

                await _watchlistService.RemoveListingAsync(int.Parse(userId), listingId);
                return Ok(new { success = true, message = "تم حذف القائمة من المفضلة بنجاح" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpDelete("auction/{auctionId:int}")]
        public async Task<IActionResult> RemoveAuction(int auctionId)
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

                await _watchlistService.RemoveAuctionAsync(int.Parse(userId), auctionId);
                return Ok(new { success = true, message = "تم حذف المزاد من المفضلة بنجاح" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }
    }
} 
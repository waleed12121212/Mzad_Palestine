using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mzad_Palestine_Core.DTO_s.Listing;
using Mzad_Palestine_Core.Interfaces.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Mzad_Palestine_Core.Enums;

namespace Mzad_Palestine_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ListingController : ControllerBase
    {
        private readonly IListingService _listingService;
        public ListingController(IListingService listingService) => _listingService = listingService;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateListingDto dto)
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer " , "");
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { error = "الرجاء تسجيل الدخول" });
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { error = "المستخدم غير موجود" });
                }

                var listing = await _listingService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById) , new { id = listing.Id } , listing);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll( )
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer " , "");
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { error = "الرجاء تسجيل الدخول" });
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { error = "المستخدم غير موجود" });
                }

                IEnumerable<ListingDto> listings = await _listingService.GetAllAsync();
                return Ok(listings);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer " , "");
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { error = "الرجاء تسجيل الدخول" });
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { error = "المستخدم غير موجود" });
                }

                var listing = await _listingService.GetByIdAsync(id);
                if (listing == null)
                    return NotFound(new { error = "العرض غير موجود" });
                return Ok(listing);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("user/{userId:int}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer " , "");
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { error = "الرجاء تسجيل الدخول" });
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var currentUserId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(currentUserId))
                {
                    return Unauthorized(new { error = "المستخدم غير موجود" });
                }

                IEnumerable<ListingDto> listings = await _listingService.GetByUserIdAsync(userId);
                return Ok(listings);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("category/{categoryId:int}")]
        public async Task<IActionResult> GetByCategory(int categoryId)
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer " , "");
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { error = "الرجاء تسجيل الدخول" });
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { error = "المستخدم غير موجود" });
                }

                IEnumerable<ListingDto> listings = await _listingService.GetByCategoryAsync(categoryId);
                return Ok(listings);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActive( )
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer " , "");
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { error = "الرجاء تسجيل الدخول" });
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { error = "المستخدم غير موجود" });
                }

                IEnumerable<ListingDto> listings = await _listingService.GetActiveAsync();
                return Ok(listings);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id , [FromBody] UpdateListingDto dto)
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer " , "");
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { error = "الرجاء تسجيل الدخول" });
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { error = "المستخدم غير موجود" });
                }

                var listing = await _listingService.GetByIdAsync(id);
                if (listing == null)
                    return NotFound(new { error = "العرض غير موجود" });

                if (listing.UserId != int.Parse(userId))
                    return Unauthorized(new { error = "غير مصرح لك بتحديث هذا العرض" });

                var updatedListing = await _listingService.UpdateAsync(id , dto);
                return Ok(updatedListing);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer " , "");
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { error = "الرجاء تسجيل الدخول" });
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { error = "المستخدم غير موجود" });
                }

                var listing = await _listingService.GetByIdAsync(id);
                if (listing == null)
                    return NotFound(new { error = "العرض غير موجود" });

                if (listing.UserId != int.Parse(userId))
                    return Unauthorized(new { error = "غير مصرح لك بحذف هذا العرض" });

                await _listingService.DeleteAsync(id);
                return Ok(new { message = "تم حذف العرض بنجاح" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}

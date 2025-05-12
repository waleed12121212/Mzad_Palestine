using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mzad_Palestine_Core.DTO_s.Review;
using Mzad_Palestine_Core.Interfaces.Services;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mzad_Palestine_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly IListingService _listingService;
        private readonly JsonSerializerOptions _jsonOptions;

        public ReviewController(IReviewService reviewService, IListingService listingService)
        {
            _reviewService = reviewService;
            _listingService = listingService;
            _jsonOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                MaxDepth = 32
            };
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReviewDto dto)
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

                if (!int.TryParse(userId, out int parsedUserId))
                {
                    return BadRequest(new { success = false, error = "معرف المستخدم غير صالح" });
                }

                if (dto.Rating < 1 || dto.Rating > 5)
                {
                    return BadRequest(new { success = false, error = "التقييم يجب أن يكون بين 1 و 5" });
                }

                if (string.IsNullOrWhiteSpace(dto.Comment))
                {
                    return BadRequest(new { success = false, error = "التعليق مطلوب" });
                }

                // التحقق من وجود القائمة
                var listing = await _listingService.GetByIdAsync(dto.ListingId);
                if (listing == null)
                {
                    return NotFound(new { success = false, error = "القائمة غير موجودة" });
                }

                // التحقق من أن المستخدم ليس صاحب القائمة
                if (listing.UserId == parsedUserId)
                {
                    return BadRequest(new { success = false, error = "لا يمكنك تقييم قائمتك الخاصة" });
                }

                dto.ReviewerId = parsedUserId;
                dto.ReviewedUserId = listing.UserId;
                var review = await _reviewService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetByListingId), new { listingId = review.ListingId }, new { success = true, data = review });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpGet("listing/{listingId:int}")]
        public async Task<IActionResult> GetByListingId(int listingId)
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { success = false, error = "الرجاء تسجيل الدخول" });
                }

                // التحقق من وجود القائمة
                var listing = await _listingService.GetByIdAsync(listingId);
                if (listing == null)
                {
                    return NotFound(new { success = false, error = "القائمة غير موجودة" });
                }

                var reviews = await _reviewService.GetByListingIdAsync(listingId);
                return Ok(new { success = true, data = reviews });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpGet("user/{userId:int}")]
        public async Task<IActionResult> GetUserReviews(int userId)
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { success = false, error = "الرجاء تسجيل الدخول" });
                }

                var reviews = await _reviewService.GetByUserIdAsync(userId);
                return Ok(new { success = true, data = reviews });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpGet("my-reviews")]
        public async Task<IActionResult> GetMyReviews()
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

                if (!int.TryParse(userId, out int parsedUserId))
                {
                    return BadRequest(new { success = false, error = "معرف المستخدم غير صالح" });
                }

                var reviews = await _reviewService.GetByReviewerIdAsync(parsedUserId);
                return Ok(new { success = true, data = reviews });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpGet("received-reviews")]
        public async Task<IActionResult> GetReceivedReviews()
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

                if (!int.TryParse(userId, out int parsedUserId))
                {
                    return BadRequest(new { success = false, error = "معرف المستخدم غير صالح" });
                }

                var reviews = await _reviewService.GetByUserIdAsync(parsedUserId);
                return Ok(new { success = true, data = reviews });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpGet("average/{userId:int}")]
        public async Task<IActionResult> GetUserAverageRating(int userId)
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { success = false, error = "الرجاء تسجيل الدخول" });
                }

                var reviews = await _reviewService.GetByListingIdAsync(userId);
                var userReviews = reviews.Where(r => r.ReviewedUserId == userId);

                if (!userReviews.Any())
                {
                    return Ok(new { success = true, data = new { averageRating = 0, totalReviews = 0 } });
                }

                var averageRating = userReviews.Average(r => r.Rating);
                var totalReviews = userReviews.Count();

                return Ok(new { success = true, data = new { averageRating, totalReviews } });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpGet("{listingId}/average")]
        public async Task<IActionResult> GetListingAverageRating(int listingId)
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { success = false, error = "الرجاء تسجيل الدخول" });
                }

                // التحقق من وجود القائمة
                var listing = await _listingService.GetByIdAsync(listingId);
                if (listing == null)
                {
                    return NotFound(new { success = false, error = "القائمة غير موجودة" });
                }

                var (averageRating, totalReviews) = await _reviewService.GetListingAverageRatingAsync(listingId);
                return Ok(new { success = true, data = new { averageRating, totalReviews } });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> EditReview(int id, [FromBody] UpdateReviewDto dto)
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

                if (!int.TryParse(userId, out int parsedUserId))
                {
                    return BadRequest(new { success = false, error = "معرف المستخدم غير صالح" });
                }

                if (dto.Rating < 1 || dto.Rating > 5)
                {
                    return BadRequest(new { success = false, error = "التقييم يجب أن يكون بين 1 و 5" });
                }

                if (string.IsNullOrWhiteSpace(dto.Comment))
                {
                    return BadRequest(new { success = false, error = "التعليق مطلوب" });
                }

                var review = await _reviewService.EditAsync(id, dto);
                return Ok(new { success = true, data = review });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteReview(int id)
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

                if (!int.TryParse(userId, out int parsedUserId))
                {
                    return BadRequest(new { success = false, error = "معرف المستخدم غير صالح" });
                }

                var success = await _reviewService.DeleteAsync(id);
                if (!success)
                {
                    return NotFound(new { success = false, error = "المراجعة غير موجودة" });
                }

                return Ok(new { success = true, message = "تم حذف المراجعة بنجاح" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }
    }
}

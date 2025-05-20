using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mzad_Palestine_Core.DTO_s.Bid;
using Mzad_Palestine_Core.Interfaces.Services;
using Mzad_Palestine_Core.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;

namespace Mzad_Palestine_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BidController : ControllerBase
    {
        private readonly IBidService _bidService;
        private readonly IAuctionService _auctionService;
        private readonly JsonSerializerOptions _jsonOptions;

        public BidController(IBidService bidService , IAuctionService auctionService)
        {
            _bidService = bidService;
            _auctionService = auctionService;
            _jsonOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve ,
                MaxDepth = 32
            };
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBidDto dto)
        {
            Console.WriteLine("CreateBid endpoint hit"); // Debug log
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        success = false ,
                        error = "بيانات العرض غير صالحة" ,
                        details = ModelState.Values
                            .SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage)
                    });
                }

                // Get token from request headers
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer " , "");
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { success = false , error = "الرجاء تسجيل الدخول" });
                }

                // Extract user ID from token
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { success = false , error = "المستخدم غير موجود" });
                }

                if (!int.TryParse(userId , out int parsedUserId))
                {
                    return BadRequest(new { success = false , error = "معرف المستخدم غير صالح" });
                }

                // Get the auction
                var auction = await _auctionService.GetAuctionDetailsAsync(dto.AuctionId);
                if (auction == null)
                {
                    return NotFound(new { success = false , error = "المزاد غير موجود" });
                }

                // التحقق من حالة المزاد
                if (auction.Status != Mzad_Palestine_Core.Enums.AuctionStatus.Open.ToString())
                {
                    return BadRequest(new { success = false , error = "المزاد مغلق" });
                }

                // التحقق من وقت المزاد
                if (auction.EndDate <= DateTime.UtcNow)
                {
                    return BadRequest(new { success = false , error = "المزاد منتهي" });
                }

                // التحقق من قيمة العرض
                if (dto.BidAmount <= 0)
                {
                    return BadRequest(new { success = false , error = "يجب أن يكون مبلغ العرض أكبر من صفر" });
                }

                // التحقق من أن العرض أكبر من العرض الحالي
                if (auction.CurrentBid >= dto.BidAmount)
                {
                    return BadRequest(new { success = false , error = "يجب أن يكون العرض أعلى من السعر الحالي" });
                }

                // التحقق من الحد الأدنى للزيادة
                var minimumBidAmount = auction.CurrentBid + auction.BidIncrement;
                if (dto.BidAmount < minimumBidAmount)
                {
                    return BadRequest(new { success = false , error = $"يجب أن يكون العرض أعلى بمقدار {auction.BidIncrement} على الأقل" });
                }

                // التحقق من أن المزايد ليس مالك المزاد
                if (auction.UserId == parsedUserId)
                {
                    return BadRequest(new { success = false , error = "لا يمكن للمالك المزايدة على مزاده" });
                }

                var bid = new Bid
                {
                    AuctionId = dto.AuctionId ,
                    UserId = parsedUserId ,
                    BidAmount = dto.BidAmount ,
                    BidTime = DateTime.UtcNow ,
                    Status = Mzad_Palestine_Core.Enums.BidStatus.Pending ,
                    IsAutoBid = false ,
                    IsWinner = false
                };

                var result = await _bidService.CreateBidAsync(bid);
                return Ok(new { success = true , message = "تم إضافة العرض بنجاح" , data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false , error = ex.Message });
            }
        }

        [HttpGet("auction/{auctionId:int}")]
        public async Task<IActionResult> GetAuctionBids(int auctionId)
        {
            try
            {
                // Using hardcoded user ID
                int parsedUserId = 1;

                // التحقق من وجود المزاد
                var auction = await _auctionService.GetAuctionDetailsAsync(auctionId);
                if (auction == null)
                {
                    return NotFound(new { success = false , error = "المزاد غير موجود" });
                }

                var bids = await _bidService.GetAuctionBidsAsync(auctionId);
                return Ok(new { success = true , data = bids });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false , error = ex.Message });
            }
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetUserBids( )
        {
            try
            {
                // Using hardcoded user ID
                int parsedUserId = 1;

                var bids = await _bidService.GetUserBidsAsync(parsedUserId);
                return Ok(new { success = true , data = bids });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false , error = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                // Using hardcoded user ID
                int parsedUserId = 1;

                await _bidService.DeleteBidAsync(id , parsedUserId);
                return Ok(new { success = true , message = "تم حذف العرض بنجاح" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false , error = ex.Message });
            }
        }
    }
}

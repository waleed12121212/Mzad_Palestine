using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mzad_Palestine_Core.DTO_s.Auction;
using Mzad_Palestine_Core.Interfaces.Services;
using Mzad_Palestine_Core.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Mzad_Palestine_Core.Enums;
using System.Text.Json.Serialization;
using System.Text.Json;
using AutoMapper;

namespace Mzad_Palestine_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuctionController : ControllerBase
    {
        private readonly IAuctionService _auctionService;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly IMapper _mapper;

        public AuctionController(IAuctionService auctionService , IMapper mapper)
        {
            _auctionService = auctionService;
            _mapper = mapper;
            _jsonOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve ,
                MaxDepth = 32
            };
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAuctionDto dto)
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer " , "");
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { success = false , error = " الرجاء تسجيل الدخول " });
                }

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

                if (dto.EndDate <= DateTime.UtcNow)
                {
                    return BadRequest(new { success = false , error = "وقت انتهاء المزاد يجب أن يكون في المستقبل" });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        success = false ,
                        error = "البيانات غير صالحة" ,
                        details = ModelState.Values
                            .SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage)
                    });
                }

                // تمرير UserId مع الداتا
                dto.UserId = parsedUserId;

                var result = await _auctionService.CreateAsync(dto);
                var auctionDto = _mapper.Map<AuctionDto>(result);
                return CreatedAtAction(
                    nameof(GetById) ,
                    new { id = result.AuctionId } ,
                    new { success = true , data = auctionDto }
                );
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false , error = ex.Message });
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

                var auction = await _auctionService.GetAuctionDetailsAsync(id);
                if (auction == null)
                    return NotFound(new { error = "المزاد غير موجود" });

                var auctionDto = _mapper.Map<AuctionDto>(auction);
                auctionDto.Bids = _mapper.Map<List<BidDto>>(auction.Bids);
                return Ok(new { success = true , data = auctionDto });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActive()
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer " , "");
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { success = false , error = "الرجاء تسجيل الدخول" });
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { success = false , error = "المستخدم غير موجود" });
                }

                var auctions = await _auctionService.GetOpenAuctionsAsync();
                var auctionDtos = _mapper.Map<IEnumerable<AuctionDto>>(auctions);
                return Ok(new { success = true , data = auctionDtos });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false , error = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id , [FromBody] UpdateAuctionDto dto)
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer " , "");
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { success = false , error = "الرجاء تسجيل الدخول" });
                }

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

                var auction = await _auctionService.GetAuctionDetailsAsync(id);
                if (auction == null)
                    return NotFound(new { success = false , error = "المزاد غير موجود" });

                if (auction.UserId != parsedUserId)
                    return Unauthorized(new { success = false , error = "غير مصرح لك بتحديث هذا المزاد" });

                var updatedAuction = new Auction
                {
                    AuctionId = id ,
                    Title = dto.Title ?? auction.Title ,
                    Description = dto.Description ?? auction.Description ,
                    Address = dto.Address ?? auction.Address ,
                    StartDate = dto.StartDate ?? auction.StartDate ,
                    EndDate = dto.EndDate ?? auction.EndDate ,
                    ReservePrice = dto.ReservePrice ?? auction.ReservePrice ,
                    BidIncrement = dto.BidIncrement ?? auction.BidIncrement ,
                    CategoryId = dto.CategoryId ?? auction.CategoryId ,
                    UserId = auction.UserId ,
                    Status = dto.Status?.ToString() ?? auction.Status ,
                    CreatedAt = auction.CreatedAt ,
                    UpdatedAt = DateTime.UtcNow
                };

                await _auctionService.UpdateAuctionAsync(updatedAuction , parsedUserId);

                var updatedDetails = await _auctionService.GetAuctionDetailsAsync(id);
                var updatedAuctionDto = _mapper.Map<AuctionDto>(updatedDetails);
                return Ok(new { success = true , data = updatedAuctionDto });
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

                var auction = await _auctionService.GetAuctionDetailsAsync(id);
                if (auction == null)
                    return NotFound(new { error = "المزاد غير موجود" });

                if (auction.UserId != int.Parse(userId))
                    return Unauthorized(new { error = "غير مصرح لك بحذف هذا المزاد" });

                await _auctionService.DeleteAuctionAsync(id , int.Parse(userId));
                return Ok(new { success = true , message = "تم حذف المزاد بنجاح" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] AuctionSearchDto searchDto)
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer " , "");
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { success = false , error = "الرجاء تسجيل الدخول" });
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { success = false , error = "المستخدم غير موجود" });
                }

                var auctions = await _auctionService.SearchAuctionsAsync(searchDto);
                var auctionDtos = _mapper.Map<IEnumerable<AuctionDto>>(auctions);
                return Ok(new { success = true , data = auctionDtos });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false , error = ex.Message });
            }
        }

        [HttpGet("user/{userId:int}")]
        public async Task<IActionResult> GetUserAuctions(int userId)
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer " , "");
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { success = false , error = "الرجاء تسجيل الدخول" });
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var currentUserId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(currentUserId))
                {
                    return Unauthorized(new { success = false , error = "المستخدم غير موجود" });
                }

                var auctions = await _auctionService.GetUserAuctionsAsync(userId);
                var auctionDtos = _mapper.Map<IEnumerable<AuctionDto>>(auctions);
                return Ok(new { success = true , data = auctionDtos });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false , error = ex.Message });
            }
        }

        [HttpGet("open")]
        public async Task<IActionResult> GetOpenAuctions( )
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer " , "");
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { success = false , error = "الرجاء تسجيل الدخول" });
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { success = false , error = "المستخدم غير موجود" });
                }

                var auctions = await _auctionService.GetOpenAuctionsAsync();
                var auctionDtos = _mapper.Map<IEnumerable<AuctionDto>>(auctions);
                return Ok(new { success = true , data = auctionDtos });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false , error = ex.Message });
            }
        }

        [HttpGet("closed")]
        public async Task<IActionResult> GetClosedAuctions( )
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

                var auctions = await _auctionService.GetClosedAuctionsAsync();
                var auctionDtos = _mapper.Map<IEnumerable<AuctionDto>>(auctions);
                return Ok(new { success = true , data = auctionDtos });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{id:int}/bids")]
        public async Task<IActionResult> GetAuctionWithBids(int id)
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

                var auction = await _auctionService.GetAuctionDetailsAsync(id);
                if (auction == null)
                    return NotFound(new { error = "المزاد غير موجود" });

                var auctionDto = _mapper.Map<AuctionDto>(auction);
                return Ok(new { success = true , data = auctionDto });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("{id:int}/close")]
        public async Task<IActionResult> CloseAuction(int id)
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer " , "");
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { success = false , error = "الرجاء تسجيل الدخول" });
                }

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

                var auction = await _auctionService.GetAuctionDetailsAsync(id);
                if (auction == null)
                    return NotFound(new { success = false , error = "المزاد غير موجود" });

                if (auction.UserId != parsedUserId)
                    return Unauthorized(new { success = false , error = "غير مصرح لك بإغلاق هذا المزاد" });

                await _auctionService.CloseAuctionAsync(id , parsedUserId);

                var updatedAuction = await _auctionService.GetAuctionDetailsAsync(id);
                var updatedAuctionDto = _mapper.Map<AuctionDto>(updatedAuction);
                return Ok(new { success = true , message = "تم إغلاق المزاد بنجاح" , data = updatedAuctionDto });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false , error = ex.Message });
            }
        }

        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingAuctions( )
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer " , "");
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { success = false , error = "الرجاء تسجيل الدخول" });
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { success = false , error = "المستخدم غير موجود" });
                }

                var auctions = await _auctionService.GetPendingAuctionsAsync();
                var auctionDtos = _mapper.Map<IEnumerable<AuctionDto>>(auctions);
                return Ok(new { success = true , data = auctionDtos });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false , error = ex.Message });
            }
        }

        [HttpGet("completed")]
        public async Task<IActionResult> GetCompletedAuctions( )
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer " , "");
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { success = false , error = "الرجاء تسجيل الدخول" });
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { success = false , error = "المستخدم غير موجود" });
                }

                var auctions = await _auctionService.GetCompletedAuctionsAsync();
                var auctionDtos = _mapper.Map<IEnumerable<AuctionDto>>(auctions);
                return Ok(new { success = true , data = auctionDtos });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false , error = ex.Message });
            }
        }
    }
}

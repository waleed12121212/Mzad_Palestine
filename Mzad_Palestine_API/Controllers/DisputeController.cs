using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mzad_Palestine_Core.DTO_s.Dispute;
using Mzad_Palestine_Core.Interfaces.Services;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using System.Text.Json.Serialization;
using Mzad_Palestine_Core.Enums;

namespace Mzad_Palestine_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DisputeController : ControllerBase
    {
        private readonly IDisputeService _disputeService;
        private readonly IAuctionService _auctionService;
        private readonly JsonSerializerOptions _jsonOptions;

        public DisputeController(IDisputeService disputeService, IAuctionService auctionService)
        {
            _disputeService = disputeService;
            _auctionService = auctionService;
            _jsonOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                MaxDepth = 32
            };
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDisputeDto dto)
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

                if (string.IsNullOrWhiteSpace(dto.Reason))
                {
                    return BadRequest(new { success = false, error = "سبب النزاع مطلوب" });
                }

                // التحقق من وجود المزاد
                var auction = await _auctionService.GetAuctionWithBidsAsync(dto.AuctionId);
                if (auction == null)
                {
                    return NotFound(new { success = false, error = "المزاد غير موجود" });
                }

                // التحقق من أن المستخدم ليس صاحب المزاد وأنه مشارك في المزاد
                if (auction.UserId == parsedUserId)
                {
                    return BadRequest(new { success = false, error = "لا يمكن لصاحب المزاد إنشاء نزاع على مزاده" });
                }

                // التحقق من أن المستخدم مشارك في المزاد
                if (!auction.Bids.Any(b => b.UserId == parsedUserId))
                {
                    return BadRequest(new { success = false, error = "يجب أن تكون مشارك في المزاد لإنشاء نزاع" });
                }

                dto.UserId = parsedUserId;
                var dispute = await _disputeService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetAll), null, new { success = true, data = dispute });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
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

                // التحقق من أن المستخدم مسؤول
                var isAdmin = jwtToken.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == "Admin");
                if (!isAdmin)
                {
                    return Unauthorized(new { success = false, error = "غير مصرح لك بعرض جميع النزاعات" });
                }

                var disputes = await _disputeService.GetAllAsync();
                return Ok(new { success = true, data = disputes });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetUserDisputes()
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

                var disputes = await _disputeService.GetAllAsync();
                var userDisputes = disputes.Where(d => d.UserId == parsedUserId);
                return Ok(new { success = true, data = userDisputes });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpGet("auction/{auctionId:int}")]
        public async Task<IActionResult> GetAuctionDisputes(int auctionId)
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

                // التحقق من وجود المزاد
                var auction = await _auctionService.GetByIdAsync(auctionId);
                if (auction == null)
                {
                    return NotFound(new { success = false, error = "المزاد غير موجود" });
                }

                // التحقق من أن المستخدم مسؤول أو مشارك في المزاد
                var isAdmin = jwtToken.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == "Admin");
                if (!isAdmin && auction.UserId != parsedUserId && !auction.Bids.Any(b => b.UserId == parsedUserId))
                {
                    return Unauthorized(new { success = false, error = "غير مصرح لك بعرض نزاعات هذا المزاد" });
                }

                var disputes = await _disputeService.GetAllAsync();
                var auctionDisputes = disputes.Where(d => d.AuctionId == auctionId);
                return Ok(new { success = true, data = auctionDisputes });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpPut("{id:int}/resolve")]
        public async Task<IActionResult> ResolveDispute(int id, [FromBody] string resolution)
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
                
                // التحقق من أن المستخدم مسؤول
                var isAdmin = jwtToken.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == "Admin");
                if (!isAdmin)
                {
                    return Unauthorized(new { success = false, error = "غير مصرح لك بحل النزاعات" });
                }

                // الحصول على معرف المسؤول
                var adminId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(adminId) || !int.TryParse(adminId, out int parsedAdminId))
                {
                    return BadRequest(new { success = false, error = "معرف المسؤول غير صالح" });
                }

                if (string.IsNullOrWhiteSpace(resolution))
                {
                    return BadRequest(new { success = false, error = "قرار حل النزاع مطلوب" });
                }

                var resolvedDispute = await _disputeService.ResolveDisputeAsync(id, resolution, parsedAdminId);
                if (resolvedDispute == null)
                {
                    return NotFound(new { success = false, error = "النزاع غير موجود" });
                }

                return Ok(new { 
                    success = true, 
                    message = "تم حل النزاع بنجاح",
                    data = resolvedDispute 
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }
    }
}

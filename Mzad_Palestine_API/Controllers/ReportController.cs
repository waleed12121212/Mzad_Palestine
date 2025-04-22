using Microsoft.AspNetCore.Mvc;
using Mzad_Palestine_Core.DTOs;
using Mzad_Palestine_Core.Interfaces.Services;
using Mzad_Palestine_Core.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Mzad_Palestine_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly IListingService _listingService;
        private readonly ILogger<ReportController> _logger;

        public ReportController(IReportService reportService, IListingService listingService, ILogger<ReportController> logger)
        {
            _reportService = reportService;
            _listingService = listingService;
            _jsonOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                MaxDepth = 32
            };
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReportDto dto)
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
                    return BadRequest(new { success = false, error = "سبب التقرير مطلوب" });
                }

                if (dto.ReportedListingId <= 0)
                {
                    return BadRequest(new { success = false, error = "معرف القائمة المبلغ عنها غير صالح" });
                }

                // Check if the listing exists
                var listing = await _listingService.GetByIdAsync(dto.ReportedListingId);
                if (listing == null)
                {
                    return BadRequest(new { success = false, error = "القائمة المبلغ عنها غير موجودة" });
                }

                // Check if user is reporting their own listing
                if (listing.UserId == parsedUserId)
                {
                    return BadRequest(new { success = false, error = "لا يمكنك الإبلاغ عن القائمة الخاصة بك" });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        success = false,
                        error = "البيانات غير صالحة",
                        details = ModelState.Values
                            .SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage)
                    });
                }

                // Check if user has already reported this listing
                var existingReport = await _reportService.GetAllAsync();
                var hasExistingReport = existingReport.Any(r => r.ReporterId == parsedUserId && r.ReportedListingId == dto.ReportedListingId);
                if (hasExistingReport)
                {
                    return BadRequest(new { success = false, error = "لقد قمت بالإبلاغ عن هذه القائمة مسبقاً" });
                }

                dto.ReporterId = parsedUserId;
                var createdReport = await _reportService.CreateAsync(new Report
                {
                    Reason = dto.Reason,
                    ReporterId = parsedUserId,
                    ReportedListingId = dto.ReportedListingId,
                    CreatedAt = DateTime.UtcNow,
                    Status = "Pending"
                });

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = createdReport.ReportId },
                    new { success = true, data = createdReport }
                );
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogWarning(ex, "Invalid data provided when creating report");
                return BadRequest(new { success = false, error = "البيانات المقدمة غير صالحة" });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid argument when creating report");
                return BadRequest(new { success = false, error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Validation error when creating report");
                return BadRequest(new { success = false, error = ex.Message });
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error when creating report");
                return StatusCode(500, new { success = false, error = "حدث خطأ في قاعدة البيانات. الرجاء المحاولة مرة أخرى." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error when creating report: {Message}", ex.Message);
                
                // Check for specific error messages and provide appropriate responses
                if (ex.Message.Contains("User with ID") && ex.Message.Contains("does not exist"))
                {
                    return BadRequest(new { success = false, error = "المستخدم غير موجود" });
                }
                if (ex.Message.Contains("Listing with ID") && ex.Message.Contains("does not exist"))
                {
                    return BadRequest(new { success = false, error = "القائمة المبلغ عنها غير موجودة" });
                }
                if (ex.Message.Contains("Failed to save"))
                {
                    return StatusCode(500, new { success = false, error = "فشل حفظ التقرير. الرجاء المحاولة مرة أخرى." });
                }
                
                return StatusCode(500, new { success = false, error = "حدث خطأ غير متوقع. الرجاء المحاولة مرة أخرى." });
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

                var reports = await _reportService.GetAllAsync();
                return Ok(new { success = true, data = reports });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
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

                var report = await _reportService.GetByIdAsync(id);
                if (report == null)
                {
                    return NotFound(new { success = false, error = "التقرير غير موجود" });
                }

                return Ok(new { success = true, data = report });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateReportDto dto)
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

                var existingReport = await _reportService.GetByIdAsync(id);
                if (existingReport == null)
                {
                    return NotFound(new { success = false, error = "التقرير غير موجود" });
                }

                if (existingReport.ReporterId != parsedUserId)
                {
                    return Unauthorized(new { success = false, error = "غير مصرح لك بتحديث هذا التقرير" });
                }

                if (string.IsNullOrWhiteSpace(dto.Reason))
                {
                    return BadRequest(new { success = false, error = "سبب التقرير مطلوب" });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        success = false,
                        error = "البيانات غير صالحة",
                        details = ModelState.Values
                            .SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage)
                    });
                }

                var updatedReport = await _reportService.UpdateAsync(id, dto);
                return Ok(new { success = true, data = updatedReport });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
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

                var existingReport = await _reportService.GetByIdAsync(id);
                if (existingReport == null)
                {
                    return NotFound(new { success = false, error = "التقرير غير موجود" });
                }

                if (existingReport.ReporterId != parsedUserId)
                {
                    return Unauthorized(new { success = false, error = "غير مصرح لك بحذف هذا التقرير" });
                }

                var result = await _reportService.DeleteAsync(id);
                if (!result)
                {
                    return NotFound(new { success = false, error = "التقرير غير موجود" });
                }

                return Ok(new { success = true, message = "تم حذف التقرير بنجاح" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }
    }
} 
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mzad_Palestine_Core.DTOs;
using Mzad_Palestine_Core.Interfaces.Services;
using Mzad_Palestine_Core.Models;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Mzad_Palestine_Core.DTOs.Report;

namespace Mzad_Palestine_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
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
                var userRole = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                if (userRole != "Admin")
                {
                    return Unauthorized(new { error = "غير مصرح لك بالوصول" });
                }

                var reports = await _reportService.GetAllAsync();
                return Ok(new { success = true , data = reports });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false , error = ex.Message });
            }
        }

        [HttpGet("{id}")]
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
                var userRole = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { error = "المستخدم غير موجود" });
                }

                var report = await _reportService.GetByIdAsync(id);
                if (report == null)
                {
                    return NotFound(new { error = "التقرير غير موجود" });
                }

                // Only allow admin or the reporter to view the report
                if (userRole != "Admin" && report.ReporterId != int.Parse(userId))
                {
                    return Unauthorized(new { error = "غير مصرح لك بعرض هذا التقرير" });
                }

                return Ok(new { success = true , data = report });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false , error = ex.Message });
            }
        }

        [HttpPost("listing/{listingId}")]
        public async Task<IActionResult> ReportListing(int listingId , [FromBody] CreateReportDto createReportDto)
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

                if (string.IsNullOrWhiteSpace(createReportDto.Reason))
                {
                    return BadRequest(new { success = false , error = "يجب تحديد سبب البلاغ" });
                }

                if (listingId <= 0)
                {
                    return BadRequest(new { success = false , error = "يجب تحديد معرف الإعلان المبلغ عنه" });
                }

                var report = new Report
                {
                    Reason = createReportDto.Reason ,
                    ReporterId = int.Parse(userId) ,
                    ReportedListingId = listingId ,
                    ReportType = "Listing" ,
                    CreatedAt = DateTime.UtcNow
                };

                var createdReport = await _reportService.CreateAsync(report);
                return CreatedAtAction(
                    nameof(GetById) ,
                    new { id = createdReport.ReportId } ,
                    new { success = true , data = createdReport }
                );
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false , error = ex.ToString() });
            }
        }

        [HttpPost("auction/{auctionId}")]
        public async Task<IActionResult> ReportAuction(int auctionId, [FromBody] CreateAuctionReportDto createAuctionReportDto)
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

                if (string.IsNullOrWhiteSpace(createAuctionReportDto.Reason))
                {
                    return BadRequest(new { success = false, error = "يجب تحديد سبب البلاغ" });
                }

                if (auctionId <= 0)
                {
                    return BadRequest(new { success = false, error = "يجب تحديد معرف المزاد المبلغ عنه" });
                }

                var report = new Report
                {
                    Reason = createAuctionReportDto.Reason,
                    ReporterId = int.Parse(userId),
                    ReportedAuctionId = auctionId,
                    ReportType = "Auction",
                    CreatedAt = DateTime.UtcNow
                };

                var createdReport = await _reportService.CreateAsync(report);
                return CreatedAtAction(
                    nameof(GetById),
                    new { id = createdReport.ReportId },
                    new { success = true, data = createdReport }
                );
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id , [FromBody] UpdateReportDto updateReportDto)
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
                var userRole = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { error = "المستخدم غير موجود" });
                }

                if (userRole != "Admin")
                {
                    return Unauthorized(new { error = "غير مصرح لك بتحديث التقارير" });
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

                updateReportDto.ResolvedBy = int.Parse(userId);

                var updatedReport = await _reportService.UpdateAsync(id , updateReportDto);
                if (updatedReport == null)
                {
                    return NotFound(new { error = "التقرير غير موجود" });
                }

                return Ok(new { success = true , data = updatedReport });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false , error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
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
                var userRole = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                if (userRole != "Admin")
                {
                    return Unauthorized(new { error = "غير مصرح لك بحذف التقارير" });
                }

                var result = await _reportService.DeleteAsync(id);
                if (result)
                {
                    return Ok(new { success = true , message = "تم حذف التقرير بنجاح" });
                }

                return NotFound(new { error = "التقرير غير موجود" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false , error = ex.Message });
            }
        }
    }
}
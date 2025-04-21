using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mzad_Palestine_Core.DTO_s.Payment;
using Mzad_Palestine_Core.Interfaces.Services;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using System.Text.Json.Serialization;
using Mzad_Palestine_Core.Models;

namespace Mzad_Palestine_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly JsonSerializerOptions _jsonOptions;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
            _jsonOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                MaxDepth = 32
            };
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentDto dto)
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

                dto.UserId = parsedUserId;
                var createdPayment = await _paymentService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetPaymentById), new { id = createdPayment.Id }, new { success = true, data = createdPayment });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentById(int id)
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { success = false, error = "الرجاء تسجيل الدخول" });
                }

                var payment = await _paymentService.GetByIdAsync(id);
                if (payment == null)
                    return NotFound(new { success = false, error = "الدفعة غير موجودة" });

                return Ok(new { success = true, data = payment });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetUserPayments()
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

                var payments = await _paymentService.GetByUserIdAsync(parsedUserId);
                return Ok(new { success = true, data = payments });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePayment(int id, [FromBody] UpdatePaymentDto dto)
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { success = false, error = "الرجاء تسجيل الدخول" });
                }

                var existingPayment = await _paymentService.GetByIdAsync(id);
                if (existingPayment == null)
                    return NotFound(new { success = false, error = "الدفعة غير موجودة" });

                // Update logic will be handled in the service
                await _paymentService.UpdateAsync(id, dto);
                return Ok(new { success = true, message = "تم تحديث الدفعة بنجاح" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpGet("auction/{auctionId:int}")]
        public async Task<IActionResult> GetAuctionPayments(int auctionId)
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { success = false, error = "الرجاء تسجيل الدخول" });
                }

                var payments = await _paymentService.GetByAuctionIdAsync(auctionId);
                return Ok(new { success = true, data = payments });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpPost("verify/{id:int}")]
        public async Task<IActionResult> VerifyPayment(int id)
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

                await _paymentService.VerifyPaymentAsync(id, parsedUserId);
                return Ok(new { success = true, message = "تم التحقق من الدفعة بنجاح" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeletePayment(int id)
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

                await _paymentService.DeletePaymentAsync(id, parsedUserId);
                return Ok(new { success = true, message = "تم حذف الدفعة بنجاح" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }
    }
}

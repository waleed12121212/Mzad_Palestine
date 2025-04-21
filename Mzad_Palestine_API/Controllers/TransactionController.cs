using Microsoft.AspNetCore.Mvc;
using Mzad_Palestine_Core.DTO_s.Transaction;
using Mzad_Palestine_Core.Interfaces.Services;
using Mzad_Palestine_Core.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mzad_Palestine_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly JsonSerializerOptions _jsonOptions;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
            _jsonOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                MaxDepth = 32
            };
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTransactionDto dto)
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

                var transaction = new Transaction
                {
                    UserId = parsedUserId,
                    Amount = dto.Amount,
                    TransactionType = dto.TransactionType,
                    Description = dto.Description,
                    Status = "Pending",
                    TransactionDate = DateTime.UtcNow
                };

                var result = await _transactionService.AddAsync(transaction);
                return CreatedAtAction(nameof(GetById), new { id = result.TransactionId }, new { success = true, data = result });
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

                var transaction = await _transactionService.GetByIdAsync(id);
                if (transaction == null)
                    return NotFound(new { success = false, error = "المعاملة غير موجودة" });

                return Ok(new { success = true, data = transaction });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetUserTransactions()
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

                var transactions = await _transactionService.GetByUserIdAsync(parsedUserId);
                return Ok(new { success = true, data = transactions });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTransactionDto dto)
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

                var existingTransaction = await _transactionService.GetByIdAsync(id);
                if (existingTransaction == null)
                    return NotFound(new { success = false, error = "المعاملة غير موجودة" });

                var updatedTransaction = new Transaction
                {
                    TransactionId = id,
                    UserId = existingTransaction.UserId,
                    Amount = dto.Amount ?? existingTransaction.Amount,
                    TransactionType = dto.TransactionType ?? existingTransaction.TransactionType,
                    Description = dto.Description ?? existingTransaction.Description,
                    Status = dto.Status ?? existingTransaction.Status,
                    TransactionDate = existingTransaction.TransactionDate
                };

                var result = await _transactionService.UpdateAsync(updatedTransaction);
                return Ok(new { success = true, data = result });
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

                var success = await _transactionService.DeleteAsync(id);
                if (!success)
                    return NotFound(new { success = false, error = "المعاملة غير موجودة" });

                return Ok(new { success = true, message = "تم حذف المعاملة بنجاح" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpGet("date-range")]
        public async Task<IActionResult> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { success = false, error = "الرجاء تسجيل الدخول" });
                }

                var transactions = await _transactionService.GetByDateRangeAsync(startDate, endDate);
                return Ok(new { success = true, data = transactions });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpPost("{id:int}/process")]
        public async Task<IActionResult> ProcessPayment(int id)
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { success = false, error = "الرجاء تسجيل الدخول" });
                }

                var success = await _transactionService.ProcessPaymentAsync(id);
                if (!success)
                    return NotFound(new { success = false, error = "المعاملة غير موجودة أو لا يمكن معالجتها" });

                return Ok(new { success = true, message = "تمت معالجة المعاملة بنجاح" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpPost("{id:int}/refund")]
        public async Task<IActionResult> RefundTransaction(int id)
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { success = false, error = "الرجاء تسجيل الدخول" });
                }

                var success = await _transactionService.RefundTransactionAsync(id);
                if (!success)
                    return NotFound(new { success = false, error = "المعاملة غير موجودة أو لا يمكن استردادها" });

                return Ok(new { success = true, message = "تم استرداد المعاملة بنجاح" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }
    }
} 
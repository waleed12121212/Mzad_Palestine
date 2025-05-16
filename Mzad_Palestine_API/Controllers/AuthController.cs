using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Mzad_Palestine_Core.DTOs;
using Mzad_Palestine_Core.Interfaces.Services;
using Mzad_Palestine_Core.Models;
using System;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Mzad_Palestine_Core.DTO_s.User;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace Mzad_Palestine_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        public AuthController(IAuthService authService , IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
                {
                    return BadRequest(new { error = "البريد الإلكتروني وكلمة المرور مطلوبان" });
                }

                var result = await _authService.LoginAsync(request.Email , request.Password);
                if (result.StartsWith("حدث خطأ"))
                {
                    return BadRequest(new { error = result });
                }

                return Ok(new { token = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout( )
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
                var username = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { error = "المستخدم غير موجود" });
                }

                var result = await _authService.LogoutAsync(username);
                if (result == "تم تسجيل الخروج بنجاح")
                {
                    // Add the token to the revoked tokens list
                    var jti = jwtToken.Id;
                    if (!string.IsNullOrEmpty(jti))
                    {
                        Mzad_Palestine_Infrastructure.Repositories.AuthRepository.RevokedTokens.Add(jti);
                    }
                    return Ok(new { message = result });
                }
                return BadRequest(new { error = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500 , new { error = $"حدث خطأ أثناء تسجيل الخروج: {ex.Message}" });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Email) ||
                    string.IsNullOrEmpty(request.Password) || string.IsNullOrEmpty(request.Phone))
                {
                    return BadRequest(new { error = "جميع الحقول مطلوبة" });
                }

                var user = new User
                {
                    UserName = request.Username ,
                    Email = request.Email ,
                    Phone = request.Phone ,
                    FirstName = request.Username ,
                    LastName = request.Username ,
                    EmailConfirmed = true // تعيين البريد الإلكتروني كمؤكد مؤقتاً
                };

                var result = await _authService.RegisterAsync(user , request.Password);
                if (result.StartsWith("تم إنشاء المستخدم"))
                {
                    return Ok(new { message = result });
                }
                return BadRequest(new { error = result });
            }
            catch (Exception ex)
            {
                // التقاط الخطأ الداخلي إذا كان موجوداً
                var innerException = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(new { error = $"حدث خطأ أثناء التسجيل: {innerException}" });
            }
        }
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto request)
        {
            try
            {
                var result = await _authService.ChangePasswordAsync(request.Email , request.CurrentPassword , request.NewPassword);
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto request)
        {
            try
            {
                var result = await _authService.ResetPasswordAsync(request.Email , request.Token , request.NewPassword);
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto request)
        {
            try
            {
                var result = await _authService.ForgotPasswordAsync(request.Email);
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailDto request)
        {
            try
            {
                var result = await _authService.ConfirmEmailAsync(request.UserId , request.Token);
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("send-email-confirmation")]
        public async Task<IActionResult> SendEmailConfirmation([FromBody] SendEmailConfirmationDto request)
        {
            try
            {
                var result = await _authService.SendEmailConfirmationLinkAsync(request.Email);
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("validate-token")]
        public async Task<IActionResult> ValidateToken(ValidateTokenDto request)
        {
            try
            {
                var result = await _authService.ValidateTokenAsync(request.Token);
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("verify-email-code")]
        public async Task<IActionResult> VerifyEmailCode([FromBody] VerifyEmailCodeDto request)
        {
            try
            {
                var result = await _authService.VerifyEmailWithCodeAsync(request.Email, request.VerificationCode);
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("reset-password-with-code")]
        public async Task<IActionResult> ResetPasswordWithCode([FromBody] ResetPasswordWithCodeDto request)
        {
            try
            {
                var result = await _authService.ResetPasswordWithCodeAsync(request.Email, request.VerificationCode, request.NewPassword);
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        //[HttpGet]
        //public async Task<IActionResult> GetAll( )
        //{
        //    try
        //    {
        //        var users = await _userService.GetAllUsersAsync();
        //        return Ok(users);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { error = ex.Message });
        //    }
        //}

        //[HttpGet("{id:int}")]
        //public async Task<IActionResult> GetById(int id)
        //{
        //    try
        //    {
        //        var user = await _userService.GetUserByIdAsync(id);
        //        if (user == null)
        //            return NotFound();
        //        return Ok(user);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { error = ex.Message });
        //    }
        //}
    }
}

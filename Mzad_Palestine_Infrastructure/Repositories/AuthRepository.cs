using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Mzad_Palestine_Core.Interfaces;
using Mzad_Palestine_Core.Models;
using Mzad_Palestine_Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthRepository(
            ApplicationDbContext context,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<string> LoginAsync(string username, string password)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(username);
                if (user == null)
                    return "فشل تسجيل الدخول: المستخدم غير موجود";

                var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
                if (!result.Succeeded)
                    return "فشل تسجيل الدخول: كلمة المرور غير صحيحة";

                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                };

                foreach (var role in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }

                var token = GenerateJwtToken(authClaims);
                return token;
            }
            catch (Exception ex)
            {
                return $"حدث خطأ أثناء تسجيل الدخول: {ex.Message}";
            }
        }

        private string GenerateJwtToken(List<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> ValidateTokenAsync(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"],
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
                return "الرمز صالح";
            }
            catch (Exception ex)
            {
                return $"الرمز غير صالح: {ex.Message}";
            }
        }

        public async Task<string> LogoutAsync(string username)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(username);
                if (user == null)
                    return "المستخدم غير موجود";

                await _signInManager.SignOutAsync();
                return "تم تسجيل الخروج بنجاح";
            }
            catch (Exception ex)
            {
                return $"حدث خطأ أثناء تسجيل الخروج: {ex.Message}";
            }
        }

        public async Task<string> RegisterAsync(User user, string password)
        {
            try
            {
                var userExists = await _userManager.FindByNameAsync(user.UserName);
                if (userExists != null)
                    return "المستخدم موجود بالفعل";

                user.SecurityStamp = Guid.NewGuid().ToString();
                user.CreatedAt = DateTime.UtcNow;

                var result = await _userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                    return string.Join(", ", result.Errors.Select(e => e.Description));

                // تعيين الدور الافتراضي
                await _userManager.AddToRoleAsync(user, "User");

                return "تم إنشاء المستخدم بنجاح";
            }
            catch (Exception ex)
            {
                return $"حدث خطأ أثناء التسجيل: {ex.Message}";
            }
        }

        public async Task<string> ChangePasswordAsync(string email, string currentPassword, string newPassword)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                    return "المستخدم غير موجود";

                var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
                if (!result.Succeeded)
                    return string.Join(", ", result.Errors.Select(e => e.Description));

                return "تم تغيير كلمة المرور بنجاح";
            }
            catch (Exception ex)
            {
                return $"حدث خطأ أثناء تغيير كلمة المرور: {ex.Message}";
            }
        }

        public async Task<string> ResetPasswordAsync(string email, string token, string newPassword)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                    return "المستخدم غير موجود";

                var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
                if (!result.Succeeded)
                    return string.Join(", ", result.Errors.Select(e => e.Description));

                return "تم إعادة تعيين كلمة المرور بنجاح";
            }
            catch (Exception ex)
            {
                return $"حدث خطأ أثناء إعادة تعيين كلمة المرور: {ex.Message}";
            }
        }

        public async Task<string> ForgotPasswordAsync(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                    return "المستخدم غير موجود";

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                // هنا يمكنك إرسال البريد الإلكتروني مع الرمز
                return token;
            }
            catch (Exception ex)
            {
                return $"حدث خطأ أثناء طلب إعادة تعيين كلمة المرور: {ex.Message}";
            }
        }

        public async Task<string> ConfirmEmailAsync(string userId, string token)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return "المستخدم غير موجود";

                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (!result.Succeeded)
                    return string.Join(", ", result.Errors.Select(e => e.Description));

                return "تم تأكيد البريد الإلكتروني بنجاح";
            }
            catch (Exception ex)
            {
                return $"حدث خطأ أثناء تأكيد البريد الإلكتروني: {ex.Message}";
            }
        }

        public async Task<string> SendEmailConfirmationLinkAsync(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                    return "المستخدم غير موجود";

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                // هنا يمكنك إرسال البريد الإلكتروني مع الرمز
                return token;
            }
            catch (Exception ex)
            {
                return $"حدث خطأ أثناء إرسال رابط التأكيد: {ex.Message}";
            }
        }
    }
}

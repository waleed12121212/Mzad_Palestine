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
            ApplicationDbContext context ,
            UserManager<User> userManager ,
            SignInManager<User> signInManager ,
            IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
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
            var credentials = new SigningCredentials(key , SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"] ,
                audience: _configuration["Jwt:Audience"] ,
                claims: claims ,
                expires: DateTime.UtcNow.AddHours(3) ,
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
                    ValidateIssuerSigningKey = true ,
                    IssuerSigningKey = new SymmetricSecurityKey(key) ,
                    ValidateIssuer = true ,
                    ValidIssuer = _configuration["Jwt:Issuer"] ,
                    ValidateAudience = true ,
                    ValidAudience = _configuration["Jwt:Audience"] ,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token , tokenValidationParameters , out SecurityToken validatedToken);

                // التحقق من وجود التوكن في القائمة السوداء
                var jti = principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
                if (!string.IsNullOrEmpty(jti) && RevokedTokens.Contains(jti))
                    return "الرمز غير صالح: تم إلغاء هذا التوكن";

                return "الرمز صالح";
            }
            catch (Exception ex)
            {
                return $"الرمز غير صالح: {ex.Message}";
            }
        }

        public static readonly HashSet<string> RevokedTokens = new HashSet<string>();

        public async Task<string> LogoutAsync(string username)
        {
            try
            {
                // البحث عن المستخدم باستخدام البريد الإلكتروني
                var user = await _userManager.FindByEmailAsync(username);
                if (user == null)
                {
                    // إذا لم يتم العثور عليه بالبريد الإلكتروني، جرب البحث باسم المستخدم
                    user = await _userManager.FindByNameAsync(username);
                    if (user == null)
                        return "المستخدم غير موجود";
                }

                // الحصول على التوكن الحالي
                var currentToken = await _userManager.GetAuthenticationTokenAsync(user, "JWT", "AccessToken");
                if (!string.IsNullOrEmpty(currentToken))
                {
                    // إضافة التوكن إلى القائمة السوداء
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var token = tokenHandler.ReadJwtToken(currentToken);
                    var jti = token.Id;
                    if (!string.IsNullOrEmpty(jti))
                    {
                        RevokedTokens.Add(jti);
                    }
                }

                // تسجيل الخروج من النظام
                await _signInManager.SignOutAsync();

                // تحديث SecurityStamp للمستخدم لإبطال الـ tokens الحالية
                var result = await _userManager.UpdateSecurityStampAsync(user);
                if (!result.Succeeded)
                    return "فشل في تسجيل الخروج";

                // إزالة التوكن من قاعدة البيانات
                await _userManager.RemoveAuthenticationTokenAsync(user, "JWT", "AccessToken");

                return "تم تسجيل الخروج بنجاح";
            }
            catch (Exception ex)
            {
                return $"حدث خطأ أثناء تسجيل الخروج: {ex.Message}";
            }
        }

        public async Task<string> RegisterAsync(User user , string password)
        {
            try
            {
                // التحقق من وجود المستخدم باسم المستخدم أو البريد الإلكتروني
                var userExists = await _userManager.FindByNameAsync(user.UserName);
                var emailExists = await _userManager.FindByEmailAsync(user.Email);

                if (userExists != null)
                    return "المستخدم موجود بالفعل";
                if (emailExists != null)
                    return "البريد الإلكتروني مستخدم بالفعل";

                // التحقق من صحة كلمة المرور
                if (string.IsNullOrEmpty(password) || password.Length < 6)
                    return "كلمة المرور يجب أن تكون على الأقل 6 أحرف";

                user.SecurityStamp = Guid.NewGuid().ToString();
                user.CreatedAt = DateTime.UtcNow;
                user.IsActive = true;
                user.Role = Mzad_Palestine_Core.Enums.UserRole.Seller;
                user.Address = user.Address ?? "غير محدد";

                var result = await _userManager.CreateAsync(user , password);
                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description);
                    return string.Join(", " , errors);
                }

                // تعيين الدور الافتراضي
                await _userManager.AddToRoleAsync(user , "User");

                return "تم إنشاء المستخدم بنجاح";
            }
            catch (Exception ex)
            {
                // التقاط الخطأ الداخلي إذا كان موجوداً
                var innerException = ex.InnerException?.Message ?? ex.Message;
                return $"حدث خطأ أثناء التسجيل: {innerException}";
            }
        }

        public async Task<string> ChangePasswordAsync(string email , string currentPassword , string newPassword)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                    return "المستخدم غير موجود";

                var result = await _userManager.ChangePasswordAsync(user , currentPassword , newPassword);
                if (!result.Succeeded)
                    return string.Join(", " , result.Errors.Select(e => e.Description));

                return "تم تغيير كلمة المرور بنجاح";
            }
            catch (Exception ex)
            {
                return $"حدث خطأ أثناء تغيير كلمة المرور: {ex.Message}";
            }
        }

        public async Task<string> ResetPasswordAsync(string email , string token , string newPassword)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                    return "المستخدم غير موجود";

                var result = await _userManager.ResetPasswordAsync(user , token , newPassword);
                if (!result.Succeeded)
                    return string.Join(", " , result.Errors.Select(e => e.Description));

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

        public async Task<string> ConfirmEmailAsync(string userId , string token)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return "المستخدم غير موجود";

                var result = await _userManager.ConfirmEmailAsync(user , token);
                if (!result.Succeeded)
                    return string.Join(", " , result.Errors.Select(e => e.Description));

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

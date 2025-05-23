using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Mzad_Palestine_Core.Interfaces;
using Mzad_Palestine_Core.Interfaces.Services;
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
        private static readonly Dictionary<string, (string Code, DateTime Expiry)> _verificationCodes = new();
        private const int VERIFICATION_CODE_LENGTH = 6;
        private const int VERIFICATION_CODE_EXPIRY_MINUTES = 15;

        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly IEmailService _emailService;

        public AuthRepository(
            ApplicationDbContext context,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IConfiguration configuration,
            RoleManager<IdentityRole<int>> roleManager,
            IEmailService emailService)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _emailService = emailService;
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                    return "فشل تسجيل الدخول: البريد الإلكتروني وكلمة المرور مطلوبان";

                // Normalize email to lowercase
                email = email.Trim().ToLowerInvariant();
                
                // Try to find user by email first
                var user = await _userManager.FindByEmailAsync(email);
                
                // If not found by email, try to find by username
                if (user == null)
                {
                    // Try to find user by normalized username
                    user = await _userManager.Users
                        .FirstOrDefaultAsync(u => u.NormalizedEmail == _userManager.NormalizeName(email) ||
                                               u.NormalizedUserName == _userManager.NormalizeName(email));
                                       
                    if (user == null)
                        return "فشل تسجيل الدخول: المستخدم غير موجود";
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
                if (!result.Succeeded)
                    return "فشل تسجيل الدخول: كلمة المرور غير صحيحة";

                // التحقق من وجود أدوار للمستخدم
                var userRoles = await _userManager.GetRolesAsync(user);
                
                // إذا لم يكن للمستخدم أي دور، قم بتعيين دور "User" له
                if (!userRoles.Any())
                {
                    // التحقق من وجود الدور "User"
                    var roleExists = await _roleManager.RoleExistsAsync("User");
                    if (!roleExists)
                    {
                        // إنشاء الدور إذا لم يكن موجوداً
                        var role = new IdentityRole<int>("User");
                        var createRoleResult = await _roleManager.CreateAsync(role);
                        if (!createRoleResult.Succeeded)
                        {
                            return $"فشل في إنشاء الدور: {string.Join(", ", createRoleResult.Errors.Select(e => e.Description))}";
                        }
                    }

                    // إضافة الدور للمستخدم
                    var addToRoleResult = await _userManager.AddToRoleAsync(user, "User");
                    if (!addToRoleResult.Succeeded)
                    {
                        return $"فشل في تعيين الدور للمستخدم: {string.Join(", ", addToRoleResult.Errors.Select(e => e.Description))}";
                    }

                    // إعادة تحميل أدوار المستخدم
                    userRoles = await _userManager.GetRolesAsync(user);
                }

                // التأكد من أن لدينا على الأقل دور واحد
                if (!userRoles.Any())
                {
                    var allRoles = await _roleManager.Roles.ToListAsync();
                    var availableRoles = string.Join(", ", allRoles.Select(r => r.Name));
                    return $"فشل في الحصول على صلاحيات المستخدم بعد المحاولة. الأدوار المتاحة: {availableRoles}";
                }

                var authClaims = new List<Claim>();

                // إضافة الأدوار أولاً
                foreach (var role in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }

                // إضافة باقي المعلومات
                authClaims.AddRange(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                });

                var token = GenerateJwtToken(authClaims);

                // التحقق من وجود الدور في التوكن
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var roleInToken = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                if (string.IsNullOrEmpty(roleInToken))
                {
                    // إرجاع تفاصيل أكثر عن المشكلة
                    var allClaims = jwtToken.Claims.Select(c => $"{c.Type}: {c.Value}").ToList();
                    return $"فشل في إنشاء التوكن: لم يتم العثور على الصلاحيات. Claims الموجودة: {string.Join(", ", allClaims)}";
                }

                return token;
            }
            catch (Exception ex)
            {
                return $"حدث خطأ أثناء تسجيل الدخول: {ex.Message} - {ex.StackTrace}";
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

        private string GenerateVerificationCode()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        public async Task<string> SendEmailConfirmationLinkAsync(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                    return "المستخدم غير موجود";

                var verificationCode = GenerateVerificationCode();
                _verificationCodes[email] = (verificationCode, DateTime.UtcNow.AddMinutes(VERIFICATION_CODE_EXPIRY_MINUTES));

                await _emailService.SendEmailConfirmationAsync(email, user.Id.ToString(), verificationCode, "");
                
                return "تم إرسال رمز التأكيد إلى بريدك الإلكتروني";
            }
            catch (Exception ex)
            {
                return $"حدث خطأ أثناء إرسال رمز التأكيد: {ex.Message}";
            }
        }

        public async Task<string> VerifyEmailWithCodeAsync(string email, string code)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                    return "المستخدم غير موجود";

                if (!_verificationCodes.ContainsKey(email))
                    return "لم يتم إرسال رمز تحقق لهذا البريد الإلكتروني";

                var (storedCode, expiry) = _verificationCodes[email];
                
                if (DateTime.UtcNow > expiry)
                {
                    _verificationCodes.Remove(email);
                    return "انتهت صلاحية رمز التحقق";
                }

                if (code != storedCode)
                    return "رمز التحقق غير صحيح";

                // Log the values before update
                Console.WriteLine($"Before update - EmailConfirmed: {user.EmailConfirmed}, IsVerified: {user.IsVerified}");

                user.EmailConfirmed = true;
                user.IsVerified = true;

                // Log the values after setting
                Console.WriteLine($"After setting values - EmailConfirmed: {user.EmailConfirmed}, IsVerified: {user.IsVerified}");
                
                var result = await _userManager.UpdateAsync(user);
                
                if (!result.Succeeded)
                {
                    Console.WriteLine($"Update failed - Errors: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    return string.Join(", ", result.Errors.Select(e => e.Description));
                }

                // Verify the update by retrieving the user again
                var updatedUser = await _userManager.FindByEmailAsync(email);
                Console.WriteLine($"After update (retrieved from db) - EmailConfirmed: {updatedUser.EmailConfirmed}, IsVerified: {updatedUser.IsVerified}");

                _verificationCodes.Remove(email);
                return "تم تأكيد البريد الإلكتروني بنجاح";
            }
            catch (Exception ex)
            {
                return $"حدث خطأ أثناء التحقق من رمز التأكيد: {ex.Message}";
            }
        }

        public async Task<string> ForgotPasswordAsync(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                    return "المستخدم غير موجود";

                var verificationCode = GenerateVerificationCode();
                _verificationCodes[email] = (verificationCode, DateTime.UtcNow.AddMinutes(VERIFICATION_CODE_EXPIRY_MINUTES));

                await _emailService.SendPasswordResetAsync(email, verificationCode, "");

                return "تم إرسال رمز إعادة تعيين كلمة المرور إلى بريدك الإلكتروني";
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

        public async Task<string> ResetPasswordWithCodeAsync(string email, string verificationCode, string newPassword)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                    return "المستخدم غير موجود";

                if (!_verificationCodes.ContainsKey(email))
                    return "لم يتم إرسال رمز تحقق لهذا البريد الإلكتروني";

                var (storedCode, expiry) = _verificationCodes[email];
                
                if (DateTime.UtcNow > expiry)
                {
                    _verificationCodes.Remove(email);
                    return "انتهت صلاحية رمز التحقق";
                }

                if (verificationCode != storedCode)
                    return "رمز التحقق غير صحيح";

                // Generate password reset token
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                
                // Reset the password
                var result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
                if (!result.Succeeded)
                    return string.Join(", ", result.Errors.Select(e => e.Description));

                // Remove the verification code after successful password reset
                _verificationCodes.Remove(email);

                return "تم إعادة تعيين كلمة المرور بنجاح";
            }
            catch (Exception ex)
            {
                return $"حدث خطأ أثناء إعادة تعيين كلمة المرور: {ex.Message}";
            }
        }
    }
}

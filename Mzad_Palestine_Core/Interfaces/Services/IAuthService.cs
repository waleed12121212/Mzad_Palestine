using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mzad_Palestine_Core.Models;

namespace Mzad_Palestine_Core.Interfaces.Services
{
    public interface IAuthService
    {
        // تسجيل مستخدم جديد
        Task<string> RegisterAsync(User user, string password);
        // تسجيل الدخول
        Task<string> LoginAsync(string username, string password);
        // تسجيل الخروج
        Task<string> LogoutAsync(string username);
        // تغيير كلمة المرور
        Task<string> ChangePasswordAsync(string email, string currentPassword, string newPassword);
        
        // إعادة تعيين كلمة المرور
        Task<string> ResetPasswordAsync(string email, string token, string newPassword);
        
        // طلب إعادة تعيين كلمة المرور
        Task<string> ForgotPasswordAsync(string email);
        
        // تأكيد البريد الإلكتروني
        Task<string> ConfirmEmailAsync(string userId, string token);
        
        // إرسال رابط تأكيد البريد الإلكتروني
        Task<string> SendEmailConfirmationLinkAsync(string email);
        
        // التحقق من صلاحية الرمز
        Task<string> ValidateTokenAsync(string token);

        // التحقق من رمز تأكيد البريد الإلكتروني
        Task<string> VerifyEmailWithCodeAsync(string email, string code);
    }
}

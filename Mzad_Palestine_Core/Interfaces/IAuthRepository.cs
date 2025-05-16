using Mzad_Palestine_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Interfaces
{
    public interface IAuthRepository
    {
        // تسجيل الدخول
        Task<string> LoginAsync(string username,string password);
        
        // تسجيل الخروج
        Task<string> LogoutAsync(string username);
        
        // تسجيل مستخدم جديد
        Task<string> RegisterAsync(User user,string password);
        Task<string> ChangePasswordAsync(string email, string currentPassword, string newPassword);
        Task<string> ResetPasswordAsync(string email, string token, string newPassword);
        Task<string> ForgotPasswordAsync(string email);
        Task<string> ConfirmEmailAsync(string userId, string token);
        Task<string> SendEmailConfirmationLinkAsync(string email);
        Task<string> ValidateTokenAsync(string token);
        
        // التحقق من رمز تأكيد البريد الإلكتروني
        Task<string> VerifyEmailWithCodeAsync(string email, string code);

        // إعادة تعيين كلمة المرور باستخدام رمز التحقق
        Task<string> ResetPasswordWithCodeAsync(string email, string verificationCode, string newPassword);
    }
}

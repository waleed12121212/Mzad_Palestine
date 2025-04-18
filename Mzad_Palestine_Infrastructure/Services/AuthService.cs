using Mzad_Palestine_Core.Interfaces;
using Mzad_Palestine_Core.Interfaces.Services;
using Mzad_Palestine_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;

        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            try
            {
                return await _authRepository.LoginAsync(email, password);
            }
            catch (Exception ex)
            {
                return $"حدث خطأ أثناء تسجيل الدخول: {ex.Message}";
            }
        }

        public async Task<string> LogoutAsync(string username)
        {
            try
            {
                return await _authRepository.LogoutAsync(username);
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
                return await _authRepository.RegisterAsync(user, password);
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
                return await _authRepository.ChangePasswordAsync(email, currentPassword, newPassword);
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
                return await _authRepository.ResetPasswordAsync(email, token, newPassword);
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
                return await _authRepository.ForgotPasswordAsync(email);
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
                return await _authRepository.ConfirmEmailAsync(userId, token);
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
                return await _authRepository.SendEmailConfirmationLinkAsync(email);
            }
            catch (Exception ex)
            {
                return $"حدث خطأ أثناء إرسال رابط التأكيد: {ex.Message}";
            }
        }

        public async Task<string> ValidateTokenAsync(string token)
        {
            try
            {
                return await _authRepository.ValidateTokenAsync(token);
            }
            catch (Exception ex)
            {
                return $"حدث خطأ أثناء التحقق من الرمز: {ex.Message}";
            }
        }
    }
}

using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Mzad_Palestine_Core.Interfaces.Services;

namespace Mzad_Palestine_Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        private readonly string _senderEmail;
        private readonly string _senderName;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            var emailSettings = _configuration.GetSection("EmailSettings");
            _smtpServer = emailSettings["SmtpServer"];
            _smtpPort = int.Parse(emailSettings["SmtpPort"]);
            _smtpUsername = emailSettings["SmtpUsername"];
            _smtpPassword = emailSettings["SmtpPassword"];
            _senderEmail = emailSettings["FromEmail"];
            _senderName = emailSettings["SenderName"];
        }

        public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = false)
        {
            try
            {
                using var client = new SmtpClient(_smtpServer, _smtpPort)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(_smtpUsername, _smtpPassword)
                };

                var message = new MailMessage
                {
                    From = new MailAddress(_senderEmail, _senderName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = isHtml
                };
                message.To.Add(to);

                await client.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to send email: {ex.Message}");
            }
        }

        public async Task SendEmailConfirmationAsync(string email, string userId, string verificationCode, string callbackUrl)
        {
            var subject = "رمز تأكيد البريد الإلكتروني - Mzad Palestine";
            var body = $@"
                <h2>مرحباً بك في Mzad Palestine</h2>
                <p>شكراً لتسجيلك معنا. رمز تأكيد بريدك الإلكتروني هو:</p>
                <h1 style='text-align: center; font-size: 32px; padding: 10px; background-color: #f5f5f5; border-radius: 5px;'>{verificationCode}</h1>
                <p>هذا الرمز صالح لمدة 15 دقيقة فقط.</p>
                <p>إذا لم تقم بالتسجيل في موقعنا، يمكنك تجاهل هذا البريد.</p>
                <p>مع تحيات فريق Mzad Palestine</p>";

            await SendEmailAsync(email, subject, body, true);
        }

        public async Task SendPasswordResetAsync(string email, string verificationCode, string callbackUrl)
        {
            var subject = "رمز إعادة تعيين كلمة المرور - Mzad Palestine";
            var body = $@"
                <h2>إعادة تعيين كلمة المرور</h2>
                <p>لقد تلقينا طلباً لإعادة تعيين كلمة المرور الخاصة بك. رمز إعادة التعيين هو:</p>
                <h1 style='text-align: center; font-size: 32px; padding: 10px; background-color: #f5f5f5; border-radius: 5px;'>{verificationCode}</h1>
                <p>هذا الرمز صالح لمدة 15 دقيقة فقط.</p>
                <p>إذا لم تقم بهذا الطلب، يرجى تجاهل هذا البريد.</p>
                <p>مع تحيات فريق Mzad Palestine</p>";

            await SendEmailAsync(email, subject, body, true);
        }
    }
}
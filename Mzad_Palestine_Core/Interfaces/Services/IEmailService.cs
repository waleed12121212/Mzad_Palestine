using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body, bool isHtml = false);
        Task SendEmailConfirmationAsync(string email, string userId, string token, string callbackUrl);
        Task SendPasswordResetAsync(string email, string token, string callbackUrl);
    }
} 
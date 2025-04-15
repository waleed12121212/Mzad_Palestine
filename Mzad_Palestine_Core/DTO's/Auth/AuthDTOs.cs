namespace Mzad_Palestine_Core.DTOs
{
    public class LoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class RegisterDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
    }

    public class ChangePasswordDto
    {
        public string Email { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class ResetPasswordDto
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }

    public class ForgotPasswordDto
    {
        public string Email { get; set; }
    }

    public class ConfirmEmailDto
    {
        public string UserId { get; set; }
        public string Token { get; set; }
    }

    public class SendEmailConfirmationDto
    {
        public string Email { get; set; }
    }

    public class ValidateTokenDto
    {
        public string Token { get; set; }
    }
} 
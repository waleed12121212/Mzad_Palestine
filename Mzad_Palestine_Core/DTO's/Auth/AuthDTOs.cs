using System.ComponentModel.DataAnnotations;

namespace Mzad_Palestine_Core.DTOs
{
    public class LoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class RegisterDto
    {
        [Required(ErrorMessage = "اسم المستخدم مطلوب")]
        [StringLength(100 , ErrorMessage = "يجب أن يكون اسم المستخدم بين {2} و {1} حرفًا" , MinimumLength = 3)]
        public string Username { get; set; }

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صالح")]
        public string Email { get; set; }

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [Phone(ErrorMessage = "رقم الهاتف غير صالح")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [StringLength(100 , ErrorMessage = "يجب أن تكون كلمة المرور بين {2} و {1} حرفًا" , MinimumLength = 9)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{9,}$" ,
            ErrorMessage = "يجب أن تحتوي كلمة المرور على حرف كبير وحرف صغير ورقم ورمز خاص على الأقل")]
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
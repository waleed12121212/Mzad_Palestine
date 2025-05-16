using System.ComponentModel.DataAnnotations;

namespace Mzad_Palestine_Core.DTOs
{
    public class ResetPasswordWithCodeDto
    {
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صالح")]
        public string Email { get; set; }

        [Required(ErrorMessage = "رمز التحقق مطلوب")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "رمز التحقق يجب أن يكون 6 أرقام")]
        public string VerificationCode { get; set; }

        [Required(ErrorMessage = "كلمة المرور الجديدة مطلوبة")]
        [StringLength(100, ErrorMessage = "يجب أن تكون كلمة المرور بين {2} و {1} حرفًا", MinimumLength = 9)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{9,}$",
            ErrorMessage = "يجب أن تحتوي كلمة المرور على حرف كبير وحرف صغير ورقم ورمز خاص على الأقل")]
        public string NewPassword { get; set; }
    }
} 
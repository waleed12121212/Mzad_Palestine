using FluentValidation;
using Mzad_Palestine_Core.DTO_s.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Validation
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoValidator( )
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required.");

            RuleFor(x => x.Email)
                .NotEmpty().EmailAddress().WithMessage("A valid email is required.");

            RuleFor(x => x.Password)
                .NotEmpty().MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("رقم الهاتف مطلوب.")
                .Matches(@"^(\+970|0)(5[0-9]|2[2-9])[0-9]{7}$")
                .WithMessage("رقم الهاتف غير صالح. يجب أن يبدأ الرقم بـ +970 أو 0، متبوعاً برقم المنطقة (5x أو 2x) و7 أرقام إضافية.");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role is required.");
        }
    }
}

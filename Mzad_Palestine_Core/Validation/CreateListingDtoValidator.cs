using FluentValidation;
using Mzad_Palestine_Core.DTOs.Listing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Validation
{
    public class CreateListingDtoValidator : AbstractValidator<CreateListingDto>
    {
        public CreateListingDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("عنوان المنتج مطلوب")
                .MaximumLength(255).WithMessage("العنوان يجب ألا يتجاوز 255 حرف")
                .MinimumLength(3).WithMessage("العنوان يجب أن يكون أكثر من 3 أحرف");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("وصف المنتج مطلوب")
                .MaximumLength(2000).WithMessage("الوصف يجب ألا يتجاوز 2000 حرف");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("العنوان مطلوب")
                .MaximumLength(500).WithMessage("العنوان يجب ألا يتجاوز 500 حرف");

            RuleFor(x => x.Price)
                .NotEmpty().WithMessage("السعر مطلوب")
                .GreaterThan(0).WithMessage("السعر يجب أن يكون أكبر من 0");

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("التصنيف مطلوب");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("تاريخ الانتهاء مطلوب")
                .GreaterThan(DateTime.Now).WithMessage("تاريخ الانتهاء يجب أن يكون في المستقبل");

            RuleFor(x => x.Images)
                .NotEmpty().WithMessage("الصور مطلوبة")
                .Must(x => x != null && x.Count > 0).WithMessage("يجب إضافة صورة واحدة على الأقل");
        }
    }
}

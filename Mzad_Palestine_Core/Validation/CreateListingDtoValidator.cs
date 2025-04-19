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
                .MaximumLength(100).WithMessage("العنوان يجب أن لا يتجاوز 100 حرف");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("وصف المنتج مطلوب")
                .MaximumLength(1000).WithMessage("الوصف يجب أن لا يتجاوز 1000 حرف");

            RuleFor(x => x.StartingPrice)
                .NotEmpty().WithMessage("السعر الابتدائي مطلوب")
                .GreaterThan(0).WithMessage("السعر يجب أن يكون أكبر من 0");

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("التصنيف مطلوب");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("تاريخ انتهاء المزاد مطلوب")
                .GreaterThan(DateTime.UtcNow).WithMessage("تاريخ انتهاء المزاد يجب أن يكون في المستقبل");
        }
    }
}

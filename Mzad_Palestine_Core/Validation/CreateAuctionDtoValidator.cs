using FluentValidation;
using Mzad_Palestine_Core.DTO_s.Auction;

namespace Mzad_Palestine_Core.Validation
{
    public class CreateAuctionDtoValidator : AbstractValidator<CreateAuctionDto>
    {
        public CreateAuctionDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("عنوان المزاد مطلوب")
                .MaximumLength(255).WithMessage("العنوان يجب ألا يتجاوز 255 حرف")
                .MinimumLength(3).WithMessage("العنوان يجب أن يكون أكثر من 3 أحرف");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("وصف المزاد مطلوب")
                .MaximumLength(2000).WithMessage("الوصف يجب ألا يتجاوز 2000 حرف");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("العنوان مطلوب")
                .MaximumLength(500).WithMessage("العنوان يجب ألا يتجاوز 500 حرف");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("تاريخ البدء مطلوب")
                .GreaterThan(DateTime.UtcNow).WithMessage("تاريخ البدء يجب أن يكون في المستقبل");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("تاريخ الانتهاء مطلوب")
                .GreaterThan(x => x.StartDate).WithMessage("تاريخ الانتهاء يجب أن يكون بعد تاريخ البدء");

            RuleFor(x => x.ReservePrice)
                .GreaterThan(0).WithMessage("السعر الابتدائي يجب أن يكون أكبر من 0");

            RuleFor(x => x.BidIncrement)
                .GreaterThan(0).WithMessage("قيمة الزيادة يجب أن تكون أكبر من 0");

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("التصنيف مطلوب");

            RuleFor(x => x.Images)
                .NotEmpty().WithMessage("الصور مطلوبة")
                .Must(x => x != null && x.Count > 0).WithMessage("يجب إضافة صورة واحدة على الأقل");
        }
    }
}

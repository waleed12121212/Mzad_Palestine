using FluentValidation;
using Mzad_Palestine_Core.DTO_s.Auction;

namespace Mzad_Palestine_Core.Validation
{
    public class CreateAuctionDtoValidator : AbstractValidator<CreateAuctionDto>
    {
        public CreateAuctionDtoValidator()
        {
            RuleFor(x => x.ListingId)
                .NotEmpty().WithMessage("Listing ID is required");

            RuleFor(x => x.StartTime)
                .NotEmpty().WithMessage("Start time is required")
                .GreaterThan(DateTime.Now).WithMessage("Start time must be in the future");

            RuleFor(x => x.EndTime)
                .NotEmpty().WithMessage("End time is required")
                .GreaterThan(x => x.StartTime).WithMessage("End time must be after start time");

            RuleFor(x => x.ReservePrice)
                .GreaterThan(0).WithMessage("Reserve price must be greater than 0");

            RuleFor(x => x.BidIncrement)
                .GreaterThan(0).WithMessage("Bid increment must be greater than 0");

            RuleFor(x => x.ImageUrl)
                .NotEmpty().WithMessage("Image URL is required");
        }
    }
}

using Mzad_Palestine_Core.DTO_s.Auction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Validation
{
    public class CreateAuctionDtoValidator : AbstractValidator<CreateAuctionDto>
    {
        public CreateAuctionDtoValidator( )
        {
            RuleFor(x => x.ListingId)
                .GreaterThan(0).WithMessage("Listing Id is required.");

            RuleFor(x => x.StartTime)
                .LessThan(x => x.EndTime).WithMessage("Start time must be less than End time.");

            RuleFor(x => x.ReservePrice)
                .GreaterThanOrEqualTo(0).WithMessage("Reserve Price must be zero or greater.");

            RuleFor(x => x.BidIncrement)
                .GreaterThan(0).WithMessage("Bid Increment must be greater than zero.");

            RuleFor(x => x.ImageUrl)
                .NotEmpty().WithMessage("Image Url is required.");
        }
    }
}

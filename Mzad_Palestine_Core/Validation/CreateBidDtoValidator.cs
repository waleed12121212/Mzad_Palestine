using FluentValidation;
using Mzad_Palestine_Core.DTO_s.Bid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Validation
{
    public class CreateBidDtoValidator : AbstractValidator<CreateBidDto>
    {
        public CreateBidDtoValidator( )
        {
            RuleFor(x => x.AuctionId)
                .GreaterThan(0).WithMessage("Auction Id is required.");

            RuleFor(x => x.BidAmount)
                .GreaterThan(0).WithMessage("Bid Amount must be greater than zero.");
        }
    }
}

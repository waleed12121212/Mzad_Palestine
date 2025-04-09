using FluentValidation;
using Mzad_Palestine_Core.DTO_s.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Validation
{
    public class CreatePaymentDtoValidator : AbstractValidator<CreatePaymentDto>
    {
        public CreatePaymentDtoValidator( )
        {
            RuleFor(x => x.AuctionId)
                .GreaterThan(0).WithMessage("Auction Id is required.");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than zero.");

            RuleFor(x => x.Method)
                .NotEmpty().WithMessage("Payment method is required.");
        }
    }

}

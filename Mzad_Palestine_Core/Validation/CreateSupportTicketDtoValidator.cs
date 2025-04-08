using Mzad_Palestine_Core.DTO_s.Customer_Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Validation
{
    public class CreateSupportTicketDtoValidator : AbstractValidator<CreateSupportTicketDto>
    {
        public CreateSupportTicketDtoValidator( )
        {
            RuleFor(x => x.Subject)
                .NotEmpty().WithMessage("Subject is required.");

            RuleFor(x => x.Message)
                .NotEmpty().WithMessage("Message is required.");
        }
    }
}

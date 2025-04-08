using Mzad_Palestine_Core.DTO_s.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Interfaces.Services
{
    public interface IPaymentService
    {
        Task<PaymentDto> CreateAsync(CreatePaymentDto dto);
        Task<IEnumerable<PaymentDto>> GetByUserIdAsync(int userId);
    }
}

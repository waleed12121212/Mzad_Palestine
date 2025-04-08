using Mzad_Palestine_Core.DTO_s.Payment;
using Mzad_Palestine_Core.Interfaces.Services;
using Mzad_Palestine_Core.Interfaces;
using Mzad_Palestine_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Infrastructure.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _repository;
        public PaymentService(IPaymentRepository repository) => _repository = repository;

        public async Task<PaymentDto> CreateAsync(CreatePaymentDto dto)
        {
            var entity = new Payment { UserId = dto.UserId , Amount = dto.Amount , Method = dto.Method , Status = "Pending" };
            var created = await _repository.AddAsync(entity);
            return new PaymentDto(created.Id , created.UserId , created.Amount , created.Method , created.Status , created.Timestamp);
        }
    }
}

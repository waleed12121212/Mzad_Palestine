using Mzad_Palestine_Core.DTO_s.Payment;
using Mzad_Palestine_Core.Interfaces.Services;
using Mzad_Palestine_Core.Interfaces;
using Mzad_Palestine_Core.Models;
using Mzad_Palestine_Core.Enums;
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
            var entity = new Payment
            {
                AuctionId = dto.AuctionId,
                Amount = dto.Amount,
                Method = Enum.Parse<PaymentMethod>(dto.Method),
                Status = PaymentStatus.Pending,
                TransactionDate = DateTime.UtcNow
            };
            
            await _repository.AddAsync(entity);
            
            return new PaymentDto
            {
                Id = entity.PaymentId,
                AuctionId = entity.AuctionId,
                Amount = entity.Amount,
                Method = entity.Method.ToString(),
                Status = entity.Status.ToString(),
                TransactionDate = entity.TransactionDate
            };
        }

        public async Task<IEnumerable<PaymentDto>> GetByUserIdAsync(int userId)
        {
            var payments = await _repository.GetPaymentsByUserAsync(userId);
            return payments.Select(p => new PaymentDto
            {
                Id = p.PaymentId,
                AuctionId = p.AuctionId,
                Amount = p.Amount,
                Method = p.Method.ToString(),
                Status = p.Status.ToString(),
                TransactionDate = p.TransactionDate
            });
        }
    }
}

using AutoMapper;
using Mzad_Palestine_Core.DTO_s.Payment;
using Mzad_Palestine_Core.Interfaces.Services;
using Mzad_Palestine_Core.Models;
using Mzad_Palestine_Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mzad_Palestine_Core.Interfaces;

namespace Mzad_Palestine_Infrastructure.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _repository;
        private readonly IMapper _mapper;

        public PaymentService(IPaymentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PaymentDto> CreateAsync(CreatePaymentDto dto)
        {
            var payment = _mapper.Map<Payment>(dto);
            payment.CreatedAt = DateTime.UtcNow;
            payment.Status = PaymentStatus.Pending;

            await _repository.CreateAsync(payment);
            return _mapper.Map<PaymentDto>(payment);
        }

        public async Task<PaymentDto> GetByIdAsync(int id)
        {
            var payment = await _repository.GetByIdAsync(id);
            if (payment == null)
                throw new InvalidOperationException("الدفعة غير موجودة");

            return _mapper.Map<PaymentDto>(payment);
        }

        public async Task<IEnumerable<PaymentDto>> GetByUserIdAsync(int userId)
        {
            var payments = await _repository.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<PaymentDto>>(payments);
        }

        public async Task<IEnumerable<PaymentDto>> GetByAuctionIdAsync(int auctionId)
        {
            var payments = await _repository.GetByAuctionIdAsync(auctionId);
            return _mapper.Map<IEnumerable<PaymentDto>>(payments);
        }

        public async Task UpdateAsync(int id, UpdatePaymentDto dto)
        {
            var payment = await _repository.GetByIdAsync(id);
            if (payment == null)
                throw new InvalidOperationException("الدفعة غير موجودة");

            // Update only the provided fields
            if (dto.Amount.HasValue)
                payment.Amount = dto.Amount.Value;
            if (!string.IsNullOrEmpty(dto.Method))
                payment.Method = Enum.Parse<PaymentMethod>(dto.Method);
            if (!string.IsNullOrEmpty(dto.TransactionId))
            {
                payment.TransactionId = dto.TransactionId;
            }
            if (!string.IsNullOrEmpty(dto.Notes))
                payment.Notes = dto.Notes;
            if (dto.Status.HasValue)
                payment.Status = dto.Status.Value;

            payment.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(payment);
        }

        public async Task VerifyPaymentAsync(int id, int userId)
        {
            var payment = await _repository.GetByIdAsync(id);
            if (payment == null)
                throw new InvalidOperationException("الدفعة غير موجودة");

            if (payment.UserId != userId)
                throw new InvalidOperationException("غير مصرح لك بالتحقق من هذه الدفعة");

            payment.Status = PaymentStatus.Verified;
            payment.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(payment);
        }

        public async Task DeletePaymentAsync(int id, int userId)
        {
            var payment = await _repository.GetByIdAsync(id);
            if (payment == null)
                throw new InvalidOperationException("الدفعة غير موجودة");

            if (payment.UserId != userId)
                throw new InvalidOperationException("غير مصرح لك بحذف هذه الدفعة");

            await _repository.DeleteAsync(payment);
        }
    }
}

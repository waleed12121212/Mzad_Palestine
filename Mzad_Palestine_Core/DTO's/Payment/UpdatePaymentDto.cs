using System.ComponentModel.DataAnnotations;
using Mzad_Palestine_Core.Enums;

namespace Mzad_Palestine_Core.DTO_s.Payment
{
    public class UpdatePaymentDto
    {
        [Range(0.01, double.MaxValue, ErrorMessage = "يجب أن يكون المبلغ أكبر من صفر")]
        public decimal? Amount { get; set; }

        public string? Method { get; set; }
        public string? TransactionId { get; set; }
        public string? Notes { get; set; }
        public PaymentStatus? Status { get; set; }
    }
} 
using System.ComponentModel.DataAnnotations;
using Mzad_Palestine_Core.Enums;

namespace Mzad_Palestine_Core.DTO_s.Payment
{
    public class CreatePaymentDto
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "معرف المزاد مطلوب")]
        public int AuctionId { get; set; }

        [Required(ErrorMessage = "المبلغ مطلوب")]
        [Range(0.01, double.MaxValue, ErrorMessage = "يجب أن يكون المبلغ أكبر من صفر")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "طريقة الدفع مطلوبة")]
        public string Method { get; set; }

        public string? TransactionId { get; set; }
        public string? Notes { get; set; }
    }
}

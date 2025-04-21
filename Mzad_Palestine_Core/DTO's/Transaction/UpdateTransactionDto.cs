using System.ComponentModel.DataAnnotations;

namespace Mzad_Palestine_Core.DTO_s.Transaction
{
    public class UpdateTransactionDto
    {
        [Range(0.01, double.MaxValue, ErrorMessage = "يجب أن يكون المبلغ أكبر من صفر")]
        public decimal? Amount { get; set; }

        public string? TransactionType { get; set; }

        public string? Description { get; set; }

        public string? Status { get; set; }
    }
} 
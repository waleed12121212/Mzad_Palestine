using System.ComponentModel.DataAnnotations;

namespace Mzad_Palestine_Core.DTO_s.Transaction
{
    public class CreateTransactionDto
    {
        [Required(ErrorMessage = "معرف المستخدم مطلوب")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "المبلغ مطلوب")]
        [Range(0.01, double.MaxValue, ErrorMessage = "يجب أن يكون المبلغ أكبر من صفر")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "نوع المعاملة مطلوب")]
        public string TransactionType { get; set; }

        [Required(ErrorMessage = "الوصف مطلوب")]
        public string Description { get; set; }
    }
} 
using System.ComponentModel.DataAnnotations;

namespace Mzad_Palestine_Core.DTO_s.AutoBid
{
    public class UpdateAutoBidDto
    {
        [Required(ErrorMessage = "الحد الأقصى للمزايدة مطلوب")]
        [Range(1, double.MaxValue, ErrorMessage = "يجب أن يكون الحد الأقصى للمزايدة أكبر من صفر")]
        public decimal MaxBid { get; set; }
    }
} 
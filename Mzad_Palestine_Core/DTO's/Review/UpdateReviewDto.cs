using System.ComponentModel.DataAnnotations;

namespace Mzad_Palestine_Core.DTO_s.Review
{
    public class UpdateReviewDto
    {
        [Required(ErrorMessage = "التعليق مطلوب")]
        [MinLength(3, ErrorMessage = "التعليق يجب أن يكون 3 أحرف على الأقل")]
        public string Comment { get; set; }

        [Required(ErrorMessage = "التقييم مطلوب")]
        [Range(1, 5, ErrorMessage = "التقييم يجب أن يكون بين 1 و 5")]
        public int Rating { get; set; }
    }
} 
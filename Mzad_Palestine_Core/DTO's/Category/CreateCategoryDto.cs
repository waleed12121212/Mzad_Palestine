using System.ComponentModel.DataAnnotations;

namespace Mzad_Palestine_Core.DTOs.Category
{
    public class CreateCategoryDto
    {
        [Required(ErrorMessage = "اسم التصنيف مطلوب")]
        [StringLength(100, ErrorMessage = "اسم التصنيف يجب أن لا يتجاوز 100 حرف")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "الوصف يجب أن لا يتجاوز 500 حرف")]
        public string Description { get; set; }

        public int? ParentCategoryId { get; set; }
    }
} 
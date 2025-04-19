using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Mzad_Palestine_Core.DTOs.Listing
{
    public class CreateListingDto
    {
        [Required(ErrorMessage = "عنوان المنتج مطلوب")]
        [StringLength(100, ErrorMessage = "العنوان يجب أن لا يتجاوز 100 حرف")]
        public string Title { get; set; }

        [Required(ErrorMessage = "وصف المنتج مطلوب")]
        [StringLength(1000, ErrorMessage = "الوصف يجب أن لا يتجاوز 1000 حرف")]
        public string Description { get; set; }

        [Required(ErrorMessage = "السعر الابتدائي مطلوب")]
        [Range(0, double.MaxValue, ErrorMessage = "السعر يجب أن يكون أكبر من 0")]
        public decimal StartingPrice { get; set; }

        [Required(ErrorMessage = "التصنيف مطلوب")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "تاريخ انتهاء المزاد مطلوب")]
        public DateTime EndDate { get; set; }

        public List<IFormFile> Images { get; set; }
    }
} 
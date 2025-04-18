using System.ComponentModel.DataAnnotations;

namespace Mzad_Palestine_Core.DTO_s.Listing
{
    public class UpdateListingDto
    {
        [Required]
        public string Title { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        [Required]
        public decimal Price { get; set; }
        
        [Required]
        public int CategoryId { get; set; }
        
        [Required]
        public int LocationId { get; set; }
        public string? Location { get; set; }
        public string? Condition { get; set; }
        public bool IsActive { get; set; }
    }
}
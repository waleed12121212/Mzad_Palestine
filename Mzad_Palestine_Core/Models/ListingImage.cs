using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mzad_Palestine_Core.Models
{
    public class ListingImage
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public bool IsMainImage { get; set; }
        public int ListingId { get; set; }
        public DateTime CreatedAt { get; set; }

        [ForeignKey("ListingId")]
        public Listing Listing { get; set; }

        public ListingImage()
        {
            CreatedAt = DateTime.UtcNow;
            IsMainImage = false;
        }
    }
} 
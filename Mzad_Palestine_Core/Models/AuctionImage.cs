using System;

namespace Mzad_Palestine_Core.Models
{
    public class AuctionImage
    {
        public int ImageId { get; set; }
        public int AuctionId { get; set; }
        public string ImageUrl { get; set; }
        public bool IsMain { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public virtual Auction Auction { get; set; }
    }
} 
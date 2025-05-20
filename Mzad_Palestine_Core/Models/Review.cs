using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }
        public int ReviewerId { get; set; }
        public int ReviewedUserId { get; set; }
        public int? ListingId { get; set; }
        public int? AuctionId { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("ReviewerId")]
        public User Reviewer { get; set; }
        [ForeignKey("ReviewedUserId")]
        public User ReviewedUser { get; set; }
        [ForeignKey("ListingId")]
        public Listing Listing { get; set; }
        [ForeignKey("AuctionId")]
        public Auction Auction { get; set; }
    }
}

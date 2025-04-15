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
        public int Id { get; set; }  // Changed from ReviewId
        public int ReviewerId { get; set; }
        public int ReviewedUserId { get; set; }  // Changed from RevieweeId
        public int ListingId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public User Reviewer { get; set; }
        public User ReviewedUser { get; set; }  // Changed from Reviewee
        public Listing Listing { get; set; }
    }
}

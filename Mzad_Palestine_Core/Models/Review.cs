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
        public int ReviewId { get; set; }
        public int ReviewerId { get; set; }
        public int RevieweeId { get; set; }
        public int ListingId { get; set; }
        public int Rating { get; set; } // بين 1 و 5
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }

        // الملاحة
        public User Reviewer { get; set; }
        public User Reviewee { get; set; }
        public Listing Listing { get; set; }
    }
}

using Mzad_Palestine_Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Models
{
    public class Listing
    {
        public int ListingId { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public int LocationId { get; set; }
        public ListingType Type { get; set; }
        public ListingStatus Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // الملاحة
        // Navigation Properties
        public virtual User User { get; set; }
        public virtual ICollection<Watchlist> Watchlists { get; set; } = new HashSet<Watchlist>();
        public virtual ICollection<Report> Reports { get; set; } = new HashSet<Report>();
        public virtual Auction Auction { get; set; }
        public virtual ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
        public virtual ICollection<ListingTag> ListingTags { get; set; } = new HashSet<ListingTag>();
        public virtual ICollection<Invoice> Invoices { get; set; } = new HashSet<Invoice>();

        public Listing()
        {
            Watchlists = new HashSet<Watchlist>();
            Reports = new HashSet<Report>();
            Reviews = new HashSet<Review>();
            ListingTags = new HashSet<ListingTag>();
            Invoices = new HashSet<Invoice>();
        }
    }
}

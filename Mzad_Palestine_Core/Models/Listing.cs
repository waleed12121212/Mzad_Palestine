using Mzad_Palestine_Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mzad_Palestine_Core.Models
{
    public class Listing
    {
        public int ListingId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal StartingPrice { get; set; }
        public int CategoryId { get; set; }
        public int UserId { get; set; }
        public ListingStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsSold { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public virtual Auction Auction { get; set; }
        public virtual ICollection<Bid> Bids { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
        public virtual ICollection<ListingImage> Images { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
        public virtual ICollection<ListingTag> ListingTags { get; set; }
        public virtual ICollection<Watchlist> Watchlists { get; set; }

        public Listing()
        {
            Bids = new HashSet<Bid>();
            Reviews = new HashSet<Review>();
            Reports = new HashSet<Report>();
            Images = new HashSet<ListingImage>();
            Invoices = new HashSet<Invoice>();
            ListingTags = new HashSet<ListingTag>();
            Watchlists = new HashSet<Watchlist>();
            CreatedAt = DateTime.UtcNow;
            IsActive = true;
            IsSold = false;
            Status = ListingStatus.Active;
        }
    }
}

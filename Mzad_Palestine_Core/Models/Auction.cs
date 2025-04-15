using Mzad_Palestine_Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Models
{
    public class Auction
    {
        public int AuctionId { get; set; }
        public int ListingId { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; } = DateTime.UtcNow;
        public DateTime EndTime { get; set; }
        public decimal ReservePrice { get; set; }
        public decimal CurrentBid { get; set; }
        public decimal BidIncrement { get; set; }
        public int? WinnerId { get; set; }
        public AuctionStatus Status { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // الملاحة
        public Listing Listing { get; set; }
        public User Winner { get; set; }
        public ICollection<Bid> Bids { get; set; } = new HashSet<Bid>();
        public ICollection<Payment> Payments { get; set; } = new HashSet<Payment>();
        public ICollection<AutoBid> AutoBids { get; set; } = new HashSet<AutoBid>();
        public ICollection<Dispute> Disputes { get; set; } = new HashSet<Dispute>();
    }
}

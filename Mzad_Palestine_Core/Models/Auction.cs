using Mzad_Palestine_Core.Enums;
using System;
using System.Collections.Generic;
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
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal ReservePrice { get; set; }
        public decimal CurrentBid { get; set; }
        public decimal BidIncrement { get; set; }
        public int? WinnerId { get; set; }
        public AuctionStatus Status { get; set; }
        public string ImageUrl { get; set; }

        // الملاحة
        public Listing Listing { get; set; }
        public User Winner { get; set; }
        public ICollection<Bid> Bids { get; set; }
        public ICollection<Payment> Payments { get; set; }
        public ICollection<AutoBid> AutoBids { get; set; }
        public ICollection<Dispute> Disputes { get; set; }
    }
}

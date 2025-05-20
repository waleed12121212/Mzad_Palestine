using Mzad_Palestine_Core.Enums;
using Mzad_Palestine_Core.Extensions;
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
        public string Title { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public decimal ReservePrice { get; set; }
        public decimal CurrentBid { get; set; }
        public decimal BidIncrement { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Foreign Keys
        public int UserId { get; set; }
        public int? WinnerId { get; set; }
        public int CategoryId { get; set; }

        // Navigation properties
        public virtual User User { get; set; }
        public virtual User Winner { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<Bid> Bids { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<Dispute> Disputes { get; set; }
        public virtual ICollection<AutoBid> AutoBids { get; set; }
        public virtual ICollection<AuctionImage> Images { get; set; }
    }
}

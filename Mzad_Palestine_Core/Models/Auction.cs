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
        public int ListingId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }

        private DateTime _startTime;
        public DateTime StartTime
        {
            get => _startTime.ToPalestineTime();
            set => _startTime = value.ToUtcFromPalestine();
        }

        private DateTime _endTime;
        public DateTime EndTime
        {
            get => _endTime.ToPalestineTime();
            set => _endTime = value.ToUtcFromPalestine();
        }

        public decimal ReservePrice { get; set; }
        public decimal CurrentBid { get; set; }
        public decimal BidIncrement { get; set; }
        public int? WinnerId { get; set; }
        public AuctionStatus Status { get; set; }
        public string ImageUrl { get; set; }

        private DateTime _createdAt;
        public DateTime CreatedAt
        {
            get => _createdAt.ToPalestineTime();
            set => _createdAt = value.ToUtcFromPalestine();
        }

        private DateTime? _updatedAt;
        public DateTime? UpdatedAt
        {
            get => _updatedAt?.ToPalestineTime();
            set => _updatedAt = value?.ToUtcFromPalestine();
        }

        // الملاحة
        public Listing Listing { get; set; }
        public User User { get; set; }
        public User Winner { get; set; }
        public ICollection<Bid> Bids { get; set; } = new HashSet<Bid>();
        public ICollection<Payment> Payments { get; set; } = new HashSet<Payment>();
        public ICollection<AutoBid> AutoBids { get; set; } = new HashSet<AutoBid>();
        public ICollection<Dispute> Disputes { get; set; } = new HashSet<Dispute>();
    }
}

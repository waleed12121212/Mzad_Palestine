using Mzad_Palestine_Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Models
{
    public class Bid
    {
        public int BidId { get; set; }
        public int AuctionId { get; set; }
        public int UserId { get; set; }
        public decimal BidAmount { get; set; }
        public DateTime BidTime { get; set; } = DateTime.UtcNow;
        public bool IsAutoBid { get; set; }
        public bool IsWinner { get; set; }
        public BidStatus Status { get; set; }

        // الملاحة
        public Auction Auction { get; set; }
        public User User { get; set; }
    }

}

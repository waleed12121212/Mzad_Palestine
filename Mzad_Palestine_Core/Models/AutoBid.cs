using Mzad_Palestine_Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Models
{
    public class AutoBid
    {
        public int AutoBidId { get; set; }
        public int UserId { get; set; }
        public int AuctionId { get; set; }
        public decimal MaxBid { get; set; }
        public decimal CurrentBid { get; set; }
        public AutoBidStatus Status { get; set; }
        public User User { get; set; }
        public Auction Auction { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.DTO_s.Bid
{
    public class BidDto
    {
        public int Id { get; set; }
        public int AuctionId { get; set; }
        public int UserId { get; set; }
        public decimal BidAmount { get; set; }
        public DateTime BidTime { get; set; }
        public bool IsAutoBid { get; set; }
        public bool IsWinner { get; set; }
        public string Status { get; set; }

        public BidDto(int id, int auctionId, int userId, decimal bidAmount, DateTime bidTime, bool isAutoBid, bool isWinner, string status)
        {
            Id = id;
            AuctionId = auctionId;
            UserId = userId;
            BidAmount = bidAmount;
            BidTime = bidTime;
            IsAutoBid = isAutoBid;
            IsWinner = isWinner;
            Status = status;
        }
    }
}

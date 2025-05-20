using System;

namespace Mzad_Palestine_Core.DTO_s.Auction
{
    public class BidDto
    {
        public int Id { get; set; }
        public int AuctionId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public decimal Amount { get; set; }
        public DateTime BidTime { get; set; }
        public bool IsWinningBid { get; set; }
    }
} 
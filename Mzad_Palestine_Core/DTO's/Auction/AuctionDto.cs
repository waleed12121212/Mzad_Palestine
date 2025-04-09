using Mzad_Palestine_Core.Enums;
using System;

namespace Mzad_Palestine_Core.DTO_s.Auction
{
    public class AuctionDto
    {
        public int Id { get; set; }
        public int ListingId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal ReservePrice { get; set; }
        public decimal CurrentBid { get; set; }
        public decimal BidIncrement { get; set; }
        public int? WinnerId { get; set; }
        public AuctionStatus Status { get; set; }
        public string ImageUrl { get; set; }
    }
}
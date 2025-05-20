using System;
using System.Collections.Generic;

namespace Mzad_Palestine_Core.DTOs.Auction
{
    public class AuctionResponseDto
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
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<string> Images { get; set; }
        public List<BidDto> Bids { get; set; }
    }

    public class BidDto
    {
        public int BidId { get; set; }
        public decimal BidAmount { get; set; }
        public DateTime BidTime { get; set; }
        public int UserId { get; set; }
        public bool IsWinner { get; set; }
    }
} 
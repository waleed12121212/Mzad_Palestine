using Mzad_Palestine_Core.Enums;
using System;
using System.Collections.Generic;

namespace Mzad_Palestine_Core.DTO_s.Auction
{
    public class AuctionDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal ReservePrice { get; set; }
        public decimal CurrentBid { get; set; }
        public decimal BidIncrement { get; set; }
        public int? WinnerId { get; set; }
        public string Status { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<string> Images { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<BidDto> Bids { get; set; }
    }
}
using System;

namespace Mzad_Palestine_Core.DTO_s.Dispute
{
    public class CreateDisputeDto
    {
        public int UserId { get; set; }
        public int AuctionId { get; set; }
        public string Reason { get; set; }
    }
}
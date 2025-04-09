using Mzad_Palestine_Core.Enums;
using System;

namespace Mzad_Palestine_Core.DTO_s.Dispute
{
    public class DisputeDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AuctionId { get; set; }
        public string Reason { get; set; }
        public DisputeStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? ResolvedBy { get; set; }
    }
}
using Mzad_Palestine_Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Models
{
    public class Dispute
    {
        public int DisputeId { get; set; }
        public int UserId { get; set; }
        public int AuctionId { get; set; }
        public string Reason { get; set; }
        public DisputeStatus Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int? ResolvedBy { get; set; }

        // الملاحة
        public User User { get; set; }
        public Auction Auction { get; set; }
        public User Resolver { get; set; }
    }
}

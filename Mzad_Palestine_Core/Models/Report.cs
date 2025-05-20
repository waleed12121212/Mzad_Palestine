using Mzad_Palestine_Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Models
{
    public class Report
    {
        public int ReportId { get; set; }
        public string Reason { get; set; }
        public int ReporterId { get; set; }
        public int? ReportedListingId { get; set; }
        public int? ReportedAuctionId { get; set; }
        public string ReportType { get; set; } // "Listing" or "Auction"
        public string Status { get; set; } = "Pending";
        public int? ResolvedBy { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public string Resolution { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public User Reporter { get; set; }
        public User Resolver { get; set; }
        public Listing ReportedListing { get; set; }
        public Auction ReportedAuction { get; set; }
    }
}

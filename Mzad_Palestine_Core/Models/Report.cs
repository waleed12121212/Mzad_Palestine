﻿using Mzad_Palestine_Core.Enums;
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
        public DateTime CreatedAt { get; set; }
        public int ReporterId { get; set; }
        public int ReportedListingId { get; set; }
        public int? ResolvedBy { get; set; }
        
        // Status as int in database
        public int StatusId { get; set; }

        [NotMapped]
        public string Status
        {
            get => StatusId switch
            {
                0 => "Pending",
                1 => "Resolved",
                2 => "Rejected",
                _ => "Unknown"
            };
            set => StatusId = value switch
            {
                "Pending" => 0,
                "Resolved" => 1,
                "Rejected" => 2,
                _ => 0
            };
        }

        // Navigation Properties
        public virtual User Reporter { get; set; }
        public virtual Listing ReportedListing { get; set; }
        public virtual User Resolver { get; set; }
    }
}

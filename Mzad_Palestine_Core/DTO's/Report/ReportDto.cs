using System;

namespace Mzad_Palestine_Core.DTOs
{
    public class ReportDto
    {
        public int ReportId { get; set; }
        public string Reason { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ReporterId { get; set; }
        public int ReportedListingId { get; set; }
        public int? ResolvedBy { get; set; }
        
        // Additional properties for display
        public string ReporterName { get; set; }
        public string ReportedListingTitle { get; set; }
        public string ResolverName { get; set; }
    }

    public class CreateReportDto
    {
        public string Reason { get; set; }
        public int ReporterId { get; set; }
        public int ReportedListingId { get; set; }
    }

    public class UpdateReportDto
    {
        public string Reason { get; set; }
        public int? ResolvedBy { get; set; }
    }
} 
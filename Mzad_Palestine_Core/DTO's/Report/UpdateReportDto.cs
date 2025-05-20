using System;

namespace Mzad_Palestine_Core.DTOs.Report
{
    public class UpdateReportDto
    {
        public string Reason { get; set; }
        public string Status { get; set; }
        public string Resolution { get; set; }
        public int? ResolvedBy { get; set; }
    }
} 
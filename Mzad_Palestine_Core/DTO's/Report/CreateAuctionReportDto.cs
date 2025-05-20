using System.ComponentModel.DataAnnotations;

namespace Mzad_Palestine_Core.DTOs.Report
{
    public class CreateAuctionReportDto
    {
        [Required]
        [StringLength(500)]
        public string Reason { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }
    }
} 
using System;

namespace Mzad_Palestine_Core.DTOs.Listing
{
    public class ListingSearchDto
    {
        public string Keyword { get; set; }
        public string Category { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Status { get; set; }
        public int? UserId { get; set; }
    }
} 
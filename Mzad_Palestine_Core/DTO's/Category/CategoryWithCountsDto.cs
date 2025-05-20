using System.Collections.Generic;

namespace Mzad_Palestine_Core.DTOs.Category
{
    public class CategoryWithCountsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ListingCount { get; set; }
        public int AuctionCount { get; set; }
        public List<int> ListingIds { get; set; }
        public List<int> AuctionIds { get; set; }
    }
} 
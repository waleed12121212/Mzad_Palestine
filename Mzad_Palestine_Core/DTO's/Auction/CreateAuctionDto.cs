using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.DTO_s.Auction
{
    public class CreateAuctionDto
    {
        public int ListingId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal ReservePrice { get; set; }
        public decimal BidIncrement { get; set; }
        public string ImageUrl { get; set; }
    }
}

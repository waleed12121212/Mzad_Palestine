using Mzad_Palestine_Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.DTO_s.Auction
{
    public class UpdateAuctionDto
    {
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public decimal? ReservePrice { get; set; }
        public decimal? BidIncrement { get; set; }
        public string ImageUrl { get; set; }
        public AuctionStatus? Status { get; set; }
    }
}

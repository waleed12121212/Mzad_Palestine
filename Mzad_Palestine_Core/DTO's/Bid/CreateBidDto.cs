using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.DTO_s.Bid
{
    public class CreateBidDto
    {
        public int AuctionId { get; set; }
        public decimal BidAmount { get; set; }
    }
}

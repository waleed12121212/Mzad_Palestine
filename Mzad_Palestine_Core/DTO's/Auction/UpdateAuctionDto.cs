using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.DTO_s.Auction
{
    public class UpdateAuctionDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime EndTime { get; set; }
        public decimal StartingPrice { get; set; }
    }
}

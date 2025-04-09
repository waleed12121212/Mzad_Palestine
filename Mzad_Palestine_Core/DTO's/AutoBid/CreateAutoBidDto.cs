using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.DTO_s.AutoBid
{
    public class CreateAutoBidDto
    {
        public int UserId { get; set; }
        public int AuctionId { get; set; }
        public decimal MaxBid { get; set; }
    }
}

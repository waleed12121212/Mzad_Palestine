using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.DTO_s.Dispute
{
    public class CreateDisputeDto
    {
        public int AuctionId { get; set; }
        public string Reason { get; set; }
    }
}

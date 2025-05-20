using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.DTO_s.Watchlist
{
    public class WatchlistDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? ListingId { get; set; }
        public int? AuctionId { get; set; }
        public DateTime AddedAt { get; set; }
    }
}

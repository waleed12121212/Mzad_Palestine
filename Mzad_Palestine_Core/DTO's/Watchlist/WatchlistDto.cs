using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.DTO_s.Watchlist
{
    public class WatchlistDto
    {
        public int WatchlistId { get; set; }
        public int UserId { get; set; }
        public int ListingId { get; set; }
        public DateTime AddedAt { get; set; }
        public string ListingTitle { get; set; }
        public decimal ListingPrice { get; set; }
        public string ListingImage { get; set; }
    }
}

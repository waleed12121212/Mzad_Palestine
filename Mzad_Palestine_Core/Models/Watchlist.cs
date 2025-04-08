using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Models
{
    public class Watchlist
    {
        public int WatchlistId { get; set; }
        public int UserId { get; set; }
        public int ListingId { get; set; }
        public DateTime AddedAt { get; set; }

        // الملاحة
        public User User { get; set; }
        public Listing Listing { get; set; }
    }
}

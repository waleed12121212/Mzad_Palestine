using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Models
{
    public class Watchlist
    {
        public int WatchlistId { get; set; }
        public int UserId { get; set; }
        public int? ListingId { get; set; }
        public int? AuctionId { get; set; }
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("UserId")]
        public User User { get; set; }
        [ForeignKey("ListingId")]
        public Listing Listing { get; set; }
        [ForeignKey("AuctionId")]
        public Auction Auction { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.DTO_s.Bid
{
    public class BidDto
    {
        public int Id { get; set; }
        public int AuctionId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public decimal BidAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsWinning { get; set; }

        public BidDto(int id, int auctionId, string userId, string userName, decimal bidAmount, DateTime createdAt, bool isWinning)
        {
            Id = id;
            AuctionId = auctionId;
            UserId = userId;
            UserName = userName;
            BidAmount = bidAmount;
            CreatedAt = createdAt;
            IsWinning = isWinning;
        }
    }
}

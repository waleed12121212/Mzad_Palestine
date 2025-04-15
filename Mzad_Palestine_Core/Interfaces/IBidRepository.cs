using Mzad_Palestine_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Interfaces
{
    public interface IBidRepository : IGenericRepository<Bid>
    {
        Task<IEnumerable<Bid>> GetBidsByAuctionAsync(int auctionId);
        Task<Bid> GetHighestBidAsync(int auctionId);
        Task<IEnumerable<Bid>> GetAuctionBidsAsync(int auctionId);
        Task<Bid> GetWinningBidAsync(int auctionId);
    }
}

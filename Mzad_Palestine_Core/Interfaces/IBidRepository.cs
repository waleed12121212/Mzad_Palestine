using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Mzad_Palestine_Core.Models;

namespace Mzad_Palestine_Core.Interfaces
{
    public interface IBidRepository : IGenericRepository<Bid>
    {
        IQueryable<Bid> Find(Expression<Func<Bid, bool>> predicate);
        Task<IEnumerable<Bid>> GetBidsByAuctionAsync(int auctionId);
        Task<Bid> GetHighestBidAsync(int auctionId);
        Task<IEnumerable<Bid>> GetAuctionBidsAsync(int auctionId);
        Task<Bid> GetWinningBidAsync(int auctionId);
        Task<IEnumerable<Bid>> GetByAuctionAsync(int auctionId);
    }
}

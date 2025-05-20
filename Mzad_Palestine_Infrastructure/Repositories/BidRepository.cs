using Microsoft.EntityFrameworkCore;
using Mzad_Palestine_Core.Interfaces;
using Mzad_Palestine_Core.Models;
using Mzad_Palestine_Infrastructure.Data;
using Mzad_Palestine_Infrastructure.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mzad_Palestine_Infrastructure.Repositories
{
    public class BidRepository : GenericRepository<Bid>, IBidRepository
    {
        private readonly DbSet<Bid> _bids;

        public BidRepository(ApplicationDbContext context) : base(context)
        {
            _bids = context.Set<Bid>();
        }

        public IQueryable<Bid> Find(Expression<Func<Bid, bool>> predicate)
        {
            return _bids.Where(predicate);
        }

        public async Task<IEnumerable<Bid>> GetBidsByAuctionAsync(int auctionId)
        {
            return await _bids
                .Where(b => b.AuctionId == auctionId)
                .Include(b => b.User)
                .OrderByDescending(b => b.BidAmount)
                .ToListAsync();
        }

        public async Task<Bid> GetHighestBidAsync(int auctionId)
        {
            return await _bids
                .Where(b => b.AuctionId == auctionId)
                .OrderByDescending(b => b.BidAmount)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Bid>> GetAuctionBidsAsync(int auctionId)
        {
            return await _bids
                .Where(b => b.AuctionId == auctionId)
                .Include(b => b.User)
                .OrderByDescending(b => b.BidTime)
                .ToListAsync();
        }

        public async Task<Bid> GetWinningBidAsync(int auctionId)
        {
            return await _bids
                .Where(b => b.AuctionId == auctionId)
                .OrderByDescending(b => b.BidAmount)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Bid>> GetByAuctionAsync(int auctionId)
        {
            return await _bids
                .Where(b => b.AuctionId == auctionId)
                .Include(b => b.User)
                .OrderByDescending(b => b.BidTime)
                .ToListAsync();
        }
    }
}

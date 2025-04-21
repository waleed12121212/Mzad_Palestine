using Microsoft.EntityFrameworkCore;
using Mzad_Palestine_Core.Interfaces;
using Mzad_Palestine_Core.Models;
using Mzad_Palestine_Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mzad_Palestine_Infrastructure.Repositories
{
    public class BidRepository : IBidRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Bid> _bids;

        public BidRepository(ApplicationDbContext context)
        {
            _context = context;
            _bids = context.Set<Bid>();
        }

        public async Task<Bid> GetByIdAsync(int id)
        {
            return await _bids.FindAsync(id);
        }

        public async Task<Bid> GetByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Bid>> GetAllAsync()
        {
            return await _bids.ToListAsync();
        }

        public async Task<IEnumerable<Bid>> FindAsync(Expression<Func<Bid, bool>> predicate)
        {
            return await _bids.Where(predicate).ToListAsync();
        }

        public IQueryable<Bid> Find(Expression<Func<Bid, bool>> predicate)
        {
            return _bids.Where(predicate);
        }

        public async Task AddAsync(Bid entity)
        {
            await _bids.AddAsync(entity);
        }

        public void Update(Bid entity)
        {
            _bids.Update(entity);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _bids.AnyAsync(b => b.BidId == id);
        }

        public async Task DeleteAsync(Bid entity)
        {
            _bids.Remove(entity);
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
    }
}

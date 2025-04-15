using Microsoft.EntityFrameworkCore;
using Mzad_Palestine_Core.DTO_s.Auction;
using Mzad_Palestine_Core.Enums;
using Mzad_Palestine_Core.Interfaces;
using Mzad_Palestine_Core.Models;
using Mzad_Palestine_Infrastructure.Data;
using Mzad_Palestine_Infrastructure.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Infrastructure.Repositories
{
    public class AuctionRepository : GenericRepository<Auction>, IAuctionRepository
    {
        public AuctionRepository(ApplicationDbContext context) : base(context) { }

        public async Task CloseAuctionAsync(int auctionId)
        {
            var auction = await _context.Auctions.FindAsync(auctionId);
            if (auction != null)
            {
                auction.Status = AuctionStatus.Closed;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var auction = await _context.Auctions.FindAsync(id);
            if (auction != null)
            {
                _context.Auctions.Remove(auction);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Auction>> GetActiveAsync()
        {
            return await _context.Auctions
                .Where(a => a.Status == AuctionStatus.Open && a.EndTime > DateTime.UtcNow)
                .Include(a => a.Listing)
                .ToListAsync();
        }

        public async Task<Auction> GetAuctionWithBidsAsync(int auctionId)
        {
            return await _context.Auctions
                .Include(a => a.Bids)
                .Include(a => a.Listing)
                .FirstOrDefaultAsync(a => a.AuctionId == auctionId);
        }

        public async Task<IEnumerable<Auction>> GetByUserIdAsync(int userId)
        {
            return await _context.Auctions
                .Include(a => a.Listing)
                .Where(a => a.Listing.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Auction>> GetClosedAuctionsAsync()
        {
            return await _context.Auctions
                .Where(a => a.Status == AuctionStatus.Closed)
                .Include(a => a.Listing)
                .ToListAsync();
        }

        public async Task<DateTime?> GetEndTimeAsync(int auctionId)
        {
            var auction = await _context.Auctions.FindAsync(auctionId);
            return auction?.EndTime;
        }

        public async Task<IEnumerable<Auction>> GetOpenAuctionsAsync()
        {
            return await _context.Auctions
                .Where(a => a.Status == AuctionStatus.Open)
                .Include(a => a.Listing)
                .ToListAsync();
        }

        public async Task<bool> IsAuctionOwnerAsync(int auctionId, int userId)
        {
            var auction = await _context.Auctions
                .Include(a => a.Listing)
                .FirstOrDefaultAsync(a => a.AuctionId == auctionId);
            
            return auction?.Listing.UserId == userId;
        }

        public async Task<IEnumerable<Auction>> SearchAsync(AuctionSearchDto searchDto)
        {
            var query = _context.Auctions
                .Include(a => a.Listing)
                .AsQueryable();

            if (searchDto.MinPrice.HasValue)
            {
                query = query.Where(a => a.CurrentBid >= searchDto.MinPrice.Value);
            }

            if (searchDto.MaxPrice.HasValue)
            {
                query = query.Where(a => a.CurrentBid <= searchDto.MaxPrice.Value);
            }

            return await query.ToListAsync();
        }

        public async Task UpdateAsync(Auction auction)
        {
            _context.Entry(auction).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}

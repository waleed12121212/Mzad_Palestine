using Microsoft.EntityFrameworkCore;
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

        public async Task<Auction> GetAuctionWithBidsAsync(int auctionId)
        {
            return await _context.Auctions
                .Include(a => a.Bids)
                .FirstOrDefaultAsync(a => a.AuctionId == auctionId);
        }

        public async Task<IEnumerable<Auction>> GetOpenAuctionsAsync( )
        {
            return await _context.Auctions
                .Where(a => a.Status == AuctionStatus.Open)
                .ToListAsync();
        }

        public async Task CloseAuctionAsync(int auctionId)
        {
            var auction = await GetByIdAsync(auctionId);
            if (auction != null)
            {
                auction.Status = AuctionStatus.Closed;
                Update(auction);
            }
        }
    }
}

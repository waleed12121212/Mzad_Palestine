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
using Microsoft.Extensions.Logging;

namespace Mzad_Palestine_Infrastructure.Repositories
{
    public class AuctionRepository : GenericRepository<Auction>, IAuctionRepository
    {
        private readonly ILogger<AuctionRepository> _logger;

        public AuctionRepository(ApplicationDbContext context, ILogger<AuctionRepository> logger) : base(context)
        {
            _logger = logger;
        }

        public async Task CloseAuctionAsync(int auctionId)
        {
            var auction = await _context.Auctions
                .Include(a => a.Bids)
                .FirstOrDefaultAsync(a => a.AuctionId == auctionId);

            if (auction != null)
            {
                // تحديد أعلى وأحدث مزايدة
                var highestBid = auction.Bids?
                    .OrderByDescending(b => b.BidAmount)
                    .ThenByDescending(b => b.BidTime)
                    .FirstOrDefault();
                if (highestBid != null)
                {
                    auction.WinnerId = highestBid.UserId;
                }

                auction.Status = AuctionStatus.Closed.ToString();
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
            return await GetOpenAuctionsAsync();
        }

        public async Task<Auction> GetAuctionWithBidsAsync(int auctionId)
        {
            var auction = await _context.Auctions
                .Include(a => a.Category)
                .Include(a => a.Bids)
                .Include(a => a.Winner)
                .Include(a => a.Images)
                .FirstOrDefaultAsync(a => a.AuctionId == auctionId);

            if (auction == null)
                return null;

            var now = DateTime.UtcNow;
            bool updated = false;

            if (auction.Status == "Pending" && now >= auction.StartDate && now < auction.EndDate)
            {
                auction.Status = "Open";
                updated = true;
            }
            else if (auction.Status != "Closed" && now >= auction.EndDate)
            {
                auction.Status = "Closed";
                updated = true;
            }

            if (updated)
            {
                _context.Entry(auction).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }

            return auction;
        }

        public async Task<IEnumerable<Auction>> GetByUserIdAsync(int userId)
        {
            return await _context.Auctions
                .Include(a => a.Category)
                .Include(a => a.Bids)
                .Include(a => a.Winner)
                .Include(a => a.Images)
                .Where(a => a.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Auction>> GetClosedAuctionsAsync()
        {
            var auctions = await _context.Auctions
                .Include(a => a.Category)
                .Include(a => a.Bids)
                .Include(a => a.Winner)
                .Include(a => a.Images)
                .ToListAsync();

            var now = DateTime.UtcNow;
            bool updated = false;

            foreach (var auction in auctions)
            {
                if (auction.Status != "Closed" && now >= auction.EndDate)
                {
                    auction.Status = "Closed";
                    updated = true;
                }
                else if (auction.Status == "Pending" && now >= auction.StartDate && now < auction.EndDate)
                {
                    auction.Status = "Open";
                    updated = true;
                }
            }

            if (updated)
                await _context.SaveChangesAsync();

            return auctions.Where(a => a.Status == "Closed").ToList();
        }

        public async Task<DateTime?> GetEndTimeAsync(int auctionId)
        {
            var auction = await _context.Auctions.FindAsync(auctionId);
            return auction?.EndDate;
        }

        public async Task<IEnumerable<Auction>> GetOpenAuctionsAsync()
        {
            var auctions = await _context.Auctions
                .Include(a => a.Category)
                .Include(a => a.Bids)
                .Include(a => a.Winner)
                .Include(a => a.Images)
                .ToListAsync();

            var now = DateTime.UtcNow;
            bool updated = false;

            foreach (var auction in auctions)
            {
                if (auction.Status == "Pending" && now >= auction.StartDate && now < auction.EndDate)
                {
                    auction.Status = "Open";
                    updated = true;
                }
                else if (auction.Status != "Closed" && now >= auction.EndDate)
                {
                    auction.Status = "Closed";
                    updated = true;
                }
            }

            if (updated)
                await _context.SaveChangesAsync();

            return auctions.Where(a => a.Status == "Open").ToList();
        }

        public async Task<bool> IsAuctionOwnerAsync(int auctionId, int userId)
        {
            var auction = await _context.Auctions
                .FirstOrDefaultAsync(a => a.AuctionId == auctionId);
            
            return auction?.UserId == userId;
        }

        public async Task<IEnumerable<Auction>> SearchAsync(AuctionSearchDto searchDto)
        {
            var query = _context.Auctions
                .Include(a => a.Category)
                .Include(a => a.Bids)
                .Include(a => a.Winner)
                .Include(a => a.Images)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchDto.Keyword))
            {
                query = query.Where(a => a.Title.ToLower().Contains(searchDto.Keyword.ToLower()));
            }

            if (!string.IsNullOrEmpty(searchDto.Category))
            {
                query = query.Where(a => a.Category.Name.ToLower().Contains(searchDto.Category.ToLower()));
            }

            if (searchDto.MinPrice.HasValue)
            {
                query = query.Where(a => a.ReservePrice >= searchDto.MinPrice.Value || 
                                      (a.CurrentBid > 0 && a.CurrentBid >= searchDto.MinPrice.Value));
            }

            if (searchDto.MaxPrice.HasValue)
            {
                query = query.Where(a => a.ReservePrice <= searchDto.MaxPrice.Value || 
                                      (a.CurrentBid > 0 && a.CurrentBid <= searchDto.MaxPrice.Value));
            }

            if (searchDto.StartDate.HasValue)
            {
                query = query.Where(a => a.StartDate >= searchDto.StartDate.Value);
            }

            if (searchDto.EndDate.HasValue)
            {
                query = query.Where(a => a.EndDate <= searchDto.EndDate.Value);
            }

            if (searchDto.Status.HasValue)
            {
                query = query.Where(a => a.Status == searchDto.Status.Value.ToString());
            }

            if (searchDto.UserId.HasValue)
            {
                query = query.Where(a => a.UserId == searchDto.UserId.Value);
            }

            return await query.ToListAsync();
        }

        public IQueryable<Auction> GetQueryable()
        {
            return _context.Auctions
                .Include(a => a.Category)
                .Include(a => a.User)
                .Include(a => a.Winner)
                .Include(a => a.Bids)
                .Include(a => a.Payments)
                .Include(a => a.AutoBids)
                .Include(a => a.Disputes)
                .AsQueryable();
        }

        public override void Update(Auction entity)
        {
            base.Update(entity);
        }

        public async Task UpdateAsync(Auction auction)
        {
            _context.Entry(auction).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<Auction> GetByNameAsync(string name)
        {
            throw new NotImplementedException("Auctions cannot be searched by name. Please use other search criteria such as ID or associated listing.");
        }

        public async Task<Auction> GetByIdAsync(int auctionId)
        {
            var auction = await _context.Auctions
                .Include(a => a.Category)
                .Include(a => a.Bids)
                .Include(a => a.Winner)
                .FirstOrDefaultAsync(a => a.AuctionId == auctionId);

            if (auction == null)
                return null;

            var now = DateTime.UtcNow;
            bool updated = false;

            if (auction.Status == "Pending" && now >= auction.StartDate && now < auction.EndDate)
            {
                auction.Status = "Open";
                updated = true;
            }
            else if (auction.Status != "Closed" && now >= auction.EndDate)
            {
                auction.Status = "Closed";
                updated = true;
            }

            if (updated)
            {
                _context.Entry(auction).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }

            return auction;
        }

        public async Task<bool> ExistsAsync(int auctionId)
        {
            return await _context.Auctions.AnyAsync(a => a.AuctionId == auctionId);
        }

        public async Task<Auction> GetByIdWithDetailsAsync(int auctionId)
        {
            var auction = await _context.Auctions
                .Include(a => a.Category)
                .Include(a => a.Bids)
                    .ThenInclude(b => b.User)
                .Include(a => a.Winner)
                .Include(a => a.Images)
                .Include(a => a.AutoBids)
                .Include(a => a.Payments)
                .Include(a => a.Disputes)
                .FirstOrDefaultAsync(a => a.AuctionId == auctionId);

            if (auction == null)
                return null;

            var now = DateTime.UtcNow;
            bool updated = false;

            if (auction.Status == "Pending" && now >= auction.StartDate && now < auction.EndDate)
            {
                auction.Status = "Open";
                updated = true;
            }
            else if (auction.Status != "Closed" && now >= auction.EndDate)
            {
                auction.Status = "Closed";
                updated = true;
            }

            if (updated)
            {
                _context.Entry(auction).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }

            return auction;
        }

        public async Task AddImageAsync(AuctionImage image)
        {
            await _context.AuctionImages.AddAsync(image);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveImagesAsync(int auctionId)
        {
            var images = await _context.AuctionImages
                .Where(ai => ai.AuctionId == auctionId)
                .ToListAsync();
            
            _context.AuctionImages.RemoveRange(images);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Auction>> GetByCategoryAsync(int categoryId)
        {
            return await _context.Auctions
                .Include(a => a.Category)
                .Include(a => a.Bids)
                .Include(a => a.Winner)
                .Include(a => a.Images)
                .Where(a => a.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Auction>> GetByUserAsync(int userId)
        {
            return await _context.Auctions
                .Include(a => a.Category)
                .Include(a => a.Bids)
                .Include(a => a.Winner)
                .Include(a => a.Images)
                .Where(a => a.UserId == userId)
                .ToListAsync();
        }

        public async Task<Category> GetCategoryAsync(int auctionId)
        {
            var auction = await _context.Auctions
                .Include(a => a.Category)
                .FirstOrDefaultAsync(a => a.AuctionId == auctionId);
            
            return auction?.Category;
        }

        public async Task<IEnumerable<AuctionImage>> GetAuctionImagesAsync(int auctionId)
        {
            return await _context.AuctionImages
                .Where(ai => ai.AuctionId == auctionId)
                .ToListAsync();
        }
    }
}

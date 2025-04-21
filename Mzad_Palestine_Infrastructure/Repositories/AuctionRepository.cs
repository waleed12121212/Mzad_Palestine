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

        public async Task<IEnumerable<AuctionResponseDto>> GetByUserIdAsync(int userId)
        {
            return await _context.Auctions
                .Include(a => a.Listing)
                    .ThenInclude(l => l.Category)
                .Include(a => a.Bids)
                .Include(a => a.Winner)
                .Where(a => a.UserId == userId)
                .Select(a => new AuctionResponseDto
                {
                    AuctionId = a.AuctionId,
                    Name = a.Name,
                    CategoryName = a.Listing.Category.Name,
                    ReservePrice = a.ReservePrice,
                    CurrentBid = a.CurrentBid,
                    BidIncrement = a.BidIncrement,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                    ImageUrl = a.ImageUrl,
                    Status = a.Status,
                    BidsCount = a.Bids.Count,
                    WinnerName = a.Winner != null ? a.Winner.UserName : null
                })
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

        public async Task<IEnumerable<AuctionResponseDto>> SearchAsync(AuctionSearchDto searchDto)
        {
            var query = _context.Auctions
                .Include(a => a.Listing)
                    .ThenInclude(l => l.Category)
                .Include(a => a.Bids)
                .Include(a => a.Winner)
                .AsQueryable();

            // البحث بالكلمة المفتاحية في اسم المزاد
            if (!string.IsNullOrEmpty(searchDto.Keyword))
            {
                query = query.Where(a => a.Name.ToLower().Contains(searchDto.Keyword.ToLower()));
            }

            // البحث بالفئة
            if (!string.IsNullOrEmpty(searchDto.Category))
            {
                query = query.Where(a => a.Listing.Category.Name.ToLower().Contains(searchDto.Category.ToLower()));
            }

            // البحث بالسعر
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

            // البحث بالتاريخ
            if (searchDto.StartDate.HasValue)
            {
                var startDate = searchDto.StartDate.Value.Date;
                query = query.Where(a => a.StartTime.Date >= startDate);
            }

            if (searchDto.EndDate.HasValue)
            {
                var endDate = searchDto.EndDate.Value.Date.AddDays(1).AddSeconds(-1);
                query = query.Where(a => a.EndTime <= endDate);
            }

            // البحث بحالة المزاد
            if (searchDto.Status.HasValue)
            {
                query = query.Where(a => a.Status == searchDto.Status.Value);
            }

            // البحث بمعرف المستخدم
            if (searchDto.UserId.HasValue)
            {
                query = query.Where(a => a.UserId == searchDto.UserId.Value);
            }

            return await query
                .Select(a => new AuctionResponseDto
                {
                    AuctionId = a.AuctionId,
                    Name = a.Name,
                    CategoryName = a.Listing.Category.Name,
                    ReservePrice = a.ReservePrice,
                    CurrentBid = a.CurrentBid,
                    BidIncrement = a.BidIncrement,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                    ImageUrl = a.ImageUrl,
                    Status = a.Status,
                    BidsCount = a.Bids.Count,
                    WinnerName = a.Winner != null ? a.Winner.UserName : null
                })
                .ToListAsync();
        }

        public async Task UpdateAsync(Auction auction)
        {
            _context.Entry(auction).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public IQueryable<Auction> GetQueryable()
        {
            return _context.Auctions
                .Include(a => a.Listing)
                    .ThenInclude(l => l.Category)
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

        public async Task<Auction> GetByNameAsync(string name)
        {
            throw new NotImplementedException("Auctions cannot be searched by name. Please use other search criteria such as ID or associated listing.");
        }
    }
}

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
    public class ListingRepository : GenericRepository<Listing>, IListingRepository
    {
        public ListingRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Listing>> GetListingsAsync()
        {
            return await _context.Listings
                .Include(l => l.Images)
                .Include(l => l.Category)
                .Include(l => l.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<Listing>> GetListingsByCategoryIdAsync(int categoryId)
        {
            return await _context.Listings
                .Include(l => l.Images)
                .Include(l => l.Category)
                .Include(l => l.User)
                .Where(l => l.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Listing>> GetActiveListingsAsync()
        {
            return await _context.Listings
                .Include(l => l.Images)
                .Include(l => l.Category)
                .Include(l => l.User)
                .Where(l => l.Status == ListingStatus.Active)
                .ToListAsync();
        }

        public async Task<IEnumerable<Listing>> GetListingsByUserIdAsync(int userId)
        {
            return await _context.Listings
                .Include(l => l.Images)
                .Include(l => l.Category)
                .Include(l => l.User)
                .Where(l => l.UserId == userId)
                .ToListAsync();
        }

        public async Task<Listing> GetListingByIdAsync(int id)
        {
            return await _context.Listings
                .Include(l => l.Images)
                .Include(l => l.Category)
                .Include(l => l.User)
                .FirstOrDefaultAsync(l => l.ListingId == id);
        }

        public async Task<Listing> CreateListingAsync(Listing listing)
        {
            await _context.Listings.AddAsync(listing);
            await _context.SaveChangesAsync();
            return listing;
        }

        public async Task<Listing> UpdateListingAsync(Listing listing)
        {
            _context.Listings.Update(listing);
            await _context.SaveChangesAsync();
            return listing;
        }

        public async Task DeleteListingAsync(int id)
        {
            var listing = await _context.Listings.FindAsync(id);
            if (listing != null)
            {
                _context.Listings.Remove(listing);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ListingExistsAsync(int id)
        {
            return await _context.Listings.AnyAsync(l => l.ListingId == id);
        }

        public async Task<IEnumerable<Listing>> GetListingsByUserAsync(int userId)
        {
            return await _context.Listings
                .Include(l => l.Images)
                .Include(l => l.Category)
                .Include(l => l.User)
                .Where(l => l.UserId == userId)
                .ToListAsync();
        }

        public Task<IEnumerable<Listing>> GetByUserIdAsync(int userId)
        {
            return GetListingsByUserAsync(userId);
        }

        public async Task<IEnumerable<Listing>> GetActiveAsync()
        {
            return await _context.Listings
                .Include(l => l.Images)
                .Include(l => l.Category)
                .Include(l => l.User)
                .Where(l => l.Status == ListingStatus.Active)
                .ToListAsync();
        }

        public Task<IEnumerable<Listing>> GetByCategoryAsync(int categoryId)
        {
            return GetListingsByCategoryIdAsync(categoryId);
        }

        public async Task UpdateAsync(Listing entity)
        {
            _context.Listings.Update(entity);
            await _context.SaveChangesAsync();
        }

        public override void Update(Listing entity)
        {
            base.Update(entity);
        }

        public async Task<Listing> GetByNameAsync(string name)
        {
            return await _context.Listings
                .Include(l => l.Images)
                .Include(l => l.Category)
                .Include(l => l.User)
                .FirstOrDefaultAsync(l => l.Title.ToLower().Contains(name.ToLower()));
        }

        public override async Task<IEnumerable<Listing>> GetAllAsync()
        {
            return await _context.Listings
                .Include(l => l.Images)
                .Include(l => l.Category)
                .Include(l => l.User)
                .ToListAsync();
        }

        public override async Task<Listing> GetByIdAsync(int id)
        {
            return await _context.Listings
                .Include(l => l.Images)
                .Include(l => l.Category)
                .Include(l => l.User)
                .FirstOrDefaultAsync(l => l.ListingId == id);
        }

        public IQueryable<Listing> GetQueryable()
        {
            return _context.Listings
                .Include(l => l.Images)
                .Include(l => l.Category)
                .Include(l => l.User);
        }
    }
}

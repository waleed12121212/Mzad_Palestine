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

        public async Task<IEnumerable<Listing>> GetListingsByUserAsync(int userId)
        {
            return await _context.Listings.Where(l => l.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<Listing>> GetActiveListingsAsync( )
        {
            return await _context.Listings.Where(l => l.Status == ListingStatus.Active).ToListAsync();
        }

        public async Task<IEnumerable<Listing>> GetListingsByCategoryAsync(int categoryId)
        {
            return await _context.Listings.Where(l => l.CategoryId == categoryId).ToListAsync();
        }

        public Task<IEnumerable<Listing>> GetByUserIdAsync(int userId)
        {
            return GetListingsByUserAsync(userId);
        }

        public Task<IEnumerable<Listing>> GetActiveAsync()
        {
            return GetActiveListingsAsync();
        }

        public Task<IEnumerable<Listing>> GetByCategoryAsync(int categoryId)
        {
            return GetListingsByCategoryAsync(categoryId);
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
                .Include(l => l.Category)
                .Include(l => l.User)
                .FirstOrDefaultAsync(l => l.Title.ToLower().Contains(name.ToLower()));
        }
    }
}

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
            // نفترض وجود خاصية CategoryId في Listing
            return await _context.Listings.Where(l => l.CategoryId == categoryId).ToListAsync();
        }
    }
}

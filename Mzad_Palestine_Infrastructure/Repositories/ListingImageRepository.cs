using Microsoft.EntityFrameworkCore;
using Mzad_Palestine_Core.Interfaces;
using Mzad_Palestine_Core.Models;
using Mzad_Palestine_Infrastructure.Data;
using Mzad_Palestine_Infrastructure.Repositories.Common;

namespace Mzad_Palestine_Infrastructure.Repositories
{
    public class ListingImageRepository : GenericRepository<ListingImage>, IListingImageRepository
    {
        public ListingImageRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<ListingImage>> GetByListingIdAsync(int listingId)
        {
            return await _context.Set<ListingImage>()
                .Where(li => li.ListingId == listingId)
                .ToListAsync();
        }
    }
} 
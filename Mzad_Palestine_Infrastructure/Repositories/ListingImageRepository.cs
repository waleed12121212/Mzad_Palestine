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

        public override void Update(ListingImage entity)
        {
            base.Update(entity);
        }

        public async Task<ListingImage> GetByNameAsync(string name)
        {
            throw new NotImplementedException("Listing images cannot be searched by name. Please use image ID or listing ID to search.");
        }
    }
} 
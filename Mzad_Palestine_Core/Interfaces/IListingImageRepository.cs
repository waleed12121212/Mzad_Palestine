using Mzad_Palestine_Core.Models;

namespace Mzad_Palestine_Core.Interfaces
{
    public interface IListingImageRepository : IGenericRepository<ListingImage>
    {
        Task<IEnumerable<ListingImage>> GetByListingIdAsync(int listingId);
    }
} 
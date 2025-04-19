using Mzad_Palestine_Core.DTOs.Listing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Interfaces.Services
{
    public interface IListingService
    {
        Task<ListingDto> CreateAsync(CreateListingDto dto);
        Task<IEnumerable<ListingDto>> GetAllAsync();
        Task<ListingDto?> GetByIdAsync(int id);
        Task<IEnumerable<ListingDto>> GetByUserIdAsync(int userId);
        Task<IEnumerable<ListingDto>> GetByCategoryAsync(int categoryId);
        Task<IEnumerable<ListingDto>> GetActiveAsync();
        Task<ListingDto> UpdateAsync(int id, UpdateListingDto dto);
        Task<bool> DeleteAsync(int id);
    }
}

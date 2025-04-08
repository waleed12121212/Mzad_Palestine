using Mzad_Palestine_Core.DTO_s.Listing;
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
        Task<IEnumerable<ListingDto>> GetAllAsync( );
        Task<ListingDto?> GetByIdAsync(int id);
        Task<IEnumerable<ListingDto>> GetByUserIdAsync(int userId);
    }
}

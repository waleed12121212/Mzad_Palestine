using Mzad_Palestine_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Interfaces
{
    public interface IListingRepository : IGenericRepository<Listing>
    {
        Task<IEnumerable<Listing>> GetListingsByUserAsync(int userId);
        Task<IEnumerable<Listing>> GetActiveListingsAsync( );
        Task<IEnumerable<Listing>> GetListingsByCategoryAsync(int categoryId);
    }
}

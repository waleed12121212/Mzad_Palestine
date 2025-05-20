using Microsoft.EntityFrameworkCore;
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
        Task<IEnumerable<Listing>> GetByUserIdAsync(int userId);
        Task<IEnumerable<Listing>> GetActiveAsync();
        Task<IEnumerable<Listing>> GetByCategoryAsync(int categoryId);
        Task UpdateAsync(Listing entity);
        IQueryable<Listing> GetQueryable();
    }
}

using Mzad_Palestine_Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<IEnumerable<Category>> GetByParentIdAsync(int parentId);
        Task<IEnumerable<Category>> GetActiveCategoriesAsync();
    }
} 
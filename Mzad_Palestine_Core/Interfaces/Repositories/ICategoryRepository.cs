using Mzad_Palestine_Core.Models;
using Mzad_Palestine_Core.Interfaces.Common;

namespace Mzad_Palestine_Core.Interfaces.Repositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<IEnumerable<Category>> GetByParentIdAsync(int parentId);
        Task<IEnumerable<Category>> GetActiveCategoriesAsync();
    }
} 
using Microsoft.EntityFrameworkCore;
using Mzad_Palestine_Core.Interfaces.Repositories;
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
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Category>> GetActiveCategoriesAsync()
        {
            return await _context.Set<Category>()
                .Include(c => c.ParentCategory)
                .Where(c => c.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetByParentIdAsync(int parentId)
        {
            return await _context.Set<Category>()
                .Include(c => c.ParentCategory)
                .Where(c => c.ParentCategoryId == parentId)
                .ToListAsync();
        }
    }
}

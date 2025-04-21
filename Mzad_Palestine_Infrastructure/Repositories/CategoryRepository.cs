using Microsoft.EntityFrameworkCore;
using Mzad_Palestine_Core.Models;
using Mzad_Palestine_Core.Interfaces;
using Mzad_Palestine_Infrastructure.Data;
using Mzad_Palestine_Infrastructure.Repositories.Common;

namespace Mzad_Palestine_Infrastructure.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public override async Task<Category> GetByIdAsync(int id)
        {
            return await _dbContext.Categories
                .Include(c => c.ParentCategory)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public override async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _dbContext.Categories
                .Include(c => c.ParentCategory)
                .ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetActiveCategoriesAsync()
        {
            return await _dbContext.Categories
                .Include(c => c.ParentCategory)
                .Where(c => c.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetByParentIdAsync(int parentId)
        {
            return await _dbContext.Categories
                .Include(c => c.ParentCategory)
                .Where(c => c.ParentCategoryId == parentId)
                .ToListAsync();
        }

        public override async Task DeleteAsync(Category entity)
        {
            _dbContext.Categories.Remove(entity);
        }
    }
} 
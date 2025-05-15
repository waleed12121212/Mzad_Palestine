using Microsoft.EntityFrameworkCore;
using Mzad_Palestine_Core.Models;
using Mzad_Palestine_Core.Interfaces;
using Mzad_Palestine_Infrastructure.Data;
using Mzad_Palestine_Infrastructure.Repositories.Common;
using System.Text;

namespace Mzad_Palestine_Infrastructure.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _dbContext = context;
            // Set UTF8 encoding for database connections
            _dbContext.Database.SetCommandTimeout(60);
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public override async Task<Category> GetByIdAsync(int id)
        {
            var category = await _dbContext.Categories
                .Include(c => c.ParentCategory)
                .Include(c => c.Listings)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category != null && !string.IsNullOrEmpty(category.Description))
            {
                category.Description = System.Web.HttpUtility.HtmlDecode(category.Description);
            }

            return category;
        }

        public override async Task<IEnumerable<Category>> GetAllAsync()
        {
            var categories = await _dbContext.Categories
                .Include(c => c.ParentCategory)
                .Include(c => c.Listings)
                .ToListAsync();

            foreach (var category in categories)
            {
                if (!string.IsNullOrEmpty(category.Description))
                {
                    category.Description = System.Web.HttpUtility.HtmlDecode(category.Description);
                }
            }

            return categories;
        }

        public async Task<IEnumerable<Category>> GetActiveCategoriesAsync()
        {
            var categories = await _dbContext.Categories
                .Include(c => c.ParentCategory)
                .Include(c => c.Listings)
                .Where(c => c.IsActive)
                .ToListAsync();

            foreach (var category in categories)
            {
                if (!string.IsNullOrEmpty(category.Description))
                {
                    category.Description = System.Web.HttpUtility.HtmlDecode(category.Description);
                }
            }

            return categories;
        }

        public async Task<IEnumerable<Category>> GetByParentIdAsync(int parentId)
        {
            var categories = await _dbContext.Categories
                .Include(c => c.ParentCategory)
                .Include(c => c.Listings)
                .Where(c => c.ParentCategoryId == parentId)
                .ToListAsync();

            foreach (var category in categories)
            {
                if (!string.IsNullOrEmpty(category.Description))
                {
                    category.Description = System.Web.HttpUtility.HtmlDecode(category.Description);
                }
            }

            return categories;
        }

        public override async Task DeleteAsync(Category entity)
        {
            _dbContext.Categories.Remove(entity);
        }

        public override void Update(Category entity)
        {
            base.Update(entity);
        }

        public async Task<Category> GetByNameAsync(string name)
        {
            var category = await _dbContext.Categories
                .Include(c => c.ParentCategory)
                .FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());

            if (category != null && !string.IsNullOrEmpty(category.Description))
            {
                category.Description = System.Web.HttpUtility.HtmlDecode(category.Description);
            }

            return category;
        }
    }
} 
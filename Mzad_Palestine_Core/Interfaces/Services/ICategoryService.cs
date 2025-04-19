using Mzad_Palestine_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mzad_Palestine_Core.DTOs.Category;

namespace Mzad_Palestine_Core.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<CategoryDto> GetByIdAsync(int id);
        Task<CategoryDto> CreateAsync(CreateCategoryDto dto);
        Task<CategoryDto> UpdateAsync(int id, UpdateCategoryDto dto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Category>> GetByParentIdAsync(int parentId);
        Task<bool> ToggleActiveStatusAsync(int id);
        Task<IEnumerable<Category>> GetActiveCategoriesAsync();
    }
} 
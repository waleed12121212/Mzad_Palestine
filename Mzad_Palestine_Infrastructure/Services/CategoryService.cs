using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Mzad_Palestine_Core.DTOs.Category;
using Mzad_Palestine_Core.Interfaces.Services;
using Mzad_Palestine_Core.Models;
using Mzad_Palestine_Core.Interfaces.Common;
using Mzad_Palestine_Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<CategoryDto> GetByIdAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto)
        {
            var category = new Category
            {
                Name = dto.Name,
                Description = dto.Description,
                ParentCategoryId = dto.ParentCategoryId
            };

            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.CompleteAsync();

            return await GetByIdAsync(category.Id);
        }

        public async Task<CategoryDto> UpdateAsync(int id, UpdateCategoryDto dto)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
                throw new Exception("التصنيف غير موجود");

            category.Name = dto.Name;
            category.Description = dto.Description;
            category.ParentCategoryId = dto.ParentCategoryId;
            category.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Categories.Update(category);
            await _unitOfWork.CompleteAsync();

            return await GetByIdAsync(id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
                return false;

            await _unitOfWork.Categories.DeleteAsync(category);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<IEnumerable<Category>> GetActiveCategoriesAsync()
        {
            return await _unitOfWork.Categories.GetActiveCategoriesAsync();
        }

        public async Task<IEnumerable<Category>> GetByParentIdAsync(int parentId)
        {
            return await _unitOfWork.Categories.GetByParentIdAsync(parentId);
        }

        public Task<bool> ToggleActiveStatusAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
} 
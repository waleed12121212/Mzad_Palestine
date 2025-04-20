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

        public CategoryService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync( )
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
            try
            {
                // التحقق من عدم وجود تصنيف بنفس الاسم
                var existingCategory = await _unitOfWork.Categories
                    .FindAsync(c => c.Name.ToLower() == dto.Name.ToLower());

                if (existingCategory.Any())
                {
                    throw new Exception("يوجد تصنيف بنفس الاسم");
                }

                // التحقق من وجود التصنيف الأب إذا تم تحديده
                if (dto.ParentCategoryId.HasValue)
                {
                    var parentCategory = await _unitOfWork.Categories.GetByIdAsync(dto.ParentCategoryId.Value);
                    if (parentCategory == null)
                    {
                        throw new Exception("التصنيف الأب غير موجود");
                    }
                }

                var category = new Category
                {
                    Name = dto.Name ,
                    Description = dto.Description ,
                    ImageUrl = dto.ImageUrl ,
                    ParentCategoryId = dto.ParentCategoryId ,
                    CreatedAt = DateTime.UtcNow ,
                    IsActive = true
                };

                await _unitOfWork.Categories.AddAsync(category);
                var result = await _unitOfWork.CompleteAsync();

                if (result <= 0)
                {
                    throw new Exception("فشل في إضافة التصنيف");
                }

                return _mapper.Map<CategoryDto>(category);
            }
            catch (Exception ex)
            {
                throw new Exception($"حدث خطأ أثناء إنشاء التصنيف: {ex.Message}");
            }
        }

        public async Task<CategoryDto> UpdateAsync(int id , UpdateCategoryDto dto)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
                throw new Exception("التصنيف غير موجود");

            // التحقق من عدم وجود تصنيف آخر بنفس الاسم
            var existingCategory = await _unitOfWork.Categories
                .FindAsync(c => c.Name.ToLower() == dto.Name.ToLower() && c.Id != id);

            if (existingCategory.Any())
            {
                throw new Exception("يوجد تصنيف آخر بنفس الاسم");
            }

            // التحقق من وجود التصنيف الأب إذا تم تحديده
            if (dto.ParentCategoryId.HasValue)
            {
                var parentCategory = await _unitOfWork.Categories.GetByIdAsync(dto.ParentCategoryId.Value);
                if (parentCategory == null)
                {
                    throw new Exception("التصنيف الأب غير موجود");
                }
                // التأكد من أن التصنيف لا يشير إلى نفسه كأب
                if (dto.ParentCategoryId == id)
                {
                    throw new Exception("لا يمكن تعيين التصنيف كأب لنفسه");
                }
            }

            category.Name = dto.Name;
            category.Description = dto.Description;
            category.ParentCategoryId = dto.ParentCategoryId;
            category.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Categories.Update(category);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var category = await _unitOfWork.Categories.GetByIdAsync(id);
                if (category == null)
                    return false;

                // التحقق من عدم وجود تصنيفات فرعية
                var hasChildren = await _unitOfWork.Categories
                    .FindAsync(c => c.ParentCategoryId == id);

                if (hasChildren.Any())
                {
                    throw new Exception("لا يمكن حذف التصنيف لأنه يحتوي على تصنيفات فرعية");
                }

                // التحقق من عدم وجود منتجات مرتبطة
                var hasListings = await _unitOfWork.Listings
                    .FindAsync(l => l.CategoryId == id);
                
                if (hasListings.Any())
                {
                    throw new Exception("لا يمكن حذف التصنيف لأنه يحتوي على منتجات");
                }

                await _unitOfWork.Categories.DeleteAsync(category);
                var result = await _unitOfWork.CompleteAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"حدث خطأ أثناء حذف التصنيف: {ex.Message}");
            }
        }

        public async Task<IEnumerable<Category>> GetActiveCategoriesAsync( )
        {
            return await _unitOfWork.Categories
                .FindAsync(c => c.IsActive);
        }

        public async Task<IEnumerable<Category>> GetByParentIdAsync(int parentId)
        {
            return await _unitOfWork.Categories
                .FindAsync(c => c.ParentCategoryId == parentId);
        }

        public async Task<bool> ToggleActiveStatusAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
                return false;

            category.IsActive = !category.IsActive;
            category.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Categories.Update(category);
            var result = await _unitOfWork.CompleteAsync();
            return result > 0;
        }
    }
}
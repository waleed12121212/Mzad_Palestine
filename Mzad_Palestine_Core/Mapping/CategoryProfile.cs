using AutoMapper;
using Mzad_Palestine_Core.DTOs.Category;
using Mzad_Palestine_Core.Models;

namespace Mzad_Palestine_Core.Mapping
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.ParentCategoryName, 
                    opt => opt.MapFrom(src => src.ParentCategory != null ? src.ParentCategory.Name : null));
        }
    }
} 
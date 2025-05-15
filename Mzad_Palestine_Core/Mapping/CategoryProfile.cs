using AutoMapper;
using Mzad_Palestine_Core.DTOs.Category;
using Mzad_Palestine_Core.Models;
using System.Text;

namespace Mzad_Palestine_Core.Mapping
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.ParentCategoryName, 
                    opt => opt.MapFrom(src => src.ParentCategory != null ? src.ParentCategory.Name : null))
                .ForMember(dest => dest.ListingsCount,
                    opt => opt.MapFrom(src => src.Listings != null ? src.Listings.Count : 0))
                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.Description) 
                        ? System.Web.HttpUtility.HtmlDecode(src.Description) 
                        : ""));
        }
    }
} 
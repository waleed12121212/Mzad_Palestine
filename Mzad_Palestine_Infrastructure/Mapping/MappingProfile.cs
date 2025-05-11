using AutoMapper;
using Mzad_Palestine_Core.DTO_s.Watchlist;
using Mzad_Palestine_Core.Models;
using System.Linq;

namespace Mzad_Palestine_Infrastructure.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ... existing mappings ...

            CreateMap<Watchlist, WatchlistDto>()
                .ForMember(dest => dest.WatchlistId, opt => opt.MapFrom(src => src.WatchlistId))
                .ForMember(dest => dest.ListingTitle, opt => opt.MapFrom(src => src.Listing.Title))
                .ForMember(dest => dest.ListingPrice, opt => opt.MapFrom(src => src.Listing.Price))
                .ForMember(dest => dest.ListingImage, opt => opt.MapFrom(src => 
                    src.Listing.Images != null && src.Listing.Images.Any() 
                        ? (src.Listing.Images.FirstOrDefault(i => i.IsMainImage) != null 
                            ? src.Listing.Images.FirstOrDefault(i => i.IsMainImage).ImageUrl 
                            : src.Listing.Images.First().ImageUrl)
                        : null));
        }
    }
} 
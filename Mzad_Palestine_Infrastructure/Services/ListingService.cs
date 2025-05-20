using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Mzad_Palestine_Core.DTOs.Listing;
using Mzad_Palestine_Core.Interfaces.Services;
using Mzad_Palestine_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mzad_Palestine_Core.Enums;
using Mzad_Palestine_Core.Interfaces.Common;

namespace Mzad_Palestine_Infrastructure.Services
{
    public class ListingService : IListingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ListingService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ListingDto> CreateAsync(CreateListingDto dto)
        {
            var listing = new Listing
            {
                Title = dto.Title ,
                Description = dto.Description ,
                Address = dto.Address ,
                Price = dto.Price ,
                StartingPrice = dto.Price ,
                CategoryId = dto.CategoryId ,
                UserId = dto.UserId ,
                EndDate = dto.EndDate ,
                Status = ListingStatus.Active ,
                IsActive = true ,
                IsSold = false ,
                CreatedAt = DateTime.UtcNow ,
                UpdatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Listings.AddAsync(listing);
            await _unitOfWork.CompleteAsync();

            if (dto.Images != null && dto.Images.Any())
            {
                var images = dto.Images.Select(imageUrl => new ListingImage
                {
                    ListingId = listing.ListingId ,
                    ImageUrl = imageUrl ,
                    CreatedAt = DateTime.UtcNow ,
                    IsMainImage = false
                }).ToList();

                if (images.Any())
                {
                    images.First().IsMainImage = true;
                }

                foreach (var image in images)
                {
                    await _unitOfWork.ListingImages.AddAsync(image);
                }
                await _unitOfWork.CompleteAsync();
            }

            var createdListing = await _unitOfWork.Listings.GetByIdAsync(listing.ListingId);
            var listingDto = _mapper.Map<ListingDto>(createdListing);
            listingDto.Images = createdListing.Images.Select(i => i.ImageUrl).ToList();
            return listingDto;
        }

        public async Task<ListingDto> GetByIdAsync(int id)
        {
            var listing = await _unitOfWork.Listings.GetByIdAsync(id);
            if (listing == null) return null;

            var listingDto = _mapper.Map<ListingDto>(listing);
            listingDto.Images = listing.Images.Select(i => i.ImageUrl).ToList();
            return listingDto;
        }

        public async Task<IEnumerable<ListingDto>> GetAllAsync( )
        {
            var listings = await _unitOfWork.Listings.GetAllAsync();
            var listingDtos = _mapper.Map<IEnumerable<ListingDto>>(listings);

            foreach (var dto in listingDtos)
            {
                var listing = listings.FirstOrDefault(l => l.ListingId == dto.ListingId);
                if (listing != null)
                {
                    dto.Images = listing.Images.Select(i => i.ImageUrl).ToList();
                }
            }

            return listingDtos;
        }

        public async Task<IEnumerable<ListingDto>> GetByUserIdAsync(int userId)
        {
            var listings = await _unitOfWork.Listings.GetByUserIdAsync(userId);
            var listingDtos = _mapper.Map<IEnumerable<ListingDto>>(listings);

            foreach (var dto in listingDtos)
            {
                var listing = listings.FirstOrDefault(l => l.ListingId == dto.ListingId);
                if (listing != null)
                {
                    dto.Images = listing.Images.Select(i => i.ImageUrl).ToList();
                }
            }

            return listingDtos;
        }

        public async Task<IEnumerable<ListingDto>> GetByCategoryAsync(int categoryId)
        {
            var listings = await _unitOfWork.Listings.GetByCategoryAsync(categoryId);
            var listingDtos = _mapper.Map<IEnumerable<ListingDto>>(listings);

            foreach (var dto in listingDtos)
            {
                var listing = listings.FirstOrDefault(l => l.ListingId == dto.ListingId);
                if (listing != null)
                {
                    dto.Images = listing.Images.Select(i => i.ImageUrl).ToList();
                }
            }

            return listingDtos;
        }

        public async Task<IEnumerable<ListingDto>> GetActiveAsync( )
        {
            var listings = await _unitOfWork.Listings.GetActiveAsync();
            var listingDtos = _mapper.Map<IEnumerable<ListingDto>>(listings);

            foreach (var dto in listingDtos)
            {
                var listing = listings.FirstOrDefault(l => l.ListingId == dto.ListingId);
                if (listing != null)
                {
                    dto.Images = listing.Images.Select(i => i.ImageUrl).ToList();
                }
            }

            return listingDtos;
        }

        public async Task<ListingDto> UpdateAsync(int id , UpdateListingDto dto)
        {
            var listing = await _unitOfWork.Listings.GetByIdAsync(id);
            if (listing == null)
                throw new Exception("المنتج غير موجود");

            listing.Title = dto.Title;
            listing.Description = dto.Description;
            listing.StartingPrice = dto.StartingPrice;
            listing.CategoryId = dto.CategoryId;
            listing.EndDate = dto.EndDate;
            listing.UpdatedAt = DateTime.UtcNow;

            // Delete images if specified
            if (dto.ImagesToDelete != null && dto.ImagesToDelete.Any())
            {
                var imagesToDelete = await _unitOfWork.ListingImages.FindAsync(
                    img => img.ListingId == id && dto.ImagesToDelete.Contains(img.ImageUrl));

                foreach (var image in imagesToDelete)
                {
                    await _unitOfWork.ListingImages.DeleteAsync(image);
                }
            }

            // Add new images if provided
            if (dto.NewImages != null && dto.NewImages.Any())
            {
                var images = dto.NewImages.Select(imageUrl => new ListingImage
                {
                    ListingId = listing.ListingId ,
                    ImageUrl = imageUrl ,
                    CreatedAt = DateTime.UtcNow ,
                    IsMainImage = false
                }).ToList();

                // If there are no existing images, set the first new image as main
                var existingImages = await _unitOfWork.ListingImages.GetByListingIdAsync(id);
                if (!existingImages.Any() && images.Any())
                {
                    images.First().IsMainImage = true;
                }

                foreach (var image in images)
                {
                    await _unitOfWork.ListingImages.AddAsync(image);
                }
            }

            await _unitOfWork.Listings.UpdateAsync(listing);
            await _unitOfWork.CompleteAsync();

            var updatedListing = await _unitOfWork.Listings.GetByIdAsync(id);
            var listingDto = _mapper.Map<ListingDto>(updatedListing);
            listingDto.Images = updatedListing.Images.Select(i => i.ImageUrl).ToList();
            return listingDto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var listing = await _unitOfWork.Listings.GetByIdAsync(id);
            if (listing == null)
                return false;

            await _unitOfWork.Listings.DeleteAsync(listing);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<IEnumerable<ListingDto>> SearchListingsAsync(ListingSearchDto searchDto)
        {
            var query = _unitOfWork.Listings.GetQueryable();

            if (!string.IsNullOrEmpty(searchDto.Keyword))
            {
                query = query.Where(l => l.Title.Contains(searchDto.Keyword) || l.Description.Contains(searchDto.Keyword));
            }

            if (!string.IsNullOrEmpty(searchDto.Category))
            {
                query = query.Where(l => l.Category.Name == searchDto.Category);
            }

            if (searchDto.MinPrice.HasValue)
            {
                query = query.Where(l => l.Price >= searchDto.MinPrice.Value);
            }

            if (searchDto.MaxPrice.HasValue)
            {
                query = query.Where(l => l.Price <= searchDto.MaxPrice.Value);
            }

            if (searchDto.StartDate.HasValue)
            {
                query = query.Where(l => l.CreatedAt >= searchDto.StartDate.Value);
            }

            if (searchDto.EndDate.HasValue)
            {
                query = query.Where(l => l.CreatedAt <= searchDto.EndDate.Value);
            }

            if (searchDto.Status.HasValue)
            {
                query = query.Where(l => (int)l.Status == searchDto.Status.Value);
            }

            if (searchDto.UserId.HasValue)
            {
                query = query.Where(l => l.UserId == searchDto.UserId.Value);
            }

            var listings = await query.ToListAsync();
            var listingDtos = _mapper.Map<IEnumerable<ListingDto>>(listings);

            foreach (var dto in listingDtos)
            {
                var listing = listings.FirstOrDefault(l => l.ListingId == dto.ListingId);
                if (listing != null)
                {
                    dto.Images = listing.Images.Select(i => i.ImageUrl).ToList();
                }
            }

            return listingDtos;
        }
    }
}

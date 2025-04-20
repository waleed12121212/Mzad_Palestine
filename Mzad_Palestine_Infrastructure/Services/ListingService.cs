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
                StartingPrice = dto.StartingPrice ,
                Price = dto.StartingPrice ,
                CategoryId = dto.CategoryId ,
                EndDate = dto.EndDate ,
                Status = ListingStatus.Active ,
                IsActive = true ,
                IsSold = false ,
                CreatedAt = DateTime.UtcNow ,
                UserId = int.Parse(dto.UserId)
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

            return await GetByIdAsync(listing.ListingId);
        }

        public async Task<ListingDto> GetByIdAsync(int id)
        {
            var listing = await _unitOfWork.Listings.GetByIdAsync(id);
            return _mapper.Map<ListingDto>(listing);
        }

        public async Task<IEnumerable<ListingDto>> GetAllAsync( )
        {
            var listings = await _unitOfWork.Listings.GetAllAsync();
            return _mapper.Map<IEnumerable<ListingDto>>(listings);
        }

        public async Task<IEnumerable<ListingDto>> GetByUserIdAsync(int userId)
        {
            var listings = await _unitOfWork.Listings.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<ListingDto>>(listings);
        }

        public async Task<IEnumerable<ListingDto>> GetByCategoryAsync(int categoryId)
        {
            var listings = await _unitOfWork.Listings.GetByCategoryAsync(categoryId);
            return _mapper.Map<IEnumerable<ListingDto>>(listings);
        }

        public async Task<IEnumerable<ListingDto>> GetActiveAsync( )
        {
            var listings = await _unitOfWork.Listings.GetActiveAsync();
            return _mapper.Map<IEnumerable<ListingDto>>(listings);
        }

        public async Task<ListingDto> UpdateAsync(int id, UpdateListingDto dto)
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
                    ListingId = listing.ListingId,
                    ImageUrl = imageUrl,
                    CreatedAt = DateTime.UtcNow,
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

            _unitOfWork.Listings.Update(listing);
            await _unitOfWork.CompleteAsync();

            return await GetByIdAsync(id);
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
    }
}

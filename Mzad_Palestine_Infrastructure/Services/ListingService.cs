using Mzad_Palestine_Core.DTO_s.Listing;
using Mzad_Palestine_Core.Interfaces.Services;
using Mzad_Palestine_Core.Interfaces;
using Mzad_Palestine_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mzad_Palestine_Core.Enums;

namespace Mzad_Palestine_Infrastructure.Services
{
    public class ListingService : IListingService
    {
        private readonly IListingRepository _repository;
        public ListingService(IListingRepository repository) => _repository = repository;

        public async Task<ListingDto> CreateAsync(CreateListingDto dto)
        {
            var entity = new Listing
            {
                Title = dto.Title ,
                Description = dto.Description ,
                Price = dto.Price ,
                CategoryId = dto.CategoryId ,
                LocationId = dto.LocationId ,
                Type = dto.Type ,
                Status = ListingStatus.Active
            };

            await _repository.AddAsync(entity);

            return new ListingDto
            {
                Id = entity.ListingId ,
                UserId = entity.UserId ,
                Title = entity.Title ,
                Description = entity.Description ,
                Price = entity.Price ,
                CategoryId = entity.CategoryId ,
                LocationId = entity.LocationId ,
                Type = entity.Type ,
                Status = entity.Status
            };
        }

        public async Task<IEnumerable<ListingDto>> GetAllAsync( )
        {
            var listings = await _repository.GetAllAsync();
            return listings.Select(l => new ListingDto
            {
                Id = l.ListingId ,
                UserId = l.UserId ,
                Title = l.Title ,
                Description = l.Description ,
                Price = l.Price ,
                CategoryId = l.CategoryId ,
                LocationId = l.LocationId ,
                Type = l.Type ,
                Status = l.Status
            });
        }

        public async Task<ListingDto?> GetByIdAsync(int id)
        {
            var listing = await _repository.GetByIdAsync(id);
            if (listing == null) return null;

            return new ListingDto
            {
                Id = listing.ListingId ,
                UserId = listing.UserId ,
                Title = listing.Title ,
                Description = listing.Description ,
                Price = listing.Price ,
                CategoryId = listing.CategoryId ,
                LocationId = listing.LocationId ,
                Type = listing.Type ,
                Status = listing.Status
            };
        }

        public async Task<IEnumerable<ListingDto>> GetByUserIdAsync(int userId)
        {
            var listings = await _repository.GetByUserIdAsync(userId);
            return listings.Select(l => new ListingDto
            {
                Id = l.ListingId ,
                UserId = l.UserId ,
                Title = l.Title ,
                Description = l.Description ,
                Price = l.Price ,
                CategoryId = l.CategoryId ,
                LocationId = l.LocationId ,
                Type = l.Type ,
                Status = l.Status
            });
        }

        public async Task<IEnumerable<ListingDto>> GetByCategoryAsync(int categoryId)
        {
            var listings = await _repository.GetByCategoryAsync(categoryId);
            return listings.Select(l => new ListingDto
            {
                Id = l.ListingId ,
                UserId = l.UserId ,
                Title = l.Title ,
                Description = l.Description ,
                Price = l.Price ,
                CategoryId = l.CategoryId ,
                LocationId = l.LocationId ,
                Type = l.Type ,
                Status = l.Status
            });
        }

        public async Task<IEnumerable<ListingDto>> GetActiveAsync( )
        {
            var listings = await _repository.GetActiveAsync();
            return listings.Select(l => new ListingDto
            {
                Id = l.ListingId ,
                UserId = l.UserId ,
                Title = l.Title ,
                Description = l.Description ,
                Price = l.Price ,
                CategoryId = l.CategoryId ,
                LocationId = l.LocationId ,
                Type = l.Type ,
                Status = l.Status
            });
        }

        public async Task<ListingDto> UpdateAsync(int id , UpdateListingDto dto)
        {
            var listing = await _repository.GetByIdAsync(id);
            if (listing == null)
                throw new KeyNotFoundException($"Listing with ID {id} not found");

            listing.Title = dto.Title;
            listing.Description = dto.Description;
            listing.Price = dto.Price;
            listing.CategoryId = dto.CategoryId;
            listing.LocationId = dto.LocationId;
            listing.Status = dto.IsActive ? ListingStatus.Active : ListingStatus.Inactive;
            listing.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(listing);

            return new ListingDto
            {
                Id = listing.ListingId ,
                UserId = listing.UserId ,
                Title = listing.Title ,
                Description = listing.Description ,
                Price = listing.Price ,
                CategoryId = listing.CategoryId ,
                LocationId = listing.LocationId ,
                Type = listing.Type ,
                Status = listing.Status
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var listing = await _repository.GetByIdAsync(id);
            if (listing == null)
                return false;

            await _repository.DeleteAsync(listing);
            return true;
        }
    }
}

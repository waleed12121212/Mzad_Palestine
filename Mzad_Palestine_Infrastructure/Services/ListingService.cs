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
                Type = Enum.Parse<ListingType>(dto.Type) ,
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
                Type = entity.Type.ToString() ,
                Status = entity.Status.ToString()
            };
        }

        public Task<IEnumerable<ListingDto>> GetAllAsync( )
        {
            throw new NotImplementedException();
        }

        public Task<ListingDto?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ListingDto>> GetByUserIdAsync(int userId)
        {
            throw new NotImplementedException();
        }
    }
}

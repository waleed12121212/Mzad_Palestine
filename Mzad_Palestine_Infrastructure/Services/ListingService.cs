using Mzad_Palestine_Core.DTO_s.Listing;
using Mzad_Palestine_Core.Interfaces.Services;
using Mzad_Palestine_Core.Interfaces;
using Mzad_Palestine_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Infrastructure.Services
{
    public class ListingService : IListingService
    {
        private readonly IListingRepository _repository;
        public ListingService(IListingRepository repository) => _repository = repository;

        public async Task<ListingDto> CreateAsync(CreateListingDto dto)
        {
            var entity = new Listing { Title = dto.Title , Description = dto.Description , Price = dto.Price , CategoryId = dto.CategoryId , LocationId = dto.LocationId , Type = dto.Type , Status = "Active" };
            var created = await _repository.AddAsync(entity);
            return new ListingDto(created.Id , created.UserId , created.Title , created.Description , created.Price , created.CategoryId , created.LocationId , created.Type , created.Status);
        }
    }
}

using Mzad_Palestine_Core.DTO_s.Dispute;
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
    public class DisputeService : IDisputeService
    {
        private readonly IDisputeRepository _repository;
        public DisputeService(IDisputeRepository repository) => _repository = repository;

        public async Task<DisputeDto> CreateAsync(CreateDisputeDto dto)
        {
            var entity = new Dispute { RelatedAuctionId = dto.RelatedAuctionId , Message = dto.Message , Status = "Open" };
            var created = await _repository.AddAsync(entity);
            return new DisputeDto(created.Id , created.RelatedAuctionId , created.Message , created.Status);
        }
    }
}

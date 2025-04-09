using Mzad_Palestine_Core.DTO_s.Dispute;
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
    public class DisputeService : IDisputeService
    {
        private readonly IDisputeRepository _repository;
        public DisputeService(IDisputeRepository repository) => _repository = repository;

        public async Task<DisputeDto> CreateAsync(CreateDisputeDto dto)
        {
            var entity = new Dispute
            {
                UserId = dto.UserId,
                AuctionId = dto.AuctionId,
                Reason = dto.Reason,
                Status = DisputeStatus.Open,
                CreatedAt = DateTime.UtcNow
            };
            await _repository.AddAsync(entity);
            return new DisputeDto
            {
                Id = entity.DisputeId,
                UserId = entity.UserId,
                AuctionId = entity.AuctionId,
                Reason = entity.Reason,
                Status = entity.Status,
                CreatedAt = entity.CreatedAt,
                ResolvedBy = entity.ResolvedBy
            };
        }

        public Task<IEnumerable<DisputeDto>> GetAllAsync( )
        {
            throw new NotImplementedException();
        }
    }
}

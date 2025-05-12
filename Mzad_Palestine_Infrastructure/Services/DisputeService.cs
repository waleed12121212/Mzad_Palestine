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
                UserId = dto.UserId ,
                AuctionId = dto.AuctionId ,
                Reason = dto.Reason ,
                Status = DisputeStatus.Open ,
                CreatedAt = DateTime.UtcNow
            };
            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();
            return new DisputeDto
            {
                Id = entity.DisputeId ,
                UserId = entity.UserId ,
                AuctionId = entity.AuctionId ,
                Reason = entity.Reason ,
                Status = entity.Status ,
                CreatedAt = entity.CreatedAt ,
                ResolvedBy = entity.ResolvedBy
            };
        }

        public async Task<IEnumerable<DisputeDto>> GetAllAsync( )
        {
            var disputes = await _repository.GetAllAsync();
            return disputes.Select(d => new DisputeDto
            {
                Id = d.DisputeId ,
                UserId = d.UserId ,
                AuctionId = d.AuctionId ,
                Reason = d.Reason ,
                Status = d.Status ,
                CreatedAt = d.CreatedAt ,
                ResolvedBy = d.ResolvedBy
            });
        }

        public async Task<DisputeDto> ResolveDisputeAsync(int id , string resolution , int resolvedById)
        {
            var dispute = await _repository.GetByIdAsync(id);
            if (dispute == null)
                throw new Exception("النزاع غير موجود");

            if (dispute.Status == DisputeStatus.Resolved)
                throw new Exception("النزاع تم حله مسبقاً");

            dispute.Status = DisputeStatus.Resolved;
            dispute.ResolvedBy = resolvedById;

            _repository.Update(dispute);
            await _repository.SaveChangesAsync();

            return new DisputeDto
            {
                Id = dispute.DisputeId ,
                UserId = dispute.UserId ,
                AuctionId = dispute.AuctionId ,
                Reason = dispute.Reason ,
                Status = dispute.Status ,
                CreatedAt = dispute.CreatedAt ,
                ResolvedBy = dispute.ResolvedBy
            };
        }
    }
}

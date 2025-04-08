using Mzad_Palestine_Core.DTO_s.Customer_Support;
using Mzad_Palestine_Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Infrastructure.Services
{
    public class SupportService : ISupportService
    {
        private readonly ISupportRepository _repository;
        public SupportService(ISupportRepository repository) => _repository = repository;

        public async Task<SupportTicketDto> CreateAsync(CreateSupportTicketDto dto)
        {
            var entity = new SupportTicket { UserId = dto.UserId , Subject = dto.Subject , Description = dto.Description , Status = "Open" };
            var created = await _repository.AddAsync(entity);
            return new SupportTicketDto(created.Id , created.UserId , created.Subject , created.Description , created.Status);
        }
    }
}

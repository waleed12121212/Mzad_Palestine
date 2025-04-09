using Mzad_Palestine_Core.DTO_s.Customer_Support;
using Mzad_Palestine_Core.Interfaces;
using Mzad_Palestine_Core.Models;
using Mzad_Palestine_Core.Enums;
using Mzad_Palestine_Core.Interfaces.Services;

namespace Mzad_Palestine_Infrastructure.Services
{
    public class SupportService : ISupportService
    {
        private readonly ISupportRepository _supportRepository;

        public SupportService(ISupportRepository supportRepository)
        {
            _supportRepository = supportRepository;
        }

        public async Task<SupportTicketDto> CreateAsync(CreateSupportTicketDto dto)
        {
            var ticket = new CustomerSupportTicket
            {
                UserId = dto.UserId , // Now both are int
                Subject = dto.Subject ,
                Description = dto.Description ,
                CreatedAt = DateTime.UtcNow ,
                Status = TicketStatus.Open // Using enum from Core.Enums
            };

            await _supportRepository.AddAsync(ticket);

            return new SupportTicketDto
            {
                Id = ticket.TicketId , // Using TicketId instead of Id
                UserId = ticket.UserId ,
                Subject = ticket.Subject ,
                Description = ticket.Description ,
                Status = ticket.Status
            };
        }

        public async Task<IEnumerable<SupportTicketDto>> GetUserTicketsAsync(int userId)
        {
            var tickets = await _supportRepository.GetUserTicketsAsync(userId);
            return tickets.Select(t => new SupportTicketDto
            {
                Id = t.TicketId , // Using TicketId instead of Id
                UserId = t.UserId ,
                Subject = t.Subject ,
                Description = t.Description ,
                Status = t.Status
            });
        }
    }
}

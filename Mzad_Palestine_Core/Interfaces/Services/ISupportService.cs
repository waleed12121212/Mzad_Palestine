using Mzad_Palestine_Core.DTO_s.Customer_Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Interfaces.Services
{
    public interface ISupportService
    {
        Task<SupportTicketDto> CreateAsync(CreateSupportTicketDto dto);
        Task<IEnumerable<SupportTicketDto>> GetUserTicketsAsync(int userId);
    }
}

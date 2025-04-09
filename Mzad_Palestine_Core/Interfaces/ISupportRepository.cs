using Mzad_Palestine_Core.Interfaces.Common;
using Mzad_Palestine_Core.Models;

namespace Mzad_Palestine_Core.Interfaces
{
    public interface ISupportRepository : IGenericRepository<CustomerSupportTicket>
    {
        Task<IEnumerable<CustomerSupportTicket>> GetUserTicketsAsync(int userId);
    }
}
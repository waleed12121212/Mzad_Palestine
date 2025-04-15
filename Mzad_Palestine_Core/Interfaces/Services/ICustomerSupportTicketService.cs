using Mzad_Palestine_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Interfaces.Services
{
    public interface ICustomerSupportTicketService
    {
        Task<CustomerSupportTicket> GetByIdAsync(int id);
        Task<IEnumerable<CustomerSupportTicket>> GetAllAsync();
        Task<CustomerSupportTicket> AddAsync(CustomerSupportTicket ticket);
        Task<CustomerSupportTicket> UpdateAsync(CustomerSupportTicket ticket);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<CustomerSupportTicket>> GetByUserIdAsync(int userId);
        Task<IEnumerable<CustomerSupportTicket>> GetByStatusAsync(string status);
        Task<bool> ChangeStatusAsync(int id, string status);
        Task<bool> AddResponseAsync(int ticketId, string response, int adminId);
    }
} 
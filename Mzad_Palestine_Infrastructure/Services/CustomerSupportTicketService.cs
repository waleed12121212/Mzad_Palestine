using Mzad_Palestine_Core.Interfaces;
using Mzad_Palestine_Core.Interfaces.Services;
using Mzad_Palestine_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Infrastructure.Services
{
    public class CustomerSupportTicketService : ICustomerSupportTicketService
    {
        private readonly ICustomerSupportTicketRepository _ticketRepository;

        public CustomerSupportTicketService(ICustomerSupportTicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public Task<CustomerSupportTicket> AddAsync(CustomerSupportTicket ticket)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddResponseAsync(int ticketId, string response, int adminId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ChangeStatusAsync(int id, string status)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CustomerSupportTicket>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<CustomerSupportTicket> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CustomerSupportTicket>> GetByStatusAsync(string status)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CustomerSupportTicket>> GetByUserIdAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<CustomerSupportTicket> UpdateAsync(CustomerSupportTicket ticket)
        {
            throw new NotImplementedException();
        }
    }
} 
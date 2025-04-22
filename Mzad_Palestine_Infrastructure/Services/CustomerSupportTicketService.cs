using Mzad_Palestine_Core.Interfaces;
using Mzad_Palestine_Core.Interfaces.Services;
using Mzad_Palestine_Core.Models;
using Mzad_Palestine_Core.Enums;
using System;
using System.Collections.Generic;
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

        public async Task<CustomerSupportTicket> AddAsync(CustomerSupportTicket ticket)
        {
            await _ticketRepository.AddAsync(ticket);
            return ticket;
        }

        public async Task<bool> AddResponseAsync(int ticketId, string response, int adminId)
        {
            var ticket = await _ticketRepository.GetByIdAsync(ticketId);
            if (ticket == null)
                return false;

            ticket.Description += $"\n\nAdmin Response ({DateTime.UtcNow}):\n{response}";
            _ticketRepository.Update(ticket);
            await _ticketRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ChangeStatusAsync(int id, string status)
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);
            if (ticket == null)
                return false;

            if (Enum.TryParse<TicketStatus>(status, true, out TicketStatus newStatus))
            {
                ticket.Status = newStatus;
                _ticketRepository.Update(ticket);
                await _ticketRepository.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);
            if (ticket == null)
                return false;

            await _ticketRepository.DeleteAsync(ticket);
            await _ticketRepository.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<CustomerSupportTicket>> GetAllAsync()
        {
            return await _ticketRepository.GetAllAsync();
        }

        public async Task<CustomerSupportTicket> GetByIdAsync(int id)
        {
            return await _ticketRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<CustomerSupportTicket>> GetByStatusAsync(string status)
        {
            if (Enum.TryParse<TicketStatus>(status, true, out TicketStatus ticketStatus))
            {
                return await _ticketRepository.FindAsync(t => t.Status == ticketStatus);
            }
            return new List<CustomerSupportTicket>();
        }

        public async Task<IEnumerable<CustomerSupportTicket>> GetByUserIdAsync(int userId)
        {
            return await _ticketRepository.FindAsync(t => t.UserId == userId);
        }

        public async Task<CustomerSupportTicket> UpdateAsync(CustomerSupportTicket ticket)
        {
            _ticketRepository.Update(ticket);
            await _ticketRepository.SaveChangesAsync();
            return ticket;
        }
    }
} 
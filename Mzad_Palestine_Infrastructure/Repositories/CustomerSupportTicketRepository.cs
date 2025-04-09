using Microsoft.EntityFrameworkCore;
using Mzad_Palestine_Core.Enums;
using Mzad_Palestine_Core.Interfaces;
using Mzad_Palestine_Core.Models;
using Mzad_Palestine_Infrastructure.Data;
using Mzad_Palestine_Infrastructure.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Infrastructure.Repositories
{
    public class CustomerSupportTicketRepository : GenericRepository<CustomerSupportTicket>, ICustomerSupportTicketRepository
    {
        public CustomerSupportTicketRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<CustomerSupportTicket>> GetOpenTicketsAsync( )
        {
            return await _context.CustomerSupportTickets.Where(t => t.Status == TicketStatus.Open).ToListAsync();
        }
    }
}

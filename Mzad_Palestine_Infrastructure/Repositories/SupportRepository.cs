using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Mzad_Palestine_Core.Interfaces;
using Mzad_Palestine_Core.Models;
using Mzad_Palestine_Infrastructure.Data;
using Mzad_Palestine_Infrastructure.Repositories.Common;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mzad_Palestine_Infrastructure.Repositories
{
    public class SupportRepository : GenericRepository<CustomerSupportTicket>, ISupportRepository
    {
        public SupportRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<CustomerSupportTicket>> GetUserTicketsAsync(int userId)
        {
            return await _context.CustomerSupportTickets.Where(t => t.UserId == userId).ToListAsync();
        }

        public async Task AddAsync(CustomerSupportTicket entity)
        {
            await _context.CustomerSupportTickets.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CustomerSupportTicket>> FindAsync(Expression<Func<CustomerSupportTicket, bool>> criteria)
        {
            return await base.FindAsync(criteria);
        }

        public async Task DeleteAsync(CustomerSupportTicket entity)
        {
            await base.DeleteAsync(entity);
        }

        public void Update(CustomerSupportTicket entity)
        {
            base.Update(entity);
        }

        public async Task<CustomerSupportTicket> GetByNameAsync(string name)
        {
            throw new NotImplementedException("Customer support tickets cannot be searched by name.");
        }
    }
}
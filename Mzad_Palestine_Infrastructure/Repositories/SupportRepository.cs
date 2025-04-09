using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Mzad_Palestine_Core.Interfaces;
using Mzad_Palestine_Core.Models;
using Mzad_Palestine_Infrastructure.Data;
using Mzad_Palestine_Infrastructure.Repositories.Common;

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
            await base.AddAsync(entity);
        }

        public async Task<IEnumerable<CustomerSupportTicket>> FindAsync(Expression<Func<CustomerSupportTicket , bool>> criteria)
        {
            return await base.FindAsync(criteria);
        }

        public void Remove(CustomerSupportTicket entity)
        {
            base.Remove(entity);
        }

        public void Update(CustomerSupportTicket entity)
        {
            base.Update(entity);
        }
    }
}
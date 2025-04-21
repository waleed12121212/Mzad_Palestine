using Microsoft.EntityFrameworkCore;
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
    public class SubscriptionRepository : GenericRepository<Subscription>, ISubscriptionRepository
    {
        public SubscriptionRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Subscription> GetActiveSubscriptionAsync(int userId)
        {
            // نفترض أن Subscription يحتوي على خاصية Status
            return await _context.Subscriptions.FirstOrDefaultAsync(s => s.UserId == userId && s.Status.ToLower() == "active");
        }

        public override void Update(Subscription entity)
        {
            base.Update(entity);
        }

        public async Task<Subscription> GetByNameAsync(string name)
        {
            throw new NotImplementedException("Subscriptions cannot be searched by name. Please use subscription ID or user ID to search.");
        }
    }
}

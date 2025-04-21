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
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Notification>> GetUnreadNotificationsAsync(int userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId && n.Status == NotificationStatus.Unread)
                .ToListAsync();
        }

        public override void Update(Notification entity)
        {
            base.Update(entity);
        }

        public async Task<Notification> GetByNameAsync(string name)
        {
            throw new NotImplementedException("Notifications cannot be searched by name. Please use notification ID or user ID to search.");
        }
    }
}

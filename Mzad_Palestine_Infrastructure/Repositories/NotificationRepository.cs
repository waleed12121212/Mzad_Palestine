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
using System.Linq.Expressions;

namespace Mzad_Palestine_Infrastructure.Repositories
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public NotificationRepository(ApplicationDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public override async Task<IEnumerable<Notification>> FindAsync(Expression<Func<Notification, bool>> predicate)
        {
            return await _dbContext.Notifications
                .Include(n => n.User)
                .Where(predicate)
                .AsNoTracking()
                .ToListAsync();
        }

        public override async Task<Notification> GetByIdAsync(int id)
        {
            return await _dbContext.Notifications
                .Include(n => n.User)
                .FirstOrDefaultAsync(n => n.NotificationId == id);
        }

        public async Task<IEnumerable<Notification>> GetUnreadNotificationsAsync(int userId)
        {
            return await _dbContext.Notifications
                .Include(n => n.User)
                .Where(n => n.UserId == userId && n.Status == NotificationStatus.Unread)
                .AsNoTracking()
                .ToListAsync();
        }

        public override void Update(Notification entity)
        {
            var existingEntity = _dbContext.Notifications.Local.FirstOrDefault(n => n.NotificationId == entity.NotificationId);
            if (existingEntity != null)
            {
                _dbContext.Entry(existingEntity).State = EntityState.Detached;
            }
            
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public override async Task DeleteAsync(Notification entity)
        {
            var existingEntity = await _dbContext.Notifications.FindAsync(entity.NotificationId);
            if (existingEntity != null)
            {
                _dbContext.Notifications.Remove(existingEntity);
            }
        }

        public async Task<Notification> GetByNameAsync(string name)
        {
            throw new NotImplementedException("Notifications cannot be searched by name. Please use notification ID or user ID to search.");
        }
    }
}

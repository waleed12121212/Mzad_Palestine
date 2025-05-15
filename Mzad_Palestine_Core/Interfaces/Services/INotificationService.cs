using Mzad_Palestine_Core.DTO_s.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Interfaces.Services
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationDto>> GetUserNotificationsAsync(int userId);
        Task<bool> MarkAsReadAsync(int notificationId, int userId);
        Task<bool> MarkAllAsReadAsync(int userId);
        Task<bool> DeleteAsync(int notificationId, int userId);
        Task<bool> ClearAllAsync(int userId);
        Task<NotificationDto> CreateNotificationAsync(CreateNotificationDto dto);
    }
}

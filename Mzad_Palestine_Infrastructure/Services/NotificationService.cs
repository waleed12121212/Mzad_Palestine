using Mzad_Palestine_Core.DTO_s.Notification;
using Mzad_Palestine_Core.Interfaces.Services;
using Mzad_Palestine_Core.Interfaces;
using Mzad_Palestine_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mzad_Palestine_Core.Enums;
using Mzad_Palestine_Core.Interfaces.Common;

namespace Mzad_Palestine_Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public NotificationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<NotificationDto>> GetUserNotificationsAsync(int userId)
        {
            var notifications = await _unitOfWork.Notifications.FindAsync(n => n.UserId == userId);
            return notifications.Select(n => new NotificationDto
            {
                Id = n.NotificationId,
                UserId = n.UserId,
                RelatedId = n.RelatedId,
                Message = n.Message,
                Type = n.Type.ToString(),
                Status = n.Status.ToString()
            });
        }

        public async Task<bool> MarkAsReadAsync(int notificationId, int userId)
        {
            var notification = await _unitOfWork.Notifications.GetByIdAsync(notificationId);
            if (notification == null || notification.UserId != userId)
                return false;

            notification.Status = NotificationStatus.Read;
            _unitOfWork.Notifications.Update(notification);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> MarkAllAsReadAsync(int userId)
        {
            var notifications = await _unitOfWork.Notifications.FindAsync(n => n.UserId == userId && n.Status == NotificationStatus.Unread);
            foreach (var notification in notifications)
            {
                notification.Status = NotificationStatus.Read;
                _unitOfWork.Notifications.Update(notification);
            }
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int notificationId, int userId)
        {
            var notification = await _unitOfWork.Notifications.GetByIdAsync(notificationId);
            if (notification == null || notification.UserId != userId)
                return false;

            await _unitOfWork.Notifications.DeleteAsync(notification);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> ClearAllAsync(int userId)
        {
            var notifications = await _unitOfWork.Notifications.FindAsync(n => n.UserId == userId);
            foreach (var notification in notifications)
            {
                await _unitOfWork.Notifications.DeleteAsync(notification);
            }
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}

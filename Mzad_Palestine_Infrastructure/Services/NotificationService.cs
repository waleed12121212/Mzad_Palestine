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
using Microsoft.Extensions.Logging;

namespace Mzad_Palestine_Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(IUnitOfWork unitOfWork, ILogger<NotificationService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IEnumerable<NotificationDto>> GetUserNotificationsAsync(int userId)
        {
            try
            {
                _logger.LogInformation($"Getting notifications for user {userId}");
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
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting notifications for user {userId}");
                throw;
            }
        }

        public async Task<bool> MarkAsReadAsync(int notificationId, int userId)
        {
            try
            {
                _logger.LogInformation($"Marking notification {notificationId} as read for user {userId}");
                var notification = await _unitOfWork.Notifications.GetByIdAsync(notificationId);
                if (notification == null || notification.UserId != userId)
                {
                    _logger.LogWarning($"Notification {notificationId} not found or does not belong to user {userId}");
                    return false;
                }

                notification.Status = NotificationStatus.Read;
                _unitOfWork.Notifications.Update(notification);
                await _unitOfWork.CompleteAsync();
                _logger.LogInformation($"Successfully marked notification {notificationId} as read");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error marking notification {notificationId} as read");
                throw;
            }
        }

        public async Task<bool> MarkAllAsReadAsync(int userId)
        {
            try
            {
                _logger.LogInformation($"Marking all notifications as read for user {userId}");
                var notifications = await _unitOfWork.Notifications.FindAsync(n => n.UserId == userId && n.Status == NotificationStatus.Unread);
                
                if (!notifications.Any())
                {
                    _logger.LogInformation($"No unread notifications found for user {userId}");
                    return true;
                }

                foreach (var notification in notifications)
                {
                    notification.Status = NotificationStatus.Read;
                    _unitOfWork.Notifications.Update(notification);
                }

                await _unitOfWork.CompleteAsync();
                _logger.LogInformation($"Successfully marked all notifications as read for user {userId}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error marking all notifications as read for user {userId}");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int notificationId, int userId)
        {
            try
            {
                _logger.LogInformation($"Deleting notification {notificationId} for user {userId}");
                var notification = await _unitOfWork.Notifications.GetByIdAsync(notificationId);
                if (notification == null || notification.UserId != userId)
                {
                    _logger.LogWarning($"Notification {notificationId} not found or does not belong to user {userId}");
                    return false;
                }

                await _unitOfWork.Notifications.DeleteAsync(notification);
                await _unitOfWork.CompleteAsync();
                _logger.LogInformation($"Successfully deleted notification {notificationId}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting notification {notificationId}");
                throw;
            }
        }

        public async Task<bool> ClearAllAsync(int userId)
        {
            try
            {
                _logger.LogInformation($"Clearing all notifications for user {userId}");
                var notifications = await _unitOfWork.Notifications.FindAsync(n => n.UserId == userId);
                
                if (!notifications.Any())
                {
                    _logger.LogInformation($"No notifications found for user {userId}");
                    return true;
                }

                foreach (var notification in notifications)
                {
                    await _unitOfWork.Notifications.DeleteAsync(notification);
                }

                await _unitOfWork.CompleteAsync();
                _logger.LogInformation($"Successfully cleared all notifications for user {userId}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error clearing all notifications for user {userId}");
                throw;
            }
        }
    }
}

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

namespace Mzad_Palestine_Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _repository;
        public NotificationService(INotificationRepository repository) => _repository = repository;

        public async Task<NotificationDto> CreateAsync(NotificationDto dto)
        {
            var entity = new Notification
            {
                UserId = dto.UserId ,
                RelatedId = dto.RelatedId ,
                Message = dto.Message ,
                Type = Enum.Parse<NotificationType>(dto.Type) ,
                Status = Enum.Parse<NotificationStatus>(dto.Status)
            };

            await _repository.AddAsync(entity);

            return new NotificationDto
            {
                Id = entity.UserId ,
                UserId = entity.UserId ,
                RelatedId = entity.RelatedId ,
                Message = entity.Message ,
                Type = entity.Type.ToString() ,
                Status = entity.Status.ToString()
            };
        }

        public Task<IEnumerable<NotificationDto>> GetUserNotificationsAsync(int userId)
        {
            throw new NotImplementedException();
        }
    }
}

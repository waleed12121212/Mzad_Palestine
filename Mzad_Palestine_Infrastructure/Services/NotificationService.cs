using Mzad_Palestine_Core.DTO_s.Notification;
using Mzad_Palestine_Core.Interfaces.Services;
using Mzad_Palestine_Core.Interfaces;
using Mzad_Palestine_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _repository;
        public NotificationService(INotificationRepository repository) => _repository = repository;

        public async Task<NotificationDto> CreateAsync(NotificationDto dto)
        {
            var entity = new Notification { UserId = dto.UserId , RelatedId = dto.RelatedId , Message = dto.Message , Type = dto.Type , Status = dto.Status };
            var created = await _repository.AddAsync(entity);
            return new NotificationDto(created.Id , created.UserId , created.RelatedId , created.Message , created.Type , created.Status);
        }
    }
}

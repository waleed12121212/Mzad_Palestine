using Mzad_Palestine_Core.DTO_s.Message;
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
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _repository;
        public MessageService(IMessageRepository repository) => _repository = repository;

        public async Task<MessageDto> CreateAsync(CreateMessageDto dto)
        {
            var entity = new Message { ReceiverId = dto.ReceiverId , Subject = dto.Subject , Content = dto.Content , Timestamp = DateTime.UtcNow };
            var created = await _repository.AddAsync(entity);
            return new MessageDto(created.Id , created.SenderId , created.ReceiverId , created.Subject , created.Content , created.Timestamp , created.IsRead);
        }
    }
}

using Mzad_Palestine_Core.DTO_s.Message;
using Mzad_Palestine_Core.Interfaces.Services;
using Mzad_Palestine_Core.Interfaces;
using Mzad_Palestine_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Mzad_Palestine_Infrastructure.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _repository;

        public MessageService(IMessageRepository repository)
        {
            _repository = repository;
        }

        public async Task<MessageDto> SendAsync(CreateMessageDto dto)
        {
            var message = new Message
            {
                SenderId = dto.SenderId ,
                ReceiverId = dto.ReceiverId ,
                Subject = dto.Subject ,
                Content = dto.Content ,
                Timestamp = DateTime.UtcNow ,
                IsRead = false
            };

            await _repository.AddAsync(message);
            await _repository.SaveChangesAsync();

            return new MessageDto
            {
                Id = message.MessageId ,
                SenderId = message.SenderId ,
                ReceiverId = message.ReceiverId ,
                Subject = message.Subject ,
                Content = message.Content ,
                Timestamp = message.Timestamp ,
                IsRead = message.IsRead
            };
        }

        public async Task<IEnumerable<MessageDto>> GetInboxAsync(int userId)
        {
            var messages = await _repository.FindAsync(m => m.ReceiverId == userId);
            return messages.Select(m => new MessageDto
            {
                Id = m.MessageId ,
                SenderId = m.SenderId ,
                ReceiverId = m.ReceiverId ,
                Subject = m.Subject ,
                Content = m.Content ,
                Timestamp = m.Timestamp ,
                IsRead = m.IsRead
            });
        }

        public async Task<IEnumerable<MessageDto>> GetSentMessagesAsync(int userId)
        {
            var messages = await _repository.FindAsync(m => m.SenderId == userId);
            return messages.Select(m => new MessageDto
            {
                Id = m.MessageId ,
                SenderId = m.SenderId ,
                ReceiverId = m.ReceiverId ,
                Subject = m.Subject ,
                Content = m.Content ,
                Timestamp = m.Timestamp ,
                IsRead = m.IsRead
            });
        }

        public async Task<IEnumerable<MessageDto>> GetConversationAsync(int currentUserId , int otherUserId)
        {
            var messages = await _repository.GetConversationAsync(currentUserId , otherUserId);
            return messages.Select(m => new MessageDto
            {
                Id = m.MessageId ,
                SenderId = m.SenderId ,
                ReceiverId = m.ReceiverId ,
                Subject = m.Subject ,
                Content = m.Content ,
                Timestamp = m.Timestamp ,
                IsRead = m.IsRead
            });
        }

        public async Task<bool> MarkAsReadAsync(int messageId, int userId)
        {
            var message = await _repository.GetByIdAsync(messageId);
            if (message == null || message.ReceiverId != userId)
                return false;

            message.IsRead = true;
            _repository.Update(message);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MarkAllInboxAsReadAsync(int userId)
        {
            try
            {
                var messages = await _repository.FindAsync(m => m.ReceiverId == userId && !m.IsRead);
                foreach (var message in messages)
                {
                    message.IsRead = true;
                    _repository.Update(message);
                }
                await _repository.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<int> GetUnreadCountAsync(int userId)
        {
            var messages = await _repository.FindAsync(m => m.ReceiverId == userId && !m.IsRead);
            return messages.Count();
        }
    }
}

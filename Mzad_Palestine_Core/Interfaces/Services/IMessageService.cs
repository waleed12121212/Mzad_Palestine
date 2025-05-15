using Mzad_Palestine_Core.DTO_s.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Interfaces.Services
{
    public interface IMessageService
    {
        Task<MessageDto> SendAsync(CreateMessageDto dto);
        Task<IEnumerable<MessageDto>> GetInboxAsync(int userId);
        Task<IEnumerable<MessageDto>> GetSentMessagesAsync(int userId);
        Task<IEnumerable<MessageDto>> GetConversationAsync(int currentUserId, int otherUserId);
        Task<bool> MarkAsReadAsync(int messageId, int userId);
        Task<bool> MarkAllInboxAsReadAsync(int userId);
        Task<int> GetUnreadCountAsync(int userId);
    }
}

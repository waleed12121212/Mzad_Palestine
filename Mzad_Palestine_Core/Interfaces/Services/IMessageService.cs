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
    }
}

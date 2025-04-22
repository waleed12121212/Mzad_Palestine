using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.DTO_s.Message
{
    public class CreateMessageDto
    {
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }

}

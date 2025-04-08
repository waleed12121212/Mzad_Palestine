using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.DTO_s.Notification
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RelatedId { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
    }

}

using Mzad_Palestine_Core.Enums;
using Mzad_Palestine_Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public int UserId { get; set; }
        public int RelatedId { get; set; } // يمكن أن يشير إلى Auction أو Bid أو غيرهما
        public string Message { get; set; }
        public NotificationType Type { get; set; }
        public NotificationStatus Status { get; set; }
        
        private DateTime _createdAt;
        public DateTime CreatedAt
        {
            get => _createdAt.ToPalestineTime();
            set => _createdAt = value.ToUtcFromPalestine();
        }

        // الملاحة
        public User User { get; set; }
    }
}

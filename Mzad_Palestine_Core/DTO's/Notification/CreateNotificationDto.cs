using System;

namespace Mzad_Palestine_Core.DTO_s.Notification
{
    public class CreateNotificationDto
    {
        public int UserId { get; set; }
        public string Message { get; set; }
        public string Type { get; set; } = "General";
    }
} 
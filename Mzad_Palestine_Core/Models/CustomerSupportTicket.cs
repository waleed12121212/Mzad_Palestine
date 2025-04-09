using Mzad_Palestine_Core.Enums;

namespace Mzad_Palestine_Core.Models
{
    public class CustomerSupportTicket
    {
        public int TicketId { get; set; }
        public int UserId { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public TicketStatus Status { get; set; }

        // Navigation Property
        public virtual User User { get; set; }
    }
}

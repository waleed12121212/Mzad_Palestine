using Mzad_Palestine_Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Phone { get; set; }
        public UserRole Role { get; set; }
        public bool IsVerified { get; set; }
        public int ReputationScore { get; set; }
        public DateTime CreatedAt { get; set; }

        // خصائص الملاحة (Navigation Properties)
        public ICollection<Listing> Listings { get; set; }
        public ICollection<Bid> Bids { get; set; }
        public ICollection<Payment> Payments { get; set; }
        public ICollection<Message> SentMessages { get; set; }
        public ICollection<Message> ReceivedMessages { get; set; }
        public ICollection<Review> ReviewsGiven { get; set; }
        public ICollection<Review> ReviewsReceived { get; set; }
        public ICollection<Report> ReportsMade { get; set; }
        public ICollection<Report> ReportsReceived { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        public ICollection<AutoBid> AutoBids { get; set; }
        public ICollection<Dispute> Disputes { get; set; }
        public ICollection<Subscription> Subscriptions { get; set; }
        public ICollection<CustomerSupportTicket> CustomerSupportTickets { get; set; }
    }
}

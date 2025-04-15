using Mzad_Palestine_Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Mzad_Palestine_Core.Models
{
    public class User : IdentityUser<int>
    {
        public string Phone { get; set; }
        public UserRole Role { get; set; }
        public bool IsVerified { get; set; }
        public int ReputationScore { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public ICollection<Listing> Listings { get; set; } = new HashSet<Listing>();
        public ICollection<Bid> Bids { get; set; } = new HashSet<Bid>();
        public ICollection<Payment> Payments { get; set; } = new HashSet<Payment>();
        public ICollection<Message> SentMessages { get; set; } = new HashSet<Message>();
        public ICollection<Message> ReceivedMessages { get; set; } = new HashSet<Message>();
        public ICollection<Review> ReviewsGiven { get; set; } = new HashSet<Review>();
        public ICollection<Review> ReviewsReceived { get; set; } = new HashSet<Review>();
        public ICollection<Report> ReportsMade { get; set; } = new HashSet<Report>();
        public ICollection<Report> ReportsReceived { get; set; } = new HashSet<Report>();
        public ICollection<Notification> Notifications { get; set; } = new HashSet<Notification>();
        public ICollection<AutoBid> AutoBids { get; set; } = new HashSet<AutoBid>();
        public ICollection<Dispute> Disputes { get; set; } = new HashSet<Dispute>();
        public ICollection<Subscription> Subscriptions { get; set; } = new HashSet<Subscription>();
        public ICollection<CustomerSupportTicket> CustomerSupportTickets { get; set; } = new HashSet<CustomerSupportTicket>();
        public ICollection<Watchlist> Watchlists { get; set; } = new HashSet<Watchlist>();
        public ICollection<Invoice> Invoices { get; set; } = new HashSet<Invoice>();
    }
}

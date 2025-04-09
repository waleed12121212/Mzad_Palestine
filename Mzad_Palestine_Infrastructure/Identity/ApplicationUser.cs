using Microsoft.AspNetCore.Identity;
using Mzad_Palestine_Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public DateTime CreatedAt { get; set; }
        public UserRole Role { get; set; }
        public string Phone { get; set; }
        public bool IsVerified { get; set; }
        public int ReputationScore { get; set; }

        // Navigation Properties
        public virtual ICollection<Listing> Listings { get; set; }
        public virtual ICollection<Bid> Bids { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<Message> SentMessages { get; set; }
        public virtual ICollection<Message> ReceivedMessages { get; set; }
        public virtual ICollection<Review> ReviewsGiven { get; set; }
        public virtual ICollection<Review> ReviewsReceived { get; set; }
        public virtual ICollection<Report> ReportsMade { get; set; }
        public virtual ICollection<Report> ResolvedReports { get; set; }
        public virtual ICollection<Report> ReportsReceived { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<AutoBid> AutoBids { get; set; }
        public virtual ICollection<Dispute> Disputes { get; set; }
        public virtual ICollection<Dispute> ResolvedDisputes { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }
        public virtual ICollection<CustomerSupportTicket> CustomerSupportTickets { get; set; }
        public virtual ICollection<Watchlist> Watchlists { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }

        public ApplicationUser( )
        {
            Listings = new HashSet<Listing>();
            Bids = new HashSet<Bid>();
            Payments = new HashSet<Payment>();
            SentMessages = new HashSet<Message>();
            ReceivedMessages = new HashSet<Message>();
            ReviewsGiven = new HashSet<Review>();
            ReviewsReceived = new HashSet<Review>();
            ReportsMade = new HashSet<Report>();
            ReportsReceived = new HashSet<Report>();
            Notifications = new HashSet<Notification>();
            AutoBids = new HashSet<AutoBid>();
            Disputes = new HashSet<Dispute>();
            Subscriptions = new HashSet<Subscription>();
            CustomerSupportTickets = new HashSet<CustomerSupportTicket>();
            Watchlists = new HashSet<Watchlist>();
            Invoices = new HashSet<Invoice>();
        }
    }
}

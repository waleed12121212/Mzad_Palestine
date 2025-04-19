using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mzad_Palestine_Core.Interfaces.Repositories;

namespace Mzad_Palestine_Core.Interfaces.Common
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IListingRepository Listings { get; }
        IAuctionRepository Auctions { get; }
        IBidRepository Bids { get; }
        IPaymentRepository Payments { get; }
        IMessageRepository Messages { get; }
        IReviewRepository Reviews { get; }
        IReportRepository Reports { get; }
        INotificationRepository Notifications { get; }
        IAutoBidRepository AutoBids { get; }
        IDisputeRepository Disputes { get; }
        ITagRepository Tags { get; }
        IWatchlistRepository Watchlists { get; }
        ISubscriptionRepository Subscriptions { get; }
        ICustomerSupportTicketRepository CustomerSupportTickets { get; }
        ICategoryRepository Categories { get; }
        Task<int> CompleteAsync( );
        
    }
}

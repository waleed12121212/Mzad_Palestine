using Mzad_Palestine_Core.Interfaces;
using Mzad_Palestine_Core.Interfaces.Common;
using Mzad_Palestine_Core.Interfaces.Repositories;
using Mzad_Palestine_Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Infrastructure.Repositories.Common
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context ,
            IUserRepository userRepository ,
            IListingRepository listingRepository ,
            IAuctionRepository auctionRepository ,
            IBidRepository bidRepository ,
            IPaymentRepository paymentRepository ,
            IMessageRepository messageRepository ,
            IReviewRepository reviewRepository ,
            IReportRepository reportRepository ,
            INotificationRepository notificationRepository ,
            IAutoBidRepository autoBidRepository ,
            IDisputeRepository disputeRepository ,
            ITagRepository tagRepository ,
            IWatchlistRepository watchlistRepository ,
            ISubscriptionRepository subscriptionRepository ,
            ICustomerSupportTicketRepository customerSupportTicketRepository ,
            ICategoryRepository categoryRepository ,
            IListingImageRepository listingImageRepository)
        {
            _context = context;
            Users = userRepository;
            Listings = listingRepository;
            Auctions = auctionRepository;
            Bids = bidRepository;
            Payments = paymentRepository;
            Messages = messageRepository;
            Reviews = reviewRepository;
            Reports = reportRepository;
            Notifications = notificationRepository;
            AutoBids = autoBidRepository;
            Disputes = disputeRepository;
            Tags = tagRepository;
            Watchlists = watchlistRepository;
            Subscriptions = subscriptionRepository;
            CustomerSupportTickets = customerSupportTicketRepository;
            Categories = categoryRepository;
            ListingImages = listingImageRepository;
        }

        public IUserRepository Users { get; }
        public IListingRepository Listings { get; }
        public IAuctionRepository Auctions { get; }
        public IBidRepository Bids { get; }
        public IPaymentRepository Payments { get; }
        public IMessageRepository Messages { get; }
        public IReviewRepository Reviews { get; }
        public IReportRepository Reports { get; }
        public INotificationRepository Notifications { get; }
        public IAutoBidRepository AutoBids { get; }
        public IDisputeRepository Disputes { get; }
        public ITagRepository Tags { get; }
        public IWatchlistRepository Watchlists { get; }
        public ISubscriptionRepository Subscriptions { get; }
        public ICustomerSupportTicketRepository CustomerSupportTickets { get; }
        public ICategoryRepository Categories { get; }
        public IListingImageRepository ListingImages { get; }

        public async Task<int> CompleteAsync( )
        {
            return await _context.SaveChangesAsync();
        }
    }
}

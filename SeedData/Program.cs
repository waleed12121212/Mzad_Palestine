using Microsoft.EntityFrameworkCore;
using Mzad_Palestine_Core.Enums;
using Mzad_Palestine_Core.Models;
using Mzad_Palestine_Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace SeedData
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), 
                sqlServerOptions => sqlServerOptions.EnableRetryOnFailure());

            using var context = new ApplicationDbContext(optionsBuilder.Options);

            var user1 = new User
            {
                UserName = "hasan",
                Email = "hasan@example.com",
                EmailConfirmed = true,
                PasswordHash = "fakehash",
                Phone = "0599999999",
                Role = UserRole.Buyer,
                IsVerified = true,
                ReputationScore = 100,
                CreatedAt = DateTime.Now
            };
            var user2 = new User
            {
                UserName = "saad",
                Email = "saad@example.com",
                EmailConfirmed = true,
                PasswordHash = "fakehash2",
                Phone = "0588888888",
                Role = UserRole.Seller,
                IsVerified = true,
                ReputationScore = 50,
                CreatedAt = DateTime.Now
            };

            context.Users.AddRange(user1, user2);
            context.SaveChanges();

            var cat1 = new Category { Name = "إلكترونيات", Description = "أجهزة إلكترونية", ImageUrl = "electronics.jpg" };
            var cat2 = new Category { Name = "أثاث", Description = "أثاث منزلي", ImageUrl = "furniture.jpg" };

            context.Categories.AddRange(cat1, cat2);
            context.SaveChanges();

            var tag1 = new Tag { Name = "جديد", Description = "المنتج جديد" };
            var tag2 = new Tag { Name = "مستعمل", Description = "المنتج مستخدم" };

            context.Tags.AddRange(tag1, tag2);
            context.SaveChanges();

            var listing1 = new Listing
            {
                UserId = context.Users.First().Id,
                Title = "جهاز لابتوب",
                Description = "لابتوب جديد بحالة ممتازة",
                Price = 1500,
                CategoryId = context.Categories.First().Id,
                LocationId = 1,
                Type = ListingType.Auction,
                Status = ListingStatus.Active,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            var listing2 = new Listing
            {
                UserId = context.Users.Skip(1).First().Id,
                Title = "كنبة جلد",
                Description = "كنبة جلد استعمال خفيف",
                Price = 300,
                CategoryId = context.Categories.Skip(1).First().Id,
                LocationId = 1,
                Type = ListingType.Normal,
                Status = ListingStatus.Active,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            context.Listings.AddRange(listing1, listing2);
            context.SaveChanges();

            var lt1 = new ListingTag { ListingId = listing1.ListingId, TagId = context.Tags.First().TagId };
            var lt2 = new ListingTag { ListingId = listing2.ListingId, TagId = context.Tags.Skip(1).First().TagId };

            context.ListingTags.AddRange(lt1, lt2);
            context.SaveChanges();

            var auction1 = new Auction
            {
                ListingId = context.Listings.First().ListingId,
                Name = "مزاد لابتوب",
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddDays(7),
                ReservePrice = 1500,
                CurrentBid = 1500,
                BidIncrement = 50,
                Status = AuctionStatus.Open,
                ImageUrl = "laptop.jpg",
                CreatedAt = DateTime.Now
            };

            var auction2 = new Auction
            {
                ListingId = context.Listings.Skip(1).First().ListingId,
                Name = "مزاد كنبة",
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddDays(5),
                ReservePrice = 300,
                CurrentBid = 300,
                BidIncrement = 20,
                Status = AuctionStatus.Open,
                ImageUrl = "sofa.jpg",
                CreatedAt = DateTime.Now
            };

            context.Auctions.AddRange(auction1, auction2);
            context.SaveChanges();

            var bid1 = new Bid
            {
                AuctionId = context.Auctions.First().AuctionId,
                UserId = context.Users.First().Id,
                BidAmount = 1550,
                BidTime = DateTime.Now,
                IsAutoBid = false,
                IsWinner = false,
                Status = BidStatus.Accepted
            };

            var bid2 = new Bid
            {
                AuctionId = context.Auctions.Skip(1).First().AuctionId,
                UserId = context.Users.Skip(1).First().Id,
                BidAmount = 320,
                BidTime = DateTime.Now,
                IsAutoBid = false,
                IsWinner = false,
                Status = BidStatus.Accepted
            };

            context.Bids.AddRange(bid1, bid2);
            context.SaveChanges();

            var autoBid1 = new AutoBid
            {
                UserId = context.Users.First().Id,
                AuctionId = context.Auctions.First().AuctionId,
                MaxBid = 2000,
                CurrentBid = 1550,
                Status = AutoBidStatus.Active
            };

            var autoBid2 = new AutoBid
            {
                UserId = context.Users.Skip(1).First().Id,
                AuctionId = context.Auctions.Skip(1).First().AuctionId,
                MaxBid = 400,
                CurrentBid = 320,
                Status = AutoBidStatus.Active
            };

            context.AutoBids.AddRange(autoBid1, autoBid2);
            context.SaveChanges();

            var review1 = new Review
            {
                ReviewerId = context.Users.First().Id,
                ReviewedUserId = context.Users.Skip(1).First().Id,
                ListingId = context.Listings.First().ListingId,
                Rating = 5,
                Comment = "بائع ممتاز وسريع في التواصل",
                CreatedAt = DateTime.Now
            };

            var review2 = new Review
            {
                ReviewerId = context.Users.Skip(1).First().Id,
                ReviewedUserId = context.Users.First().Id,
                ListingId = context.Listings.Skip(1).First().ListingId,
                Rating = 4,
                Comment = "تجربة شراء جيدة",
                CreatedAt = DateTime.Now
            };

            context.Reviews.AddRange(review1, review2);
            context.SaveChanges();

            var message1 = new Message
            {
                SenderId = context.Users.First().Id,
                ReceiverId = context.Users.Skip(1).First().Id,
                Subject = "استفسار عن المنتج",
                Content = "هل المنتج ما زال متوفر؟",
                Timestamp = DateTime.Now,
                IsRead = false
            };

            var message2 = new Message
            {
                SenderId = context.Users.Skip(1).First().Id,
                ReceiverId = context.Users.First().Id,
                Subject = "رد على الاستفسار",
                Content = "نعم، المنتج متوفر",
                Timestamp = DateTime.Now,
                IsRead = false
            };

            context.Messages.AddRange(message1, message2);
            context.SaveChanges();

            var notification1 = new Notification
            {
                UserId = context.Users.First().Id,
                RelatedId = context.Auctions.First().AuctionId,
                Message = "تم تقديم مزايدة جديدة على منتجك",
                Type = NotificationType.Bid,
                Status = NotificationStatus.Unread,
                CreatedAt = DateTime.Now
            };

            var notification2 = new Notification
            {
                UserId = context.Users.Skip(1).First().Id,
                RelatedId = context.Messages.First().MessageId,
                Message = "لديك رسالة جديدة",
                Type = NotificationType.General,
                Status = NotificationStatus.Unread,
                CreatedAt = DateTime.Now
            };

            context.Notifications.AddRange(notification1, notification2);
            context.SaveChanges();

            var payment1 = new Payment
            {
                UserId = context.Users.First().Id,
                AuctionId = context.Auctions.First().AuctionId,
                Amount = 1550,
                Currency = "ILS",
                Method = PaymentMethod.CreditCard,
                EscrowStatus = PaymentEscrowStatus.Pending,
                Status = PaymentStatus.Completed,
                TransactionDate = DateTime.Now
            };

            var payment2 = new Payment
            {
                UserId = context.Users.Skip(1).First().Id,
                AuctionId = context.Auctions.Skip(1).First().AuctionId,
                Amount = 320,
                Currency = "ILS",
                Method = PaymentMethod.BankTransfer,
                EscrowStatus = PaymentEscrowStatus.Pending,
                Status = PaymentStatus.Completed,
                TransactionDate = DateTime.Now
            };

            context.Payments.AddRange(payment1, payment2);
            context.SaveChanges();

            var invoice1 = new Invoice
            {
                UserId = context.Users.First().Id,
                ListingId = context.Listings.First().ListingId,
                Amount = 1550,
                DueDate = DateTime.Now.AddDays(7),
                Status = InvoiceStatus.Paid,
                IssuedAt = DateTime.Now
            };

            var invoice2 = new Invoice
            {
                UserId = context.Users.Skip(1).First().Id,
                ListingId = context.Listings.Skip(1).First().ListingId,
                Amount = 320,
                DueDate = DateTime.Now.AddDays(7),
                Status = InvoiceStatus.Paid,
                IssuedAt = DateTime.Now
            };

            context.Invoices.AddRange(invoice1, invoice2);
            context.SaveChanges();

            var report1 = new Report
            {
                ReporterId = context.Users.First().Id,
                ReportedListingId = context.Listings.First().ListingId,
                Reason = "وصف غير دقيق للمنتج",
                Status = ReportStatus.Pending,
                CreatedAt = DateTime.Now
            };

            var report2 = new Report
            {
                ReporterId = context.Users.Skip(1).First().Id,
                ReportedListingId = context.Listings.Skip(1).First().ListingId,
                Reason = "سعر غير مناسب",
                Status = ReportStatus.Pending,
                CreatedAt = DateTime.Now
            };

            context.Reports.AddRange(report1, report2);
            context.SaveChanges();

            var dispute1 = new Dispute
            {
                UserId = context.Users.First().Id,
                AuctionId = context.Auctions.First().AuctionId,
                Reason = "لم يتم تسليم المنتج",
                Status = DisputeStatus.Open,
                CreatedAt = DateTime.Now
            };

            var dispute2 = new Dispute
            {
                UserId = context.Users.Skip(1).First().Id,
                AuctionId = context.Auctions.Skip(1).First().AuctionId,
                Reason = "المنتج غير مطابق للوصف",
                Status = DisputeStatus.Open,
                CreatedAt = DateTime.Now
            };

            context.Disputes.AddRange(dispute1, dispute2);
            context.SaveChanges();

            var ticket1 = new CustomerSupportTicket
            {
                UserId = context.Users.First().Id,
                Subject = "مشكلة في الدفع",
                Description = "لا يمكنني إتمام عملية الدفع",
                Status = TicketStatus.Open,
                CreatedAt = DateTime.Now
            };

            var ticket2 = new CustomerSupportTicket
            {
                UserId = context.Users.Skip(1).First().Id,
                Subject = "استفسار عن المزاد",
                Description = "كيف يمكنني المشاركة في المزاد؟",
                Status = TicketStatus.Open,
                CreatedAt = DateTime.Now
            };

            context.CustomerSupportTickets.AddRange(ticket1, ticket2);
            context.SaveChanges();

            var subscription1 = new Subscription
            {
                UserId = context.Users.First().Id,
                Plan = "Premium",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1),
                RenewalDate = DateTime.Now.AddMonths(1),
                Status = "Active"
            };

            var subscription2 = new Subscription
            {
                UserId = context.Users.Skip(1).First().Id,
                Plan = "Basic",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1),
                RenewalDate = DateTime.Now.AddMonths(1),
                Status = "Active"
            };

            context.Subscriptions.AddRange(subscription1, subscription2);
            context.SaveChanges();

            var transaction1 = new Transaction
            {
                UserId = context.Users.First().Id,
                Amount = 1550,
                TransactionDate = DateTime.Now,
                TransactionType = "Purchase",
                Status = "Completed",
                Description = "شراء لابتوب"
            };

            var transaction2 = new Transaction
            {
                UserId = context.Users.Skip(1).First().Id,
                Amount = 320,
                TransactionDate = DateTime.Now,
                TransactionType = "Sale",
                Status = "Completed",
                Description = "بيع كنبة"
            };

            context.Transactions.AddRange(transaction1, transaction2);
            context.SaveChanges();

            var watchlist1 = new Watchlist
            {
                UserId = context.Users.First().Id,
                ListingId = context.Listings.First().ListingId,
                AddedAt = DateTime.Now
            };

            var watchlist2 = new Watchlist
            {
                UserId = context.Users.Skip(1).First().Id,
                ListingId = context.Listings.Skip(1).First().ListingId,
                AddedAt = DateTime.Now
            };

            context.Watchlists.AddRange(watchlist1, watchlist2);
            context.SaveChanges();

            Console.WriteLine("✅ تم تعبئة جميع البيانات بنجاح!");
        }
    }
}

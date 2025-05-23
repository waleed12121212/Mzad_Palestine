﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Mzad_Palestine_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<User , IdentityRole<int> , int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Listing> Listings { get; set; }
        public DbSet<Auction> Auctions { get; set; }
        public DbSet<Bid> Bids { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<AutoBid> AutoBids { get; set; }
        public DbSet<Dispute> Disputes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ListingTag> ListingTags { get; set; }
        public DbSet<Watchlist> Watchlists { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<CustomerSupportTicket> CustomerSupportTickets { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Phone> Phones { get; set; }
        public DbSet<AuctionImage> AuctionImages { get; set; }

        // Override لـ OnModelCreating لتكوين العلاقات والقيود باستخدام Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region تكوين الكيان User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.UserName)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(u => u.Email)
                      .IsRequired()
                      .HasMaxLength(255);
                entity.Property(u => u.PasswordHash)
                      .IsRequired();
                entity.Property(u => u.CreatedAt)
                      .HasDefaultValueSql("GETDATE()");
                // يمكن إضافة فهارس أو قيود إضافية حسب الحاجة
            });
            #endregion

            #region تكوين Listing
            modelBuilder.Entity<Listing>(entity =>
            {
                entity.HasKey(l => l.ListingId);
                entity.Property(l => l.Title)
                      .IsRequired()
                      .HasMaxLength(255);
                entity.Property(l => l.Description)
                      .HasMaxLength(2000);
                entity.Property(l => l.CreatedAt)
                      .HasDefaultValueSql("GETDATE()");
                entity.Property(l => l.UpdatedAt)
                      .HasDefaultValueSql("GETDATE()");
                
                // إضافة نوع للأعمدة العشرية
                entity.Property(l => l.Price)
                      .HasColumnType("decimal(18,2)");
                
                // العلاقة مع User
                entity.HasOne(l => l.User)
                      .WithMany(u => u.Listings)
                      .HasForeignKey(l => l.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
                
                // العلاقة مع Category
                entity.HasOne(l => l.Category)
                      .WithMany(c => c.Listings)
                      .HasForeignKey(l => l.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
            #endregion

            #region تكوين Category
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Name)
                      .IsRequired()
                      .HasMaxLength(255)
                      .HasColumnType("nvarchar(255)");
                entity.Property(c => c.Description)
                      .HasColumnType("nvarchar(max)")
                      .HasDefaultValue(null);
                entity.Property(c => c.ImageUrl)
                      .HasMaxLength(255)
                      .HasColumnType("nvarchar(255)");
                // العلاقة مع نفسها (فئة فرعية)
                entity.HasOne(c => c.ParentCategory)
                      .WithMany(c => c.SubCategories)
                      .HasForeignKey(c => c.ParentCategoryId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
            #endregion

            #region تكوين Auction
            modelBuilder.Entity<Auction>(entity =>
            {
                entity.HasKey(a => a.AuctionId);
                entity.Property(a => a.Title)
                      .IsRequired()
                      .HasMaxLength(255);
                entity.Property(a => a.Description)
                      .HasMaxLength(2000);
                entity.Property(a => a.ReservePrice)
                      .HasColumnType("decimal(18,2)");
                entity.Property(a => a.CurrentBid)
                      .HasColumnType("decimal(18,2)");
                entity.Property(a => a.BidIncrement)
                      .HasColumnType("decimal(18,2)");
                entity.Property(a => a.StartDate)
                      .IsRequired();
                entity.Property(a => a.EndDate)
                      .IsRequired();
                entity.Property(a => a.Status)
                      .IsRequired()
                      .HasMaxLength(50);
                entity.Property(a => a.CreatedAt)
                      .HasDefaultValueSql("GETDATE()");
                entity.Property(a => a.UpdatedAt)
                      .HasDefaultValueSql("GETDATE()");

                // العلاقة مع User (المالك)
                entity.HasOne(a => a.User)
                      .WithMany()
                      .HasForeignKey(a => a.UserId)
                      .OnDelete(DeleteBehavior.NoAction);

                // العلاقة مع Winner
                entity.HasOne(a => a.Winner)
                      .WithMany()
                      .HasForeignKey(a => a.WinnerId)
                      .OnDelete(DeleteBehavior.NoAction);

                // العلاقة مع Category
                entity.HasOne(a => a.Category)
                      .WithMany()
                      .HasForeignKey(a => a.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
            #endregion

            #region تكوين AuctionImage
            modelBuilder.Entity<AuctionImage>(entity =>
            {
                entity.HasKey(ai => ai.ImageId);
                entity.Property(ai => ai.ImageUrl)
                      .IsRequired()
                      .HasMaxLength(255);
                entity.Property(ai => ai.IsMain)
                      .HasDefaultValue(false);
                entity.Property(ai => ai.CreatedAt)
                      .HasDefaultValueSql("GETDATE()");

                // العلاقة مع Auction
                entity.HasOne(ai => ai.Auction)
                      .WithMany(a => a.Images)
                      .HasForeignKey(ai => ai.AuctionId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion

            #region تكوين Bid
            modelBuilder.Entity<Bid>(entity =>
            {
                entity.HasKey(b => b.BidId);
                entity.Property(b => b.BidAmount)
                      .HasColumnType("decimal(10,2)");
                entity.Property(b => b.BidTime)
                      .HasDefaultValueSql("GETDATE()");
                // العلاقة مع Auction
                entity.HasOne(b => b.Auction)
                      .WithMany(a => a.Bids)
                      .HasForeignKey(b => b.AuctionId)
                      .OnDelete(DeleteBehavior.NoAction);
                // العلاقة مع User
                entity.HasOne(b => b.User)
                      .WithMany(u => u.Bids)
                      .HasForeignKey(b => b.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion

            #region تكوين Payment
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payments");
                entity.HasKey(p => p.PaymentId);
                entity.Property(p => p.Amount)
                      .HasColumnType("decimal(10,2)");
                entity.Property(p => p.Method)
                      .IsRequired()
                      .HasMaxLength(50);
                entity.Property(p => p.Status)
                      .IsRequired()
                      .HasMaxLength(50);
                entity.Property(p => p.TransactionId)
                      .HasMaxLength(100);
                entity.Property(p => p.Notes)
                      .HasMaxLength(500);
                entity.Property(p => p.CreatedAt)
                      .HasDefaultValueSql("GETDATE()");
                entity.Property(p => p.UpdatedAt);

                // العلاقة مع User
                entity.HasOne(p => p.User)
                      .WithMany(u => u.Payments)
                      .HasForeignKey(p => p.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
                // العلاقة مع Auction
                entity.HasOne(p => p.Auction)
                      .WithMany(a => a.Payments)
                      .HasForeignKey(p => p.AuctionId)
                      .OnDelete(DeleteBehavior.NoAction);
            });
            #endregion

            #region تكوين Invoice
            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.HasKey(i => i.InvoiceId);
                entity.Property(i => i.Amount)
                      .HasColumnType("decimal(10,2)");
                entity.Property(i => i.IssuedAt)
                      .HasDefaultValueSql("GETDATE()");
                // العلاقة مع User
                entity.HasOne(i => i.User)
                      .WithMany(u => u.Invoices)
                      .HasForeignKey(i => i.UserId)
                      .OnDelete(DeleteBehavior.NoAction);
                // العلاقة مع Listing
                entity.HasOne(i => i.Listing)
                      .WithMany(l => l.Invoices)
                      .HasForeignKey(i => i.ListingId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
            #endregion

            #region تكوين Message
            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(m => m.MessageId);
                entity.Property(m => m.Subject)
                      .HasMaxLength(255);
                entity.Property(m => m.Timestamp)
                      .HasDefaultValueSql("GETDATE()");
                // العلاقة مع Sender والReceiver (علاقة ذاتية)
                entity.HasOne(m => m.Sender)
                      .WithMany(u => u.SentMessages)
                      .HasForeignKey(m => m.SenderId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(m => m.Receiver)
                      .WithMany(u => u.ReceivedMessages)
                      .HasForeignKey(m => m.ReceiverId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
            #endregion

            #region تكوين Transaction
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(t => t.TransactionId);
                entity.Property(t => t.Amount)
                      .HasColumnType("decimal(18,2)");
                entity.Property(t => t.TransactionDate)
                      .HasDefaultValueSql("GETDATE()");
                entity.HasOne(t => t.User)
                      .WithMany()
                      .HasForeignKey(t => t.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion

            #region تكوين Review
            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Rating)
                      .IsRequired();
                entity.Property(r => r.CreatedAt)
                      .HasDefaultValueSql("GETDATE()");
                // العلاقة مع Reviewer, ReviewedUser و Listing
                entity.HasOne(r => r.Reviewer)
                      .WithMany(u => u.ReviewsGiven)
                      .HasForeignKey(r => r.ReviewerId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(r => r.ReviewedUser)
                      .WithMany(u => u.ReviewsReceived)
                      .HasForeignKey(r => r.ReviewedUserId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(r => r.Listing)
                      .WithMany(l => l.Reviews)
                      .HasForeignKey(r => r.ListingId)
                      .OnDelete(DeleteBehavior.NoAction);
            });
            #endregion

            #region تكوين Report
            modelBuilder.Entity<Report>(entity =>
            {
                entity.HasKey(e => e.ReportId);
                entity.Property(e => e.Reason).IsRequired();
                entity.Property(e => e.Status).IsRequired();
                entity.Property(e => e.ReportType).IsRequired();
                
                entity.HasOne(e => e.Reporter)
                    .WithMany()
                    .HasForeignKey(e => e.ReporterId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Resolver)
                    .WithMany()
                    .HasForeignKey(e => e.ResolvedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.ReportedListing)
                    .WithMany()
                    .HasForeignKey(e => e.ReportedListingId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.ReportedAuction)
                    .WithMany()
                    .HasForeignKey(e => e.ReportedAuctionId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            #endregion

            #region تكوين Notification
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(n => n.NotificationId);
                entity.Property(n => n.CreatedAt)
                      .HasDefaultValueSql("GETDATE()");
                // العلاقة مع User
                entity.HasOne(n => n.User)
                      .WithMany(u => u.Notifications)
                      .HasForeignKey(n => n.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion

            #region تكوين AutoBid
            modelBuilder.Entity<AutoBid>(entity =>
            {
                entity.HasKey(ab => ab.AutoBidId);
                entity.Property(ab => ab.MaxBid)
                      .HasColumnType("decimal(10,2)");
                entity.Property(ab => ab.CurrentBid)
                      .HasColumnType("decimal(10,2)");
                // العلاقة مع User
                entity.HasOne(ab => ab.User)
                      .WithMany(u => u.AutoBids)
                      .HasForeignKey(ab => ab.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
                // العلاقة مع Auction
                entity.HasOne(ab => ab.Auction)
                      .WithMany(a => a.AutoBids)
                      .HasForeignKey(ab => ab.AuctionId)
                      .OnDelete(DeleteBehavior.NoAction);
            });
            #endregion

            #region تكوين Dispute
            modelBuilder.Entity<Dispute>(entity =>
            {
                entity.HasKey(d => d.DisputeId);
                entity.Property(d => d.CreatedAt)
                      .HasDefaultValueSql("GETDATE()");
                // العلاقة مع User و Auction و Resolver
                entity.HasOne(d => d.User)
                      .WithMany(u => u.Disputes)
                      .HasForeignKey(d => d.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(d => d.Auction)
                      .WithMany(a => a.Disputes)
                      .HasForeignKey(d => d.AuctionId)
                      .OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(d => d.Resolver)
                      .WithMany()
                      .HasForeignKey(d => d.ResolvedBy)
                      .OnDelete(DeleteBehavior.Restrict);
            });
            #endregion

            #region تكوين Tag و ListingTag
            modelBuilder.Entity<Tag>(entity =>
            {
                entity.HasKey(t => t.TagId);
                entity.Property(t => t.Name)
                      .IsRequired()
                      .HasMaxLength(255);
            });

            modelBuilder.Entity<ListingTag>(entity =>
            {
                // مفتاح مركب
                entity.HasKey(lt => new { lt.ListingId , lt.TagId });
                // العلاقة مع Listing
                entity.HasOne(lt => lt.Listing)
                      .WithMany(l => l.ListingTags)
                      .HasForeignKey(lt => lt.ListingId)
                      .OnDelete(DeleteBehavior.Cascade);
                // العلاقة مع Tag
                entity.HasOne(lt => lt.Tag)
                      .WithMany(t => t.ListingTags)
                      .HasForeignKey(lt => lt.TagId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion

            #region تكوين Watchlist
            modelBuilder.Entity<Watchlist>(entity =>
            {
                entity.HasKey(w => w.WatchlistId);
                entity.Property(w => w.AddedAt)
                      .HasDefaultValueSql("GETDATE()");
                entity.HasOne(w => w.User)
                      .WithMany(u => u.Watchlists)
                      .HasForeignKey(w => w.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(w => w.Listing)
                      .WithMany(l => l.Watchlists)
                      .HasForeignKey(w => w.ListingId)
                      .OnDelete(DeleteBehavior.NoAction);
            });
            #endregion

            #region تكوين Subscription
            modelBuilder.Entity<Subscription>(entity =>
            {
                entity.HasKey(s => s.SubscriptionId);
                // يمكن إضافة إعدادات إضافية حسب الحاجة
                entity.HasOne(s => s.User)
                      .WithMany(u => u.Subscriptions)
                      .HasForeignKey(s => s.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion

            #region تكوين CustomerSupportTicket
            modelBuilder.Entity<CustomerSupportTicket>(entity =>
            {
                entity.HasKey(t => t.TicketId);
                entity.Property(t => t.CreatedAt)
                      .HasDefaultValueSql("GETDATE()");
                entity.HasOne(t => t.User)
                      .WithMany(u => u.CustomerSupportTickets)
                      .HasForeignKey(t => t.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion
        }
    }
}

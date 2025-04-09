using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Mzad_Palestine_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Infrastructure.Identity
{
    public class ApplicationIdentityDbContext : IdentityDbContext<ApplicationUser , IdentityRole<int> , int>
    {
        public ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options)
            : base(options)
        {
        }

        // Override لتكوين العلاقات والإعدادات الخاصة بالهوية
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure ApplicationUser properties
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(u => u.UserName)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(u => u.Email)
                      .IsRequired()
                      .HasMaxLength(255);
                entity.Property(u => u.CreatedAt)
                      .HasDefaultValueSql("GETDATE()");

                // Configure Message relationships
                entity.HasMany(u => u.SentMessages)
                      .WithOne(m => m.Sender)
                      .HasForeignKey(m => m.SenderId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(u => u.ReceivedMessages)
                      .WithOne(m => m.Receiver)
                      .HasForeignKey(m => m.ReceiverId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Configure Report relationships
                entity.HasMany(u => u.ReportsMade)
                      .WithOne(r => r.Reporter)
                      .HasForeignKey(r => r.ReporterId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(u => u.ResolvedReports)
                      .WithOne(r => r.Resolver)
                      .HasForeignKey(r => r.ResolvedBy)
                      .OnDelete(DeleteBehavior.Restrict);

                // Configure Review relationships
                entity.HasMany(u => u.ReviewsGiven)
                      .WithOne(r => r.Reviewer)
                      .HasForeignKey(r => r.ReviewerId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(u => u.ReviewsReceived)
                      .WithOne(r => r.ReviewedUser)
                      .HasForeignKey(r => r.ReviewedUserId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Configure Dispute relationships
                entity.HasMany(u => u.Disputes)
                      .WithOne(d => d.User)
                      .HasForeignKey(d => d.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(u => u.ResolvedDisputes)
                      .WithOne(d => d.Resolver)
                      .HasForeignKey(d => d.ResolvedBy)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // يمكن تعديل أسماء الجداول إن رغبت
            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<IdentityRole<int>>().ToTable("Roles");
            builder.Entity<IdentityUserRole<int>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
            builder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");
        }
    }
}

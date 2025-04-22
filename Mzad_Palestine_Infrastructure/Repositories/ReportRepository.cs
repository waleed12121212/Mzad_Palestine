using Microsoft.EntityFrameworkCore;
using Mzad_Palestine_Core.Interfaces;
using Mzad_Palestine_Core.Models;
using Mzad_Palestine_Infrastructure.Data;
using Mzad_Palestine_Infrastructure.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mzad_Palestine_Infrastructure.Repositories
{
    public class ReportRepository : GenericRepository<Report>, IReportRepository
    {
        private readonly DbSet<Report> _reports;
        private readonly ApplicationDbContext _context;

        public ReportRepository(ApplicationDbContext context) : base(context)
        {
            _reports = context.Set<Report>();
            _context = context;
        }

        public override async Task<IEnumerable<Report>> GetAllAsync()
        {
            return await _reports
                .Include(r => r.Reporter)
                .Include(r => r.ReportedListing)
                .Include(r => r.Resolver)
                .ToListAsync();
        }

        public override async Task<Report> GetByIdAsync(int id)
        {
            return await _reports
                .Include(r => r.Reporter)
                .Include(r => r.ReportedListing)
                .Include(r => r.Resolver)
                .FirstOrDefaultAsync(r => r.ReportId == id);
        }

        public async Task<Report> CreateAsync(Report report)
        {
            try
            {
                // Start a transaction
                await using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    // Verify that related entities exist
                    var reporterExists = await _context.Users.AnyAsync(u => u.Id == report.ReporterId);
                    if (!reporterExists)
                        throw new InvalidOperationException($"User with ID {report.ReporterId} does not exist");

                    var listingExists = await _context.Listings.AnyAsync(l => l.ListingId == report.ReportedListingId);
                    if (!listingExists)
                        throw new InvalidOperationException($"Listing with ID {report.ReportedListingId} does not exist");

                    // Set default values
                    report.CreatedAt = DateTime.UtcNow;
                    report.Status = "Pending";
                    
                    // Add the report
                    await _reports.AddAsync(report);
                    
                    // Save changes
                    var saveResult = await _context.SaveChangesAsync();
                    if (saveResult <= 0)
                        throw new Exception("Failed to save the report to the database");

                    // Commit transaction
                    await transaction.CommitAsync();

                    // Reload the report with navigation properties
                    return await _reports
                        .Include(r => r.Reporter)
                        .Include(r => r.ReportedListing)
                        .FirstOrDefaultAsync(r => r.ReportId == report.ReportId);
                }
                catch (Exception ex)
                {
                    // Rollback transaction on error
                    await transaction.RollbackAsync();
                    throw new Exception($"Database operation failed: {ex.Message}", ex);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating report: {ex.Message}", ex);
            }
        }

        public async Task<Report> UpdateAsync(Report report)
        {
            Update(report);
            await SaveChangesAsync();
            return report;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var report = await GetByIdAsync(id);
            if (report == null)
                return false;

            await DeleteAsync(report);
            await SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Report>> GetPendingReportsAsync()
        {
            return await _reports
                .Where(r => r.Status == "Pending")
                .Include(r => r.Reporter)
                .Include(r => r.ReportedListing)
                .ToListAsync();
        }

        public override async Task<Report> GetByNameAsync(string name)
        {
            // في حالة التقارير، يمكننا البحث عن طريق سبب التقرير
            return await _reports
                .Include(r => r.Reporter)
                .Include(r => r.ReportedListing)
                .Include(r => r.Resolver)
                .FirstOrDefaultAsync(r => r.Reason.Contains(name));
        }
    }
}

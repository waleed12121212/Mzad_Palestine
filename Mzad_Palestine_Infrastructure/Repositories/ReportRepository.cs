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
                .Include(r => r.ReportedAuction)
                .ToListAsync();
        }

        public override async Task<Report> GetByIdAsync(int id)
        {
            return await _reports
                .Include(r => r.Reporter)
                .Include(r => r.ReportedListing)
                .Include(r => r.ReportedAuction)
                .Include(r => r.Resolver)
                .FirstOrDefaultAsync(r => r.ReportId == id);
        }

        public async Task<Report> CreateAsync(Report report)
        {
            try
            {
                // تعيين القيم الافتراضية
                report.CreatedAt = DateTime.UtcNow;
                report.Status = "Pending";
                
                // إضافة التقرير
                await _reports.AddAsync(report);
                
                // حفظ التغييرات
                await _context.SaveChangesAsync();
                
                // إعادة تحميل التقرير مع العلاقات
                return await _reports
                    .Include(r => r.Reporter)
                    .Include(r => r.ReportedListing)
                    .Include(r => r.ReportedAuction)
                    .FirstOrDefaultAsync(r => r.ReportId == report.ReportId);
            }
            catch (Exception ex)
            {
                throw new Exception($"فشل في إنشاء التقرير: {ex.Message}", ex);
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
                .Include(r => r.ReportedAuction)
                .ToListAsync();
        }

        public override async Task<Report> GetByNameAsync(string name)
        {
            // في حالة التقارير، يمكننا البحث عن طريق سبب التقرير
            return await _reports
                .Include(r => r.Reporter)
                .Include(r => r.ReportedListing)
                .Include(r => r.ReportedAuction)
                .Include(r => r.Resolver)
                .FirstOrDefaultAsync(r => r.Reason.Contains(name));
        }

        public async Task<IEnumerable<Report>> GetByReporterIdAsync(int reporterId)
        {
            return await _reports
                .Include(r => r.Reporter)
                .Include(r => r.ReportedListing)
                .Include(r => r.ReportedAuction)
                .Include(r => r.Resolver)
                .Where(r => r.ReporterId == reporterId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Report>> GetByStatusAsync(string status)
        {
            return await _reports
                .Include(r => r.Reporter)
                .Include(r => r.ReportedListing)
                .Include(r => r.ReportedAuction)
                .Include(r => r.Resolver)
                .Where(r => r.Status == status)
                .ToListAsync();
        }

        public void Update(Report report)
        {
            _context.Reports.Update(report);
        }

        public async Task DeleteAsync(Report report)
        {
            _context.Reports.Remove(report);
        }
    }
}

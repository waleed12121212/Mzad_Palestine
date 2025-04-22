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

        public ReportRepository(ApplicationDbContext context) : base(context)
        {
            _reports = context.Set<Report>();
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
            await AddAsync(report);
            await SaveChangesAsync();
            return report;
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

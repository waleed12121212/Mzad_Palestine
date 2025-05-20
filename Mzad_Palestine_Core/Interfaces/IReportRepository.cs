using Mzad_Palestine_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Interfaces
{
    public interface IReportRepository : IGenericRepository<Report>
    {
        Task<IEnumerable<Report>> GetPendingReportsAsync();
        Task<Report> CreateAsync(Report report);
        Task<Report> UpdateAsync(Report report);
        Task<bool> DeleteAsync(int id);
        Task<Report> GetByNameAsync(string name);
        Task<IEnumerable<Report>> GetByReporterIdAsync(int reporterId);
        Task<IEnumerable<Report>> GetByStatusAsync(string status);
        void Update(Report report);
        Task DeleteAsync(Report report);
    }
}

using Mzad_Palestine_Core.DTOs;
using Mzad_Palestine_Core.DTOs.Report;
using Mzad_Palestine_Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Interfaces.Services
{
    public interface IReportService
    {
        /// <summary>
        /// Gets all reports
        /// </summary>
        /// <returns>List of reports</returns>
        Task<IEnumerable<ReportDto>> GetAllAsync();

        /// <summary>
        /// Gets a report by its ID
        /// </summary>
        /// <param name="id">Report ID</param>
        /// <returns>Report if found, null otherwise</returns>
        Task<ReportDto> GetByIdAsync(int id);

        /// <summary>
        /// Creates a new report
        /// </summary>
        /// <param name="report">Report entity to create</param>
        /// <returns>Created report</returns>
        Task<ReportDto> CreateAsync(Report report);

        /// <summary>
        /// Updates an existing report
        /// </summary>
        /// <param name="id">Report ID</param>
        /// <param name="updateReportDto">Report update data</param>
        /// <returns>Updated report</returns>
        Task<ReportDto> UpdateAsync(int id, DTOs.Report.UpdateReportDto updateReportDto);

        /// <summary>
        /// Deletes a report
        /// </summary>
        /// <param name="id">Report ID</param>
        /// <returns>True if deleted, false if not found</returns>
        Task<bool> DeleteAsync(int id);

        Task<IEnumerable<Report>> GetByReporterIdAsync(int reporterId);
        Task<IEnumerable<Report>> GetByStatusAsync(string status);
    }
}

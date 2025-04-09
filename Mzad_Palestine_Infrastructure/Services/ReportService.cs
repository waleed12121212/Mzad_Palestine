using Mzad_Palestine_Core.DTO_s.Report;
using Mzad_Palestine_Core.Interfaces.Services;
using Mzad_Palestine_Core.Interfaces;
using Mzad_Palestine_Core.Models;
using Mzad_Palestine_Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Infrastructure.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _repository;
        public ReportService(IReportRepository repository) => _repository = repository;

        public async Task<ReportDto> CreateAsync(CreateReportDto dto)
        {
            var entity = new Report
            {
                ReportedListingId = dto.ReportedListingId,
                Reason = dto.Reason,
                Status = ReportStatus.Pending
            };
            
            await _repository.AddAsync(entity);
            
            return new ReportDto
            {
                Id = entity.ReportId,
                ReporterId = entity.ReporterId,
                ReportedListingId = entity.ReportedListingId,
                Reason = entity.Reason,
                Status = entity.Status.ToString(),
                ResolvedBy = entity.ResolvedBy
            };
        }

        public async Task<IEnumerable<ReportDto>> GetAllAsync()
        {
            var reports = await _repository.GetAllAsync();
            return reports.Select(r => new ReportDto
            {
                Id = r.ReportId,
                ReporterId = r.ReporterId,
                ReportedListingId = r.ReportedListingId,
                Reason = r.Reason,
                Status = r.Status.ToString(),
                ResolvedBy = r.ResolvedBy
            });
        }
    }
}

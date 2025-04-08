using Mzad_Palestine_Core.DTO_s.Report;
using Mzad_Palestine_Core.Interfaces.Services;
using Mzad_Palestine_Core.Interfaces;
using Mzad_Palestine_Core.Models;
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
            var entity = new Report { ReportedListingId = dto.ReportedListingId , Reason = dto.Reason , Status = "Pending" };
            var created = await _repository.AddAsync(entity);
            return new ReportDto(created.Id , created.ReporterId , created.ReportedListingId , created.Reason , created.Status , created.ResolvedBy);
        }
    }
}

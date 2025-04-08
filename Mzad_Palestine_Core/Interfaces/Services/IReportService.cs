using Mzad_Palestine_Core.DTO_s.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Interfaces.Services
{
    public interface IReportService
    {
        Task<ReportDto> CreateAsync(CreateReportDto dto);
        Task<IEnumerable<ReportDto>> GetAllAsync( );
    }
}

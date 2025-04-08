using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mzad_Palestine_Core.DTO_s.Report;
using Mzad_Palestine_Core.Interfaces.Services;

namespace Mzad_Palestine_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        public ReportController(IReportService reportService) => _reportService = reportService;

        [HttpPost]
        public async Task<IActionResult> CreateReport([FromBody] CreateReportDto dto)
        {
            var report = await _reportService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetAll) , new { id = report.Id } , report);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll( )
        {
            IEnumerable<ReportDto> reports = await _reportService.GetAllAsync();
            return Ok(reports);
        }
    }
}

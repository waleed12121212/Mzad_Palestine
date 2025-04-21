using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mzad_Palestine_Core.DTOs;
using Mzad_Palestine_Core.Interfaces.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mzad_Palestine_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        /// <summary>
        /// Gets all reports
        /// </summary>
        /// <returns>List of all reports</returns>
        /// <response code="200">Returns the list of reports</response>
        /// <response code="401">If the user is not authenticated</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ReportDto>), 200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<IEnumerable<ReportDto>>> GetAll()
        {
            var reports = await _reportService.GetAllAsync();
            return Ok(reports);
        }

        /// <summary>
        /// Gets a specific report by ID
        /// </summary>
        /// <param name="id">The ID of the report to retrieve</param>
        /// <returns>The requested report</returns>
        /// <response code="200">Returns the requested report</response>
        /// <response code="404">If the report is not found</response>
        /// <response code="401">If the user is not authenticated</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ReportDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<ReportDto>> GetById(int id)
        {
            var report = await _reportService.GetByIdAsync(id);
            if (report == null)
                return NotFound();

            return Ok(report);
        }

        /// <summary>
        /// Creates a new report
        /// </summary>
        /// <param name="createReportDto">The report data</param>
        /// <returns>The created report</returns>
        /// <response code="201">Returns the newly created report</response>
        /// <response code="400">If the report data is invalid</response>
        /// <response code="401">If the user is not authenticated</response>
        [HttpPost]
        [ProducesResponseType(typeof(ReportDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<ReportDto>> Create([FromBody] CreateReportDto createReportDto)
        {
            var report = await _reportService.CreateAsync(createReportDto);
            return CreatedAtAction(nameof(GetById), new { id = report.ReportId }, report);
        }

        /// <summary>
        /// Updates an existing report
        /// </summary>
        /// <param name="id">The ID of the report to update</param>
        /// <param name="updateReportDto">The updated report data</param>
        /// <returns>The updated report</returns>
        /// <response code="200">Returns the updated report</response>
        /// <response code="404">If the report is not found</response>
        /// <response code="400">If the report data is invalid</response>
        /// <response code="401">If the user is not authenticated</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ReportDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<ReportDto>> Update(int id, [FromBody] UpdateReportDto updateReportDto)
        {
            try
            {
                var report = await _reportService.UpdateAsync(id, updateReportDto);
                return Ok(report);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Deletes a specific report
        /// </summary>
        /// <param name="id">The ID of the report to delete</param>
        /// <returns>No content</returns>
        /// <response code="204">If the report was successfully deleted</response>
        /// <response code="404">If the report is not found</response>
        /// <response code="401">If the user is not authenticated</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _reportService.DeleteAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}

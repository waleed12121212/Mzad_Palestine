using AutoMapper;
using Microsoft.Extensions.Logging;
using Mzad_Palestine_Core.DTOs;
using Mzad_Palestine_Core.Models;
using Mzad_Palestine_Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mzad_Palestine_Infrastructure.Repositories;
using Mzad_Palestine_Core.Interfaces;

namespace Mzad_Palestine_Infrastructure.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ReportService> _logger;

        public ReportService(
            IReportRepository reportRepository,
            IMapper mapper,
            ILogger<ReportService> logger)
        {
            _reportRepository = reportRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<ReportDto>> GetAllAsync()
        {
            try
            {
                var reports = await _reportRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<ReportDto>>(reports);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all reports");
                throw;
            }
        }

        public async Task<ReportDto> GetByIdAsync(int id)
        {
            try
            {
                var report = await _reportRepository.GetByIdAsync(id);
                return _mapper.Map<ReportDto>(report);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting report with ID: {Id}", id);
                throw;
            }
        }

        public async Task<ReportDto> CreateAsync(Report report)
        {
            try
            {
                if (report == null)
                    throw new ArgumentNullException(nameof(report), "Report object cannot be null");

                if (string.IsNullOrWhiteSpace(report.Reason))
                    throw new ArgumentException("Reason is required", nameof(report));

                if (report.ReporterId <= 0)
                    throw new ArgumentException("Invalid ReporterId", nameof(report));

                if (report.ReportedListingId <= 0)
                    throw new ArgumentException("Invalid ReportedListingId", nameof(report));

                _logger.LogInformation(
                    "Creating new report. ReporterId: {ReporterId}, ReportedListingId: {ListingId}, Reason: {Reason}",
                    report.ReporterId,
                    report.ReportedListingId,
                    report.Reason);

                var createdReport = await _reportRepository.CreateAsync(report);
                if (createdReport == null)
                    throw new Exception("Failed to create report - repository returned null");

                var mappedReport = _mapper.Map<ReportDto>(createdReport);
                if (mappedReport == null)
                    throw new Exception("Failed to map created report to DTO");

                _logger.LogInformation(
                    "Successfully created report with ID: {ReportId}",
                    mappedReport.ReportId);

                return mappedReport;
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "Null argument provided when creating report");
                throw;
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Invalid argument provided when creating report");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Validation error while creating report: {Message}", ex.Message);
                throw;
            }
            catch (AutoMapper.AutoMapperMappingException ex)
            {
                _logger.LogError(ex, "Mapping error occurred while creating report");
                throw new Exception("Error mapping report data", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while creating report: {Message}", ex.Message);
                throw new Exception("An unexpected error occurred while creating the report", ex);
            }
        }

        public async Task<ReportDto> UpdateAsync(int id, UpdateReportDto updateReportDto)
        {
            try
            {
                var existingReport = await _reportRepository.GetByIdAsync(id);
                if (existingReport == null)
                    throw new KeyNotFoundException($"Report with ID {id} not found");

                existingReport.Reason = updateReportDto.Reason;
                if (updateReportDto.ResolvedBy.HasValue)
                {
                    existingReport.ResolvedBy = updateReportDto.ResolvedBy;
                    existingReport.Status = "Resolved";
                }

                var updatedReport = await _reportRepository.UpdateAsync(existingReport);
                return _mapper.Map<ReportDto>(updatedReport);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating report with ID: {Id}", id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                return await _reportRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting report with ID: {Id}", id);
                throw;
            }
        }
    }
}

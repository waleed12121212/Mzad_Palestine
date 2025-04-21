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

        public async Task<ReportDto> CreateAsync(CreateReportDto createReportDto)
        {
            try
            {
                var report = _mapper.Map<Report>(createReportDto);
                report.CreatedAt = DateTime.UtcNow;

                var createdReport = await _reportRepository.CreateAsync(report);
                return _mapper.Map<ReportDto>(createdReport);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new report");
                throw;
            }
        }

        public async Task<ReportDto> UpdateAsync(int id, UpdateReportDto updateReportDto)
        {
            try
            {
                var existingReport = await _reportRepository.GetByIdAsync(id);
                if (existingReport == null)
                    throw new KeyNotFoundException($"Report with ID {id} not found");

                _mapper.Map(updateReportDto, existingReport);
                if (updateReportDto.ResolvedBy.HasValue)
                {
                    existingReport.ResolvedBy = updateReportDto.ResolvedBy;
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

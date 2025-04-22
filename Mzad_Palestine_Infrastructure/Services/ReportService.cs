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
                    throw new ArgumentNullException(nameof(report), "كائن التقرير لا يمكن أن يكون فارغًا");

                if (string.IsNullOrWhiteSpace(report.Reason))
                    throw new ArgumentException("سبب البلاغ مطلوب", nameof(report));

                if (report.ReporterId <= 0)
                    throw new ArgumentException("معرف المبلغ غير صالح", nameof(report));

                if (report.ReportedListingId <= 0)
                    throw new ArgumentException("معرف الإعلان المبلغ عنه غير صالح", nameof(report));

                _logger.LogInformation(
                    "إنشاء تقرير جديد. معرف المبلغ: {ReporterId}، معرف الإعلان: {ListingId}، السبب: {Reason}",
                    report.ReporterId,
                    report.ReportedListingId,
                    report.Reason);

                report.StatusId = 0; // حالة "قيد الانتظار"
                report.CreatedAt = DateTime.UtcNow;

                var createdReport = await _reportRepository.CreateAsync(report);
                
                if (createdReport == null)
                    throw new Exception("فشل إنشاء التقرير - أعاد المستودع قيمة فارغة");

                var mappedReport = _mapper.Map<ReportDto>(createdReport);
                
                _logger.LogInformation(
                    "تم إنشاء التقرير بنجاح بمعرف: {ReportId}",
                    mappedReport.ReportId);

                return mappedReport;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "حدث خطأ غير متوقع أثناء إنشاء التقرير: {Message}", ex.Message);
                throw new Exception("حدث خطأ غير متوقع أثناء إنشاء التقرير", ex);
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
                    existingReport.StatusId = 1; // "Resolved" status (1)
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

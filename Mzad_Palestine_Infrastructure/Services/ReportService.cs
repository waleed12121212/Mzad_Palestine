using AutoMapper;
using Microsoft.Extensions.Logging;
using Mzad_Palestine_Core.DTOs;
using Mzad_Palestine_Core.DTOs.Report;
using Mzad_Palestine_Core.Models;
using Mzad_Palestine_Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mzad_Palestine_Infrastructure.Repositories;
using Mzad_Palestine_Core.Interfaces;
using Mzad_Palestine_Core.Interfaces.Common;

namespace Mzad_Palestine_Infrastructure.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ReportService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public ReportService(
            IReportRepository reportRepository,
            IMapper mapper,
            ILogger<ReportService> logger,
            IUnitOfWork unitOfWork)
        {
            _reportRepository = reportRepository;
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
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

                if (report.ReportType == "Listing" && (!report.ReportedListingId.HasValue || report.ReportedListingId <= 0))
                    throw new ArgumentException("معرف الإعلان المبلغ عنه غير صالح", nameof(report));

                if (report.ReportType == "Auction" && (!report.ReportedAuctionId.HasValue || report.ReportedAuctionId <= 0))
                    throw new ArgumentException("معرف المزاد المبلغ عنه غير صالح", nameof(report));

                report.Resolution = "قيد المراجعة";

                _logger.LogInformation(
                    "إنشاء تقرير جديد. معرف المبلغ: {ReporterId}، نوع التقرير: {ReportType}، السبب: {Reason}",
                    report.ReporterId,
                    report.ReportType,
                    report.Reason);

                report.Status = "Pending";
                report.CreatedAt = DateTime.UtcNow;

                await _unitOfWork.Reports.AddAsync(report);
                await _unitOfWork.CompleteAsync();

                var mappedReport = _mapper.Map<ReportDto>(report);
                
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

        public async Task<ReportDto> UpdateAsync(int id, Mzad_Palestine_Core.DTOs.Report.UpdateReportDto updateReportDto)
        {
            try
            {
                var existingReport = await _reportRepository.GetByIdAsync(id);
                if (existingReport == null)
                    throw new KeyNotFoundException($"Report with ID {id} not found");

                existingReport.Reason = updateReportDto.Reason;
                existingReport.Status = updateReportDto.Status;
                existingReport.Resolution = updateReportDto.Resolution;
                existingReport.ResolvedBy = updateReportDto.ResolvedBy;
                existingReport.ResolvedAt = DateTime.UtcNow;
                existingReport.UpdatedAt = DateTime.UtcNow;

                _reportRepository.Update(existingReport);
                await _unitOfWork.CompleteAsync();
                return _mapper.Map<ReportDto>(existingReport);
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
                var report = await _reportRepository.GetByIdAsync(id);
                if (report == null)
                    return false;

                await _reportRepository.DeleteAsync(report);
                await _unitOfWork.CompleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting report with ID: {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<Report>> GetByReporterIdAsync(int reporterId)
        {
            return await _reportRepository.GetByReporterIdAsync(reporterId);
        }

        public async Task<IEnumerable<Report>> GetByStatusAsync(string status)
        {
            return await _reportRepository.GetByStatusAsync(status);
        }
    }
}

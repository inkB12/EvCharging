using EVCharging.DAL.Entities;
using EVCharging.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCharging.DAL.Services
{
    public class AdminFaultReportRepository : IAdminFaultReportRepository
    {
        private readonly EvchargingContext _context;

        public AdminFaultReportRepository(EvchargingContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Lấy tất cả FaultReport (cả open & closed),
        /// Include luôn User, Point, Station cho UI admin dùng.
        /// </summary>
        public async Task<List<FaultReport>> GetAllAsync()
        {
            return await _context.FaultReports
                .Include(f => f.User)
                .Include(f => f.Point)
                    .ThenInclude(p => p.Station)
                .OrderByDescending(f => f.ReportTime)
                .ToListAsync();
        }

        public async Task<FaultReport?> GetByIdAsync(int id)
        {
            return await _context.FaultReports
                .Include(f => f.User)
                .Include(f => f.Point)
                    .ThenInclude(p => p.Station)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task AddAsync(FaultReport report)
        {
            // Chuẩn hóa status và severity trước khi lưu
            report.Status = NormalizeStatus(report.Status);
            report.Severity = NormalizeSeverity(report.Severity);

            // ReportTime để DB tự fill bằng DEFAULT(SYSDATETIME())
            // nếu bạn muốn override thì giữ nguyên report.ReportTime hiện tại.

            _context.FaultReports.Add(report);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(FaultReport report)
        {
            var existing = await _context.FaultReports
                .FirstOrDefaultAsync(f => f.Id == report.Id);

            if (existing == null)
            {
                return; // hoặc throw exception tùy convention
            }

            // Giữ UserId nếu bạn không muốn cho đổi người báo
            existing.Title = report.Title;
            existing.Description = report.Description;
            existing.Severity = NormalizeSeverity(report.Severity);
            existing.Status = NormalizeStatus(report.Status);
            existing.PointId = report.PointId;
            // existing.ReportTime: thường không đổi

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// XÓA MỀM: chỉ chuyển trạng thái report sang 'closed'.
        /// Không xóa record khỏi DB để giữ lịch sử.
        /// </summary>
        public async Task SoftDeleteAsync(int id)
        {
            var report = await _context.FaultReports
                .FirstOrDefaultAsync(f => f.Id == id);

            if (report == null)
                return;

            report.Status = "closed"; // soft delete
            await _context.SaveChangesAsync();
        }

        public async Task<List<FaultReport>> GetByStatusAsync(string status)
        {
            status = NormalizeStatus(status);

            return await _context.FaultReports
                .Where(f => f.Status == status)
                .Include(f => f.User)
                .Include(f => f.Point)
                    .ThenInclude(p => p.Station)
                .OrderByDescending(f => f.ReportTime)
                .ToListAsync();
        }

        public async Task<List<FaultReport>> GetOpenReportsAsync()
        {
            return await _context.FaultReports
                .Where(f => f.Status == "open")
                .Include(f => f.User)
                .Include(f => f.Point)
                    .ThenInclude(p => p.Station)
                .OrderByDescending(f => f.ReportTime)
                .ToListAsync();
        }

        /// <summary>
        /// Lấy các report theo độ nghiêm trọng (severity).
        /// minSeverity: tối thiểu, maxSeverity: tối đa (nếu null thì = minSeverity..5).
        /// </summary>
        public async Task<List<FaultReport>> GetBySeverityRangeAsync(byte minSeverity, byte? maxSeverity = null)
        {
            if (minSeverity < 1) minSeverity = 1;
            if (!maxSeverity.HasValue || maxSeverity.Value < minSeverity)
                maxSeverity = 5; // default: 1..5

            return await _context.FaultReports
                .Where(f => f.Severity >= minSeverity && f.Severity <= maxSeverity.Value)
                .Include(f => f.User)
                .Include(f => f.Point)
                    .ThenInclude(p => p.Station)
                .OrderByDescending(f => f.Severity)
                .ThenByDescending(f => f.ReportTime)
                .ToListAsync();
        }

        public async Task<List<FaultReport>> GetByPointAsync(int pointId)
        {
            return await _context.FaultReports
                .Where(f => f.PointId == pointId)
                .Include(f => f.User)
                .Include(f => f.Point)
                    .ThenInclude(p => p.Station)
                .OrderByDescending(f => f.ReportTime)
                .ToListAsync();
        }

        public async Task<List<FaultReport>> GetByStationAsync(int stationId)
        {
            return await _context.FaultReports
                .Include(f => f.User)
                .Include(f => f.Point)
                    .ThenInclude(p => p.Station)
                .Where(f => f.Point.StationId == stationId)
                .OrderByDescending(f => f.ReportTime)
                .ToListAsync();
        }

        public async Task<bool> UpdateStatusAsync(int id, string status)
        {
            var report = await _context.FaultReports
                .FirstOrDefaultAsync(f => f.Id == id);

            if (report == null)
                return false;

            report.Status = NormalizeStatus(status);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Status chỉ được 'open' hoặc 'closed' theo CHECK constraint.
        /// </summary>
        private string NormalizeStatus(string? status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return "open";

            status = status.Trim().ToLower();

            return status switch
            {
                "closed" => "closed",
                // map một số alias nếu có
                "done" => "closed",
                "resolved" => "closed",
                _ => "open"
            };
        }

        /// <summary>
        /// Severity trong DB là tinyint, ở đây chuẩn hóa về 1..5.
        /// </summary>
        private byte NormalizeSeverity(byte severity)
        {
            if (severity < 1) return 1;
            if (severity > 5) return 5;
            return severity;
        }
    }
}

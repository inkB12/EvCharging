using EVCharging.BLL.AdminDTOs;
using EVCharging.BLL.Interfaces;
using EVCharging.DAL.Entities;
using EVCharging.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCharging.BLL.Services
{
    public class AdminFaultReportService : IAdminFaultReportService
    {
        private readonly IAdminFaultReportRepository _repository;

        public AdminFaultReportService(IAdminFaultReportRepository repository)
        {
            _repository = repository;
        }

        // ========================= CRUD =========================

        public async Task<List<AdminFaultReportDTO>> GetAllAsync()
        {
            var list = await _repository.GetAllAsync();
            return list.Select(MapToDTO).ToList();
        }

        public async Task<AdminFaultReportDTO?> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity == null ? null : MapToDTO(entity);
        }

        public async Task AddAsync(AdminFaultReportDTO dto)
        {
            var entity = MapToEntity(dto);
            await _repository.AddAsync(entity);
        }

        public async Task UpdateAsync(AdminFaultReportDTO dto)
        {
            var entity = MapToEntity(dto);
            await _repository.UpdateAsync(entity);
        }

        public async Task SoftDeleteAsync(int id)
        {
            await _repository.SoftDeleteAsync(id);
        }

        // ========================= FILTERS / OPERATIONS =========================

        public async Task<List<AdminFaultReportDTO>> GetByStatusAsync(string status)
        {
            var list = await _repository.GetByStatusAsync(status);
            return list.Select(MapToDTO).ToList();
        }

        public async Task<List<AdminFaultReportDTO>> GetOpenReportsAsync()
        {
            var list = await _repository.GetOpenReportsAsync();
            return list.Select(MapToDTO).ToList();
        }

        public async Task<List<AdminFaultReportDTO>> GetBySeverityRangeAsync(byte minSeverity, byte? maxSeverity = null)
        {
            var list = await _repository.GetBySeverityRangeAsync(minSeverity, maxSeverity);
            return list.Select(MapToDTO).ToList();
        }

        public async Task<List<AdminFaultReportDTO>> GetByPointAsync(int pointId)
        {
            var list = await _repository.GetByPointAsync(pointId);
            return list.Select(MapToDTO).ToList();
        }

        public async Task<List<AdminFaultReportDTO>> GetByStationAsync(int stationId)
        {
            var list = await _repository.GetByStationAsync(stationId);
            return list.Select(MapToDTO).ToList();
        }

        public async Task<bool> UpdateStatusAsync(int id, string status)
        {
            return await _repository.UpdateStatusAsync(id, status);
        }

        // ========================= MAPPERS =========================

        private AdminFaultReportDTO MapToDTO(FaultReport e)
        {
            return new AdminFaultReportDTO
            {
                Id = e.Id,
                UserId = e.UserId,
                UserName = e.User?.FullName,
                UserEmail = e.User?.Email,
                PointId = e.PointId,
                PointPortType = e.Point?.PortType,
                StationId = e.Point?.StationId,
                StationName = e.Point?.Station?.Name,
                ReportTime = e.ReportTime,
                Title = e.Title,
                Description = e.Description,
                Severity = e.Severity,
                Status = e.Status
            };
        }

        private FaultReport MapToEntity(AdminFaultReportDTO d)
        {
            return new FaultReport
            {
                Id = d.Id,
                UserId = d.UserId,
                PointId = d.PointId,
                Title = d.Title,
                Description = d.Description,
                ReportTime = d.ReportTime == default ? System.DateTime.Now : d.ReportTime,
                Severity = d.Severity,
                Status = string.IsNullOrWhiteSpace(d.Status) ? "open" : d.Status
            };
        }
    }
}

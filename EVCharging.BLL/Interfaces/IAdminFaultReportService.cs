using EVCharging.BLL.AdminDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCharging.BLL.Interfaces
{
    public interface IAdminFaultReportService
    {
        Task<List<AdminFaultReportDTO>> GetAllAsync();
        Task<AdminFaultReportDTO?> GetByIdAsync(int id);
        Task AddAsync(AdminFaultReportDTO dto);
        Task UpdateAsync(AdminFaultReportDTO dto);
        Task SoftDeleteAsync(int id);

        // Các hàm nâng cao cho dashboard / thống kê
        Task<List<AdminFaultReportDTO>> GetByStatusAsync(string status);
        Task<List<AdminFaultReportDTO>> GetOpenReportsAsync();
        Task<List<AdminFaultReportDTO>> GetBySeverityRangeAsync(byte minSeverity, byte? maxSeverity = null);
        Task<List<AdminFaultReportDTO>> GetByPointAsync(int pointId);
        Task<List<AdminFaultReportDTO>> GetByStationAsync(int stationId);

        // Đóng / mở report
        Task<bool> UpdateStatusAsync(int id, string status);
    }
}

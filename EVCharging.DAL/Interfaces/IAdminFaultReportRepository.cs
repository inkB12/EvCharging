using EVCharging.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCharging.DAL.Interfaces
{
    public interface IAdminFaultReportRepository
    {

        // CRUD cơ bản
        Task<List<FaultReport>> GetAllAsync();
        Task<FaultReport?> GetByIdAsync(int id);
        Task AddAsync(FaultReport report);
        Task UpdateAsync(FaultReport report);

        // XÓA MỀM: chỉ chuyển trạng thái => closed
        Task SoftDeleteAsync(int id);

        // Các hàm hỗ trợ quản lý / lọc
        Task<List<FaultReport>> GetByStatusAsync(string status); // open/closed
        Task<List<FaultReport>> GetOpenReportsAsync();           // chỉ report đang open
        Task<List<FaultReport>> GetBySeverityRangeAsync(byte minSeverity, byte? maxSeverity = null);
        Task<List<FaultReport>> GetByPointAsync(int pointId);    // lỗi theo Point
        Task<List<FaultReport>> GetByStationAsync(int stationId);// lỗi theo Station

        // Cập nhật trạng thái open/closed
        Task<bool> UpdateStatusAsync(int id, string status);
    }
}

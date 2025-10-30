using EVCharging.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCharging.DAL.Interfaces
{
    public interface IReportRepository
    {
        // Báo cáo doanh thu
        Task<decimal> GetTotalRevenueAsync(DateTime? start = null, DateTime? end = null);

        // Thống kê sử dụng trạm sạc
        Task<int> CountTotalSessionsAsync(DateTime? start = null, DateTime? end = null);
        Task<Dictionary<string, int>> GetUsageByStationAsync(DateTime? start = null, DateTime? end = null);

        // Lấy danh sách giao dịch
        Task<List<Transaction>> GetTransactionsAsync(DateTime? start = null, DateTime? end = null);
    }
}

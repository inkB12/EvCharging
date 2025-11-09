using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCharging.DAL.Interfaces
{
    public interface IAdminDashboardRepository
    {
        // Tổng doanh thu trong 1 khoảng thời gian
        Task<decimal> GetTotalRevenueAsync(DateTime start, DateTime end);

        // Doanh thu nhóm theo tháng trong 1 năm
        Task<List<(int Month, decimal Revenue)>> GetMonthlyRevenueAsync(int year);

        // Doanh thu nhóm theo năm (năm gần đây)
        Task<List<(int Year, decimal Revenue)>> GetYearlyRevenueAsync(int recentYearsCount = 5);

        // Đếm giao dịch theo trạng thái
        Task<Dictionary<string, int>> CountTransactionsByStatusAsync();

        // Top 5 trạm sạc doanh thu cao nhất
        Task<List<(string StationName, decimal Revenue)>> GetTopStationsAsync(int top = 5);

        // Top 5 người dùng chi tiêu nhiều nhất
        Task<List<(string UserName, decimal TotalSpent)>> GetTopUsersAsync(int top = 5);
    }
}

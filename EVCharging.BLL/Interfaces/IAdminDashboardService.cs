using EVCharging.BLL.AdminDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCharging.BLL.Interfaces
{
    public interface IAdminDashboardService
    {
        // Lấy dữ liệu tổng hợp cho dashboard
        Task<AdminDashboardDTO> GetDashboardSummaryAsync(int year);

        // Tách riêng từng phần nếu muốn gọi riêng lẻ
        Task<List<MonthlyRevenueDTO>> GetMonthlyRevenueAsync(int year);
        Task<List<YearlyRevenueDTO>> GetYearlyRevenueAsync(int recentYearsCount = 5);
        Task<List<TransactionStatusCountDTO>> GetTransactionStatusCountAsync();
        Task<List<TopStationDTO>> GetTopStationsAsync(int top = 5);
        Task<List<TopUserDTO>> GetTopUsersAsync(int top = 5);
    }
}

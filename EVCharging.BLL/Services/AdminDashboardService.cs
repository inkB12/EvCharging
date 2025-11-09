using EVCharging.BLL.AdminDTOs;
using EVCharging.BLL.Interfaces;
using EVCharging.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCharging.BLL.Services
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly IAdminDashboardRepository _repository;

        public AdminDashboardService(IAdminDashboardRepository repository)
        {
            _repository = repository;
        }

        // =================== TỔNG HỢP DASHBOARD ===================
        public async Task<AdminDashboardDTO> GetDashboardSummaryAsync(int year)
        {
            var now = DateTime.Now;
            var startMonth = new DateTime(year, now.Month, 1);
            var endMonth = startMonth.AddMonths(1).AddDays(-1);

            var dashboard = new AdminDashboardDTO
            {
                TotalRevenueThisMonth = await _repository.GetTotalRevenueAsync(startMonth, endMonth),
                TotalRevenueThisYear = await _repository.GetTotalRevenueAsync(
                    new DateTime(year, 1, 1),
                    new DateTime(year, 12, 31)
                ),
                MonthlyRevenues = (await _repository.GetMonthlyRevenueAsync(year))
                    .Select(m => new MonthlyRevenueDTO { Month = m.Month, Revenue = m.Revenue })
                    .ToList(),

                YearlyRevenues = (await _repository.GetYearlyRevenueAsync())
                    .Select(y => new YearlyRevenueDTO { Year = y.Year, Revenue = y.Revenue })
                    .ToList(),

                TransactionStatusCounts = (await _repository.CountTransactionsByStatusAsync())
                    .Select(kv => new TransactionStatusCountDTO { Status = kv.Key, Count = kv.Value })
                    .ToList(),

                TopStations = (await _repository.GetTopStationsAsync())
                    .Select(s => new TopStationDTO { StationName = s.StationName, Revenue = s.Revenue })
                    .ToList(),

                TopUsers = (await _repository.GetTopUsersAsync())
                    .Select(u => new TopUserDTO { UserName = u.UserName, TotalSpent = u.TotalSpent })
                    .ToList()
            };

            return dashboard;
        }

        // =================== HÀM CON ===================

        public async Task<List<MonthlyRevenueDTO>> GetMonthlyRevenueAsync(int year)
        {
            var data = await _repository.GetMonthlyRevenueAsync(year);
            return data.Select(x => new MonthlyRevenueDTO
            {
                Month = x.Month,
                Revenue = x.Revenue
            }).ToList();
        }

        public async Task<List<YearlyRevenueDTO>> GetYearlyRevenueAsync(int recentYearsCount = 5)
        {
            var data = await _repository.GetYearlyRevenueAsync(recentYearsCount);
            return data.Select(x => new YearlyRevenueDTO
            {
                Year = x.Year,
                Revenue = x.Revenue
            }).ToList();
        }

        public async Task<List<TransactionStatusCountDTO>> GetTransactionStatusCountAsync()
        {
            var data = await _repository.CountTransactionsByStatusAsync();
            return data.Select(kv => new TransactionStatusCountDTO
            {
                Status = kv.Key,
                Count = kv.Value
            }).ToList();
        }

        public async Task<List<TopStationDTO>> GetTopStationsAsync(int top = 5)
        {
            var data = await _repository.GetTopStationsAsync(top);
            return data.Select(x => new TopStationDTO
            {
                StationName = x.StationName,
                Revenue = x.Revenue
            }).ToList();
        }

        public async Task<List<TopUserDTO>> GetTopUsersAsync(int top = 5)
        {
            var data = await _repository.GetTopUsersAsync(top);
            return data.Select(x => new TopUserDTO
            {
                UserName = x.UserName,
                TotalSpent = x.TotalSpent
            }).ToList();
        }
    }
}

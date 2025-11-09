using EVCharging.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCharging.DAL.Services
{
    public class AdminDashboardRepository : IAdminDashboardRepository
    {
        private readonly EvchargingContext _context;

        public AdminDashboardRepository(EvchargingContext context)
        {
            _context = context;
        }

        // ======================================================
        // 🔹 Tổng doanh thu trong khoảng thời gian
        // ======================================================
        public async Task<decimal> GetTotalRevenueAsync(DateTime start, DateTime end)
        {
            return await _context.Transactions
                .Where(t => t.Status == "success" && t.Datetime >= start && t.Datetime <= end)
                .SumAsync(t => (decimal?)t.Total) ?? 0;
        }

        // ======================================================
        // 🔹 Doanh thu theo tháng (đủ 12 tháng)
        // ======================================================
        public async Task<List<(int Month, decimal Revenue)>> GetMonthlyRevenueAsync(int year)
        {
            // Lấy dữ liệu thật có trong DB
            var data = await _context.Transactions
                .Where(t => t.Status == "success" && t.Datetime.Year == year)
                .GroupBy(t => t.Datetime.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    Revenue = g.Sum(x => x.Total)
                })
                .ToListAsync();

            // Đảm bảo có đủ 12 tháng (tháng nào không có dữ liệu -> 0)
            var fullYear = Enumerable.Range(1, 12)
                .Select(m => new
                {
                    Month = m,
                    Revenue = data.FirstOrDefault(x => x.Month == m)?.Revenue ?? 0
                })
                .OrderBy(x => x.Month)
                .Select(x => (x.Month, x.Revenue))
                .ToList();

            return fullYear;
        }

        // ======================================================
        // 🔹 Doanh thu theo năm (gần X năm trở lại)
        // ======================================================
        public async Task<List<(int Year, decimal Revenue)>> GetYearlyRevenueAsync(int recentYearsCount = 5)
        {
            var currentYear = DateTime.Now.Year;
            var minYear = currentYear - recentYearsCount + 1;

            var data = await _context.Transactions
                .Where(t => t.Status == "success" && t.Datetime.Year >= minYear)
                .GroupBy(t => t.Datetime.Year)
                .Select(g => new
                {
                    Year = g.Key,
                    Revenue = g.Sum(x => x.Total)
                })
                .OrderBy(x => x.Year)
                .ToListAsync();

            // Fill các năm không có giao dịch
            var fullRange = Enumerable.Range(minYear, recentYearsCount)
                .Select(y => new
                {
                    Year = y,
                    Revenue = data.FirstOrDefault(x => x.Year == y)?.Revenue ?? 0
                })
                .Select(x => (x.Year, x.Revenue))
                .ToList();

            return fullRange;
        }

        // ======================================================
        // 🔹 Đếm giao dịch theo trạng thái (success, pending, fail...)
        // ======================================================
        public async Task<Dictionary<string, int>> CountTransactionsByStatusAsync()
        {
            return await _context.Transactions
                .GroupBy(t => t.Status)
                .Select(g => new { g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Key, x => x.Count);
        }

        // ======================================================
        // 🔹 Top 5 trạm sạc có doanh thu cao nhất
        // ======================================================
        public async Task<List<(string StationName, decimal Revenue)>> GetTopStationsAsync(int top = 5)
        {
            // Chỉ lấy transaction thành công và user có HomeStation
            var data = await _context.Transactions
                .Where(t => t.Status == "success"
                            && t.Booking.User.HomeStation != null)
                .GroupBy(t => t.Booking.User.HomeStation!.Name)
                .Select(g => new
                {
                    StationName = g.Key,
                    Revenue = g.Sum(x => x.Total)
                })
                .OrderByDescending(x => x.Revenue)
                .Take(top)
                .ToListAsync();   // <-- nhớ dùng ToListAsync (async)

            // Map sang dạng tuple như interface yêu cầu
            return data
                .Select(x => (x.StationName, x.Revenue))
                .ToList();
        }

        // ======================================================
        // 🔹 Top 5 người dùng chi tiêu nhiều nhất
        // ======================================================
        public async Task<List<(string UserName, decimal TotalSpent)>> GetTopUsersAsync(int top = 5)
        {
            var transactions = await _context.Transactions
                .Where(t => t.Status == "success")
                .Include(t => t.Booking)
                    .ThenInclude(b => b.User)
                .ToListAsync();

            var grouped = transactions
                .GroupBy(t => t.Booking.User.FullName ?? "Không xác định")
                .Select(g => new
                {
                    UserName = g.Key,
                    TotalSpent = g.Sum(x => x.Total)
                })
                .OrderByDescending(x => x.TotalSpent)
                .Take(top)
                .ToList();

            return grouped.Select(x => (x.UserName, x.TotalSpent)).ToList();
        }
    }
}

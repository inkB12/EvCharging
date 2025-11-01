using EVCharging.BLL.DTO;
using EVCharging.BLL.Interfaces;
using EVCharging.DAL.Interfaces;

namespace EVCharging.BLL.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _repo;
        private readonly ITransactionRepository _transactionRepo;

        public ReportService(IReportRepository repo, ITransactionRepository transactionRepo)
        {
            _repo = repo;
            _transactionRepo = transactionRepo;
        }

        public async Task<ReportDto> GetSystemReportAsync(DateTime? start = null, DateTime? end = null)
        {
            var totalRevenue = await _repo.GetTotalRevenueAsync(start, end);
            var totalSessions = await _repo.CountTotalSessionsAsync(start, end);
            var usageByStation = await _repo.GetUsageByStationAsync(start, end);

            return new ReportDto
            {
                TotalRevenue = totalRevenue,
                TotalSessions = totalSessions,
                UsageByStation = usageByStation
            };
        }

        public async Task<List<MonthlyCostReportDTO>> GetMonthlyCostReportByYear(int userId, int year)
        {
            var transactions = await _transactionRepo.GetByUserAndYearAsync(userId, year);

            return [.. transactions
                .GroupBy(t => new { t.Datetime.Year, t.Datetime.Month })
                .Select(g => new MonthlyCostReportDTO
                {
                    MonthYear = $"{g.Key.Month}/{g.Key.Year}",
                    TotalSessions = g.Count(),
                    TotalCost = g.Sum(x => x.Total),
                    TotalEnergyKWh = g.Sum(x => x.Booking.ChargingSessions.Sum(c => c.EnergyConsumedKwh) ?? 0)
                })
                .OrderByDescending(m => m.MonthYear)];
        }
    }
}

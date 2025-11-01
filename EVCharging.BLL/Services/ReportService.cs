using EVCharging.BLL.DTO;
using EVCharging.BLL.Interfaces;
using EVCharging.DAL.Interfaces;

namespace EVCharging.BLL.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _repo;
        private readonly ITransactionRepository _transactionRepo;
        private readonly IChargingSessionRepository _chargingSessionRepo;

        public ReportService(IReportRepository repo, ITransactionRepository transactionRepo, IChargingSessionRepository chargingSessionRepo)
        {
            _repo = repo;
            _transactionRepo = transactionRepo;
            _chargingSessionRepo = chargingSessionRepo;
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

        public async Task<YearlyCostOverviewDTO> GetMonthlyCostReportByYear(int userId, int year)
        {
            var transactions = await _transactionRepo.GetByUserAndYearAsync(userId, year);

            List<MonthlyCostReportDTO> monthlyReports =
                [.. transactions
                .GroupBy(t => new { t.Datetime.Year, t.Datetime.Month })
                .Select(g => new MonthlyCostReportDTO
                {
                    MonthYear = $"{g.Key.Month}/{g.Key.Year}",
                    TotalSessions = g.Count(),
                    TotalCost = g.Sum(x => x.Total),
                    TotalEnergyKWh = g.Sum(x => x.Booking.ChargingSessions.Sum(c => c.EnergyConsumedKwh) ?? 0)
                })
                .OrderByDescending(m => m.MonthYear)];

            return new YearlyCostOverviewDTO()
            {
                TotalCostYear = monthlyReports.Sum(m => m.TotalCost),
                TotalEnergyKWhYear = monthlyReports.Sum(m => m.TotalEnergyKWh) ?? 0,
                TotalSessionsYear = monthlyReports.Sum(m => m.TotalSessions),
                MonthlyData = monthlyReports
            };
        }

        public async Task<List<ChargingLocationHabitDTO>> GetChargingLocationHabits(int userId, int year)
        {
            var sessions = await _chargingSessionRepo.GetAllByUserAndYear(userId, year);

            return [.. sessions
                .GroupBy(s => new { s.Point.Station.Id, s.Point.Station.Name, s.Point.Station.Location })
                .Select(g => new ChargingLocationHabitDTO()
                {
                    Location = g.Key.Location ?? "",
                    StationName = g.Key.Name,
                    SessionCount = g.Count()
                })
                .OrderByDescending(h => h.SessionCount)
                .Take(5)];
        }

        public async Task<List<ChargingTimeHabitDTO>> GetChargingTimeHabits(int userId, int year)
        {
            var sessions = await _chargingSessionRepo.GetAllByUserAndYear(userId, year);

            return [.. sessions
                .GroupBy(s => s.StartTime.Hour)
                .Select(g => new ChargingTimeHabitDTO()
                {
                    HourOfDay = g.Key,
                    TimeRangeLabel = $"{g.Key:00}:00 - {g.Key + 1:00}:00",
                    SessionCount = g.Count()
                })
                .OrderBy(t => t.HourOfDay)
                .Take(5)];
        }
    }
}

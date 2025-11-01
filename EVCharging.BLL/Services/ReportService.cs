using EVCharging.BLL.DTO;
using EVCharging.BLL.Interfaces;
using EVCharging.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCharging.BLL.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _repo;

        public ReportService(IReportRepository repo)
        {
            _repo = repo;
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
    }
}

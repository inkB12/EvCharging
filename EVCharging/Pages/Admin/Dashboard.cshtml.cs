using EVCharging.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVCharging.Pages.Admin
{
    public class DashboardModel : PageModel
    {
        private readonly IReportService _reportService;
        private readonly IChargingStationService _stationService;
        private readonly IChargingPointService _pointService;
        private readonly IAdminUserService _userService;

        public decimal TotalRevenue { get; set; }
        public int StationCount { get; set; }
        public int PointCount { get; set; }
        public int UserCount { get; set; }
        public Dictionary<string, int> UsageByStation { get; set; } = new();

        public DashboardModel(
            IReportService reportService,
            IChargingStationService stationService,
            IChargingPointService pointService,
            IAdminUserService userService)
        {
            _reportService = reportService;
            _stationService = stationService;
            _pointService = pointService;
            _userService = userService;
        }

        public async Task OnGet()
        {
            var report = await _reportService.GetSystemReportAsync();
            TotalRevenue = report.TotalRevenue;
            UsageByStation = report.UsageByStation ?? new();

            StationCount = (await _stationService.GetAllAsync()).Count;
            PointCount = (await _pointService.GetByStatusAsync("Online")).Count;
            UserCount = (await _userService.GetAllAsync()).Count;
        }
    }
}

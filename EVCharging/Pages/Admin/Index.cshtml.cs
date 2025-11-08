using EVCharging.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVCharging.Pages.Admin
{
    public class IndexModel : PageModel
    {
        private readonly IAdminChargingStationService _stationService;
        private readonly IAdminChargingPointService _pointService;
        private readonly IAdminUserService _userService;
        private readonly IAdminServicePlanService _planService;

        public int TotalStations { get; set; }
        public int TotalPoints { get; set; }
        public int TotalUsers { get; set; }
        public int TotalPlans { get; set; }

        public int OnlineStations { get; set; }
        public int OfflineStations { get; set; }
        public int MaintenanceStations { get; set; }

        public int DriverCount { get; set; }
        public int StaffCount { get; set; }
        public int AdminCount { get; set; }

        public IndexModel(
            IAdminChargingStationService stationService,
            IAdminChargingPointService pointService,
            IAdminUserService userService,
            IAdminServicePlanService planService)
        {
            _stationService = stationService;
            _pointService = pointService;
            _userService = userService;
            _planService = planService;
        }

        public async Task OnGet()
        {
            var stations = await _stationService.GetAllAsync();
            var users = await _userService.GetAllAsync();
            var plans = await _planService.GetAllAsync();
            var points = await _pointService.GetAllAsync();

            TotalStations = stations.Count;
            TotalPoints = points.Count;
            TotalUsers = users.Count;
            TotalPlans = plans.Count;

            OnlineStations = stations.Count(s => s.Status == "online");
            OfflineStations = stations.Count(s => s.Status == "offline");
            MaintenanceStations = stations.Count(s => s.Status == "maintenance");

            DriverCount = users.Count(u => u.Role == "Driver");
            StaffCount = users.Count(u => u.Role == "Staff");
            AdminCount = users.Count(u => u.Role == "Admin");
        }
    }
}

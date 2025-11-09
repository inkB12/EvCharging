using EVCharging.BLL.AdminDTOs;
using EVCharging.BLL.Interfaces;
using EVCharging.DAL;
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
        private readonly IAdminFaultReportService _faultService;
        private readonly IAdminChargingSessionService _sessionService;

        // Overview
        public int TotalStations { get; set; }
        public int TotalPoints { get; set; }
        public int TotalUsers { get; set; }
        public int TotalPlans { get; set; }

        // Charts
        public int OnlineStations { get; set; }
        public int OfflineStations { get; set; }
        public int MaintenanceStations { get; set; }

        public int DriverCount { get; set; }
        public int StaffCount { get; set; }
        public int AdminCount { get; set; }

        public int TotalFaultReports { get; set; }
        public int OpenFaultReports { get; set; }
        public int ClosedFaultReports { get; set; }

        public int TotalSessions { get; set; }
        public int OngoingSessions { get; set; }
        public int SuccessSessions { get; set; }
        public int ComingSoonSessions { get; set; }

        // Tables
        public List<AdminChargingStationDTO> TopStations { get; set; } = new();
        public List<AdminUserDTO> TopUsers { get; set; } = new();
        public List<AdminServicePlanDTO> TopPlans { get; set; } = new();

        public IndexModel(
            IAdminChargingStationService stationService,
            IAdminChargingPointService pointService,
            IAdminUserService userService,
            IAdminServicePlanService planService,
            IAdminFaultReportService faultService,
    IAdminChargingSessionService sessionService
           )
        {
            _stationService = stationService;
            _pointService = pointService;
            _userService = userService;
            _planService = planService;
            _faultService = faultService;
            _sessionService = sessionService;

        }

        public async Task OnGet()
        {
            // ========== LẤY DỮ LIỆU GỐC ==========
            var stations = await _stationService.GetAllAsync();
            var users = await _userService.GetAllAsync();
            var plans = await _planService.GetAllAsync();
            var points = await _pointService.GetAllAsync();
            var faults = await _faultService.GetAllAsync();
            var sessions = await _sessionService.GetAllAsync();

            // ========== OVERVIEW ==========
            TotalStations = stations.Count;
            TotalPoints = points.Count;
            TotalUsers = users.Count;
            TotalPlans = plans.Count;
            TotalFaultReports = faults.Count;
            OpenFaultReports = faults.Count(f => f.Status == "open");
            ClosedFaultReports = faults.Count(f => f.Status == "closed");

            TotalSessions = sessions.Count;
            OngoingSessions = sessions.Count(s => s.Status == "ongoing");
            SuccessSessions = sessions.Count(s => s.Status == "success");
            ComingSoonSessions = sessions.Count(s => s.Status == "coming-soon");

            // ========== BIỂU ĐỒ ==========
            OnlineStations = stations.Count(s => s.Status == "online");
            OfflineStations = stations.Count(s => s.Status == "offline");
            MaintenanceStations = stations.Count(s => s.Status == "maintenance");

            DriverCount = users.Count(u => u.Role?.ToLower() == "driver");
            StaffCount = users.Count(u => u.Role?.ToLower() == "staff");
            AdminCount = users.Count(u => u.Role?.ToLower() == "admin");

            // ========== TOP 5 ==========
            TopStations = stations
                .OrderByDescending(s => s.ChargingPoints?.Count ?? 0)
                .Take(5)
                .ToList();

            TopUsers = users
                .Where(u => !string.IsNullOrEmpty(u.ServicePlanName))
                .OrderBy(u => u.FullName)
                .Take(5)
                .ToList();

            TopPlans = plans
                .OrderByDescending(p => users.Count(u => u.ServicePlanId == p.Id))
                .Take(5)
                .ToList();
        }
    }
}

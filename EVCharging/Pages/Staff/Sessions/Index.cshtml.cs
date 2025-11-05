using EVCharging.BLL.DTO;
using EVCharging.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVCharging.Pages.Staff.Sessions
{
    public class IndexModel : PageModel
    {
        // Inject BLL Service
        private readonly IChargingSessionService _sessionService;

        public IndexModel(IChargingSessionService sessionService)
        {
            _sessionService = sessionService;
        }

        public List<ChargingSessionDto> Sessions { get; set; } = new List<ChargingSessionDto>();

        public async Task<IActionResult> OnGetAsync()
        {
            // 1. Kiểm tra quyền (phải là Staff)
            var userRole = HttpContext.Session.GetString("User.Role");
            if (userRole != "Staff")
            {
                TempData["Error"] = "Bạn không có quyền truy cập trang này.";
                return RedirectToPage("/Index");
            }

            // 2. Lấy StationId từ Session (dùng key "User.HomeStationId" như file Login)
            var stationId = HttpContext.Session.GetInt32("User.HomeStationId");
            if (!stationId.HasValue)
            {
                TempData["Message"] = "Tài khoản của bạn chưa được gán trạm.";
                return Page();
            }

            // 3. Gọi BLL Service
            Sessions = await _sessionService.GetSessionsByStationAsync(stationId.Value);

            return Page();
        }
    }
}

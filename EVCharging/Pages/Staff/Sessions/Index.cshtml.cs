using EVCharging.BLL.DTO;
using EVCharging.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        [BindProperty(SupportsGet = true)]
        public string? StatusFilter { get; set; }

        // THÊM MỚI: Dùng để hiển thị các lựa chọn trong Combo Box
        public SelectList StatusOptions { get; set; }
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

            var allSessions = await _sessionService.GetSessionsByStationAsync(stationId.Value);
            // 4. (THÊM MỚI) Tạo danh sách cho Combo Box
            // Lấy tất cả các trạng thái duy nhất từ danh sách
            var statuses = allSessions.Select(s => s.Status).Distinct().ToList();
            StatusOptions = new SelectList(statuses);

            // 5. (THÊM MỚI) Áp dụng bộ lọc nếu có
            if (!string.IsNullOrEmpty(StatusFilter))
            {
                Sessions = allSessions.Where(s => s.Status == StatusFilter).ToList();
            }
            else
            {
                Sessions = allSessions;
            }
            return Page();
        }
    }
}

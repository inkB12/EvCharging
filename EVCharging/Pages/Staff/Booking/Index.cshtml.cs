using EVCharging.BLL.DTO;
using EVCharging.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVCharging.Pages.Staff.Booking
{
    public class IndexModel : PageModel
    {
        private readonly IStaffBookingQueryService _svc;
        private readonly IUserService _userSvc;

        public IndexModel(IStaffBookingQueryService svc, IUserService userSvc)
        {
            _svc = svc;
            _userSvc = userSvc;
        }

        public List<BookingListItemDTO> Items { get; private set; } = new();

        // filter
        [BindProperty(SupportsGet = true)] public string? Q { get; set; }
        [BindProperty(SupportsGet = true)] public string? Status { get; set; }
        [BindProperty(SupportsGet = true)] public DateOnly? From { get; set; }
        [BindProperty(SupportsGet = true)] public DateOnly? To { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = HttpContext.Session.GetInt32("User.Id");
            if (userId is null) return RedirectToPage("/Auth/Login");

            // lấy HomeStationId của staff
            var user = await _userSvc.GetByIdAsync(userId.Value);
            if (user == null)
            {
                TempData["Error"] = "Không tìm thấy tài khoản.";
                return RedirectToPage("/Auth/Login");
            }

            // bạn đã mở rộng UserDTO đầy đủ field trước đó
            // nếu chưa, bạn có thể lấy trực tiếp từ Session ("User.HomeStationId")
            var stationId = HttpContext.Session.GetInt32("User.HomeStationId");
            if (stationId is null)
            {
                TempData["Error"] = "Bạn chưa được gán Home Station.";
            }

            // Dùng stationId.Value từ đây
            var all = await _svc.GetByStationAsync(stationId.Value);


            // Status
            if (!string.IsNullOrWhiteSpace(Status))
                all = all.Where(x => string.Equals(x.Status, Status, StringComparison.OrdinalIgnoreCase)).ToList();

            // Date range
            DateTime? fromUtc = null, toUtc = null;
            if (From.HasValue)
                fromUtc = new DateTime(From.Value.Year, From.Value.Month, From.Value.Day, 0, 0, 0, DateTimeKind.Local).ToUniversalTime();
            if (To.HasValue)
                toUtc = new DateTime(To.Value.Year, To.Value.Month, To.Value.Day, 23, 59, 59, DateTimeKind.Local).ToUniversalTime();

            if (fromUtc.HasValue) all = all.Where(x => x.StartTime >= fromUtc).ToList();
            if (toUtc.HasValue) all = all.Where(x => x.StartTime <= toUtc).ToList();

            // Search
            if (!string.IsNullOrWhiteSpace(Q))
            {
                var q = Q.Trim().ToLowerInvariant();
                all = all.Where(x =>
                    (x.StationName?.ToLower().Contains(q) ?? false) ||
                    (x.StationLocation?.ToLower().Contains(q) ?? false) ||
                    (x.PortType?.ToLower().Contains(q) ?? false)
                ).ToList();
            }

            Items = all.OrderByDescending(x => x.StartTime).ToList();
            return Page();
        }
    }
}

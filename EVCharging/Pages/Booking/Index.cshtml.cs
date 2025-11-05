using EVCharging.BLL.Interfaces;
using EVCharging.BLL.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVCharging.Pages.Bookings
{
    public class IndexModel : PageModel
    {
        private readonly IBookingQueryService _query;
        private readonly IBookingCommandService _cmd;   // NEW

        public IndexModel(IBookingQueryService query, IBookingCommandService cmd)
        {
            _query = query;
            _cmd = cmd;
        }

        public List<BookingListItemDTO> Items { get; private set; } = new();
        private static readonly string[] _validStatus = ["ongoing", "success", "cancelled"];

        // --- Filters ---
        [BindProperty(SupportsGet = true)] public string? Q { get; set; }
        [BindProperty(SupportsGet = true)] public string? Status { get; set; } // ongoing|success|cancelled
        [BindProperty(SupportsGet = true)] public DateOnly? From { get; set; } // ngày (local)
        [BindProperty(SupportsGet = true)] public DateOnly? To { get; set; }   // ngày (local)

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = HttpContext.Session.GetInt32("User.Id");
            if (userId is null)
            {
                TempData["Info"] = "Vui lòng đăng nhập để xem lịch đặt.";
                return RedirectToPage("/Auth/Login");
            }

            var all = await _query.GetMyBookingsAsync(userId.Value);

            // ---- Status filter
            if (!string.IsNullOrWhiteSpace(Status))
            {
                var st = Status.Trim().ToLowerInvariant();
                if (_validStatus.Contains(st))
                    all = all.Where(x => string.Equals(x.Status, st, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // ---- Date range filter (local -> UTC)
            DateTime? fromUtc = null, toUtc = null;
            if (From.HasValue)
            {
                var localStart = new DateTime(From.Value.Year, From.Value.Month, From.Value.Day, 0, 0, 0, DateTimeKind.Local);
                fromUtc = localStart.ToUniversalTime();
            }
            if (To.HasValue)
            {
                // tới cuối ngày 23:59:59 local
                var localEnd = new DateTime(To.Value.Year, To.Value.Month, To.Value.Day, 23, 59, 59, DateTimeKind.Local);
                toUtc = localEnd.ToUniversalTime();
            }
            if (fromUtc.HasValue)
                all = all.Where(x => x.StartTime.HasValue && x.StartTime.Value >= fromUtc.Value).ToList();
            if (toUtc.HasValue)
                all = all.Where(x => x.StartTime.HasValue && x.StartTime.Value <= toUtc.Value).ToList();

            // ---- Search (station/addr/port)
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

        // NEW: POST /Bookings?handler=Cancel
        public async Task<IActionResult> OnPostCancelAsync(int id)
        {
            var userId = HttpContext.Session.GetInt32("User.Id");
            if (userId is null)
            {
                TempData["Info"] = "Vui lòng đăng nhập.";
                return RedirectToPage("/Auth/Login");
            }

            var (ok, msg) = await _cmd.CancelAsync(userId.Value, id);
            TempData[ok ? "Info" : "Error"] = msg;

            // Quay lại trang list, giữ filter nếu muốn
            var route = new { Q, Status };
            return RedirectToPage(new { Q, Status });
        }
    }
}

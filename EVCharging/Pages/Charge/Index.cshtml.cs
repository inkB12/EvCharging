using EVCharging.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVCharging.Pages.Charge
{
    public class IndexModel : PageModel
    {
        private readonly IChargeRuntimeService _svc;
        public IndexModel(IChargeRuntimeService svc) => _svc = svc;

        [BindProperty(SupportsGet = true)]
        public int SessionId { get; set; }

        // thông tin để hiển thị sơ bộ
        public string? StationName { get; private set; }
        public string? StationLocation { get; private set; }
        public string? PortType { get; private set; }
        public int? PowerLevelKW { get; private set; }
        public int? ChargingSpeedKW { get; private set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (SessionId <= 0) return RedirectToPage("/Staff/Booking/Index");

            var (ok, msg, dto) = await _svc.GetSessionAsync(SessionId);
            if (!ok || dto == null)
            {
                TempData["Error"] = msg;
                return RedirectToPage("/Staff/Booking/Index");
            }

            StationName = dto.StationName;
            StationLocation = dto.StationLocation;
            PortType = dto.PortType;
            PowerLevelKW = dto.PowerLevelKW;
            ChargingSpeedKW = dto.ChargingSpeedKW;

            return Page();
        }
    }
}

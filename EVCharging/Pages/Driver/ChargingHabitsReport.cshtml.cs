using EVCharging.BLL.DTO;
using EVCharging.BLL.Interfaces;
using EVCharging.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVCharging.Pages.Driver
{
    public class ChargingHabitsReportModel(IReportService reportService) : PageModel
    {
        private readonly IReportService _reportService = reportService;

        [BindProperty(SupportsGet = true)]
        public int SelectedYear { get; set; } = DateTime.Now.Year;
        public int TotalSessionsYear { get; set; }

        public List<ChargingLocationHabitDTO> LocationHabits { get; set; } = [];
        public List<ChargingTimeHabitDTO> TimeHabits { get; set; } = [];

        public async Task<IActionResult> OnGetAsync()
        {
            int userId = HttpContext.Session.GetInt32(SessionKeys.UserId) ?? 0;
            if (userId == 0)
            {
                return RedirectToPage("/Auth/Login");
            }

            LocationHabits = await _reportService.GetChargingLocationHabits(userId, SelectedYear);
            TimeHabits = await _reportService.GetChargingTimeHabits(userId, SelectedYear);
            TotalSessionsYear = LocationHabits.Sum(h => h.SessionCount);

            return Page();
        }
    }
}

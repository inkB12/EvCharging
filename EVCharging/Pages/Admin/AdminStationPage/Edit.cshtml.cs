using EVCharging.BLL.AdminDTOs;
using EVCharging.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVCharging.Pages.Admin.AdminStationPage
{
    public class EditModel : PageModel
    {
        private readonly IAdminChargingStationService _stationService;

        [BindProperty]
        public AdminChargingStationDTO Station { get; set; } = new();

        public EditModel(IAdminChargingStationService stationService)
        {
            _stationService = stationService;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var station = await _stationService.GetByIdAsync(id);
            if (station == null) return RedirectToPage("Index");

            Station = station;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            await _stationService.UpdateAsync(Station);
            return RedirectToPage("Index");
        }
    }
}

using EVCharging.BLL.AdminDTOs;
using EVCharging.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVCharging.Pages.Admin.AdminStationPage
{
    public class DetailsModel : PageModel
    {
        private readonly IAdminChargingStationService _stationService;
        public AdminChargingStationDTO Station { get; set; } = new();

        public DetailsModel(IAdminChargingStationService stationService)
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
    }
}

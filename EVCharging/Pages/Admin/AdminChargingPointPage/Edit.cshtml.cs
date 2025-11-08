using EVCharging.BLL.AdminDTOs;
using EVCharging.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVCharging.Pages.Admin.AdminChargingPointPage
{
    public class EditModel : PageModel
    {
        private readonly IAdminChargingPointService _pointService;
        private readonly IAdminChargingStationService _stationService;

        [BindProperty]
        public AdminChargingPointDTO Point { get; set; } = new();

        public List<AdminChargingStationDTO> Stations { get; set; } = new();

        public EditModel(IAdminChargingPointService pointService, IAdminChargingStationService stationService)
        {
            _pointService = pointService;
            _stationService = stationService;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var point = await _pointService.GetByIdAsync(id);
            if (point == null) return RedirectToPage("Index");

            Point = point;
            Stations = await _stationService.GetAllAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Stations = await _stationService.GetAllAsync();
                return Page();
            }

            await _pointService.UpdateAsync(Point);
            return RedirectToPage("Index");
        }
    }
}

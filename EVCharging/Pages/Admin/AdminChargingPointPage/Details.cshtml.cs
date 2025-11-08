using EVCharging.BLL.AdminDTOs;
using EVCharging.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVCharging.Pages.Admin.AdminChargingPointPage
{
    public class DetailsModel : PageModel
    {
        private readonly IAdminChargingPointService _pointService;
        private readonly IAdminChargingStationService _stationService;

        public AdminChargingPointDTO Point { get; set; } = new();
        public string StationName { get; set; } = "";

        public DetailsModel(IAdminChargingPointService pointService, IAdminChargingStationService stationService)
        {
            _pointService = pointService;
            _stationService = stationService;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var point = await _pointService.GetByIdAsync(id);
            if (point == null) return RedirectToPage("Index");

            Point = point;
            var station = await _stationService.GetByIdAsync(point.StationId);
            StationName = station?.Name ?? "(Không xác định)";
            return Page();
        }
    }
}

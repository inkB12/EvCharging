using EVCharging.BLL.AdminDTOs;
using EVCharging.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVCharging.Pages.Admin.AdminChargingPointPage
{
    public class CreateModel : PageModel
    {
        private readonly IAdminChargingPointService _pointService;
        private readonly IAdminChargingStationService _stationService;

        [BindProperty]
        public AdminChargingPointDTO Point { get; set; } = new();

        public List<AdminChargingStationDTO> Stations { get; set; } = new();

        public CreateModel(IAdminChargingPointService pointService, IAdminChargingStationService stationService)
        {
            _pointService = pointService;
            _stationService = stationService;
        }

        public async Task OnGetAsync()
        {
            Stations = await _stationService.GetAllAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Stations = await _stationService.GetAllAsync();
                return Page();
            }

            await _pointService.AddAsync(Point);
            return RedirectToPage("Index");
        }
    }
}

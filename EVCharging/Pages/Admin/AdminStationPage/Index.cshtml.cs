using EVCharging.BLL.AdminDTOs;
using EVCharging.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVCharging.Pages.Admin.AdminStationPage
{
    public class IndexModel : PageModel
    {
        private readonly IAdminChargingStationService _stationService;

        public List<AdminChargingStationDTO> Stations { get; set; } = new();

        public IndexModel(IAdminChargingStationService stationService)
        {
            _stationService = stationService;
        }

        public async Task OnGetAsync()
        {
            Stations = await _stationService.GetAllAsync();
        }
    }
}

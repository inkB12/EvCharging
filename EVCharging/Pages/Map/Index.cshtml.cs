using EVCharging.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVCharging.Pages.Map
{
    public class IndexModel : PageModel
    {
        private readonly IChargingStationService _stationService;
        public IndexModel(IChargingStationService stationService)
        {
            _stationService = stationService;
        }

        public void OnGet() { }
    }
}

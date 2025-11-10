using EVCharging.BLL.AdminDTOs;
using EVCharging.BLL.Interfaces;
using EVCharging.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;

namespace EVCharging.Pages.Admin.AdminStationPage
{
    public class CreateModel : PageModel
    {
        private readonly IAdminChargingStationService _stationService;
        private readonly IHubContext<StationHub> _hub;

        [BindProperty]
        public AdminChargingStationDTO Station { get; set; } = new();

        public CreateModel(IAdminChargingStationService stationService, IHubContext<StationHub> hub)
        {
            _stationService = stationService;
            _hub = hub;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var entity = await _stationService.AddAsync(Station);
            if (entity != null)
            {
                await _hub.Clients.All.SendAsync("StationCreated", entity);
            }


            return RedirectToPage("Index");
        }
    }
}

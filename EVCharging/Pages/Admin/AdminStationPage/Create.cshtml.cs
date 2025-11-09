using EVCharging.BLL.AdminDTOs;
using EVCharging.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVCharging.Pages.Admin.AdminStationPage
{
    public class CreateModel : PageModel
    {
        private readonly IAdminChargingStationService _stationService;

        [BindProperty]
        public AdminChargingStationDTO Station { get; set; } = new();

        public CreateModel(IAdminChargingStationService stationService)
        {
            _stationService = stationService;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            await _stationService.AddAsync(Station);
            return RedirectToPage("Index");
        }
    }
}

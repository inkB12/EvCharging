using EVCharging.BLL.AdminDTOs;
using EVCharging.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVCharging.Pages.Admin.AdminStationPage
{
    public class DeleteModel : PageModel
    {
        private readonly IAdminChargingStationService _stationService;

        [BindProperty]
        public AdminChargingStationDTO Station { get; set; } = new();

        public DeleteModel(IAdminChargingStationService stationService)
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
            // Xóa mềm (đặt trạng thái thành offline)
            await _stationService.DeleteAsync(Station.Id);
            return RedirectToPage("Index");
        }
    }
}

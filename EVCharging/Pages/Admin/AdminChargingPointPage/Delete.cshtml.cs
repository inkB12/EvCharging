using EVCharging.BLL.AdminDTOs;
using EVCharging.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVCharging.Pages.Admin.AdminChargingPointPage
{
    public class DeleteModel : PageModel
    {
        private readonly IAdminChargingPointService _pointService;

        [BindProperty]
        public AdminChargingPointDTO Point { get; set; } = new();

        public DeleteModel(IAdminChargingPointService pointService)
        {
            _pointService = pointService;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var point = await _pointService.GetByIdAsync(id);
            if (point == null) return RedirectToPage("Index");

            Point = point;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _pointService.DeleteAsync(Point.Id); // soft delete = offline
            return RedirectToPage("Index");
        }
    }
}

using EVCharging.BLL.AdminDTOs;
using EVCharging.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVCharging.Pages.Admin.AdminFaultReportPage
{
    public class DetailsModel : PageModel
    {
        private readonly IAdminFaultReportService _faultService;

        [BindProperty]
        public AdminFaultReportDTO FaultReport { get; set; } = new();

        public DetailsModel(IAdminFaultReportService faultService)
        {
            _faultService = faultService;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var report = await _faultService.GetByIdAsync(id);
            if (report == null) return RedirectToPage("Index");
            FaultReport = report;
            return Page();
        }

        public async Task<IActionResult> OnPostCloseAsync()
        {
            await _faultService.UpdateStatusAsync(FaultReport.Id, "closed");
            TempData["Success"] = "Đã đóng báo lỗi!";
            return RedirectToPage("Index");
        }
    }
}

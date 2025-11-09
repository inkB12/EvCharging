using EVCharging.BLL.AdminDTOs;
using EVCharging.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVCharging.Pages.Admin.AdminFaultReportPage
{
    public class DeleteModel : PageModel
    {
        private readonly IAdminFaultReportService _faultService;

        [BindProperty]
        public AdminFaultReportDTO FaultReport { get; set; } = new();

        public DeleteModel(IAdminFaultReportService faultService)
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

        public async Task<IActionResult> OnPostAsync()
        {
            await _faultService.SoftDeleteAsync(FaultReport.Id);
            TempData["Success"] = "Xóa mềm báo lỗi thành công!";
            return RedirectToPage("Index");
        }
    }
}

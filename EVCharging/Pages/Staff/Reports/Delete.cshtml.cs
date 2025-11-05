using EVCharging.BLL.DTO;
using EVCharging.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVCharging.Pages.Staff.Reports
{
    public class DeleteModel : PageModel
    {
        private readonly IFaultReportService _faultReportService;

        public DeleteModel(IFaultReportService faultReportService)
        {
            _faultReportService = faultReportService;
        }

        [BindProperty]
        public FaultReportDto Report { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Report = await _faultReportService.GetFaultReportByIdAsync(id);
            if (Report == null)
            {
                return NotFound();
            }

            // Lấy thêm thông tin tên để hiển thị (GetByIdAsync không lấy)
            var allReports = await _faultReportService.GetAllFaultReportAsync();
            var fullReport = allReports.FirstOrDefault(r => r.Id == id);
            if (fullReport != null)
            {
                Report.PointName = fullReport.PointName;
                Report.ReportedByUserName = fullReport.ReportedByUserName;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            await _faultReportService.DeleteFaultReportAsync(id);
            return RedirectToPage("./Index");
        }
    }
}

using EVCharging.BLL.DTO;
using EVCharging.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVCharging.Pages.Staff.Reports
{
    public class IndexModel : PageModel
    {
        private readonly IFaultReportService _faultReportService;

        public IndexModel(IFaultReportService faultReportService)
        {
            _faultReportService = faultReportService;
        }

        public IEnumerable<FaultReportDto> Reports { get; set; } = new List<FaultReportDto>();

        public async Task OnGetAsync()
        {
            Reports = await _faultReportService.GetAllFaultReportAsync();
        }

        // THÊM HÀM NÀY: Xử lý POST từ Modal Delete
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _faultReportService.DeleteFaultReportAsync(id);
            return RedirectToPage(); // Tải lại trang Index
        }
    }
}

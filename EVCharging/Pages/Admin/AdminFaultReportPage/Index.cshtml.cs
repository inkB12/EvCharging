using EVCharging.BLL.AdminDTOs;
using EVCharging.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVCharging.Pages.Admin.AdminFaultReportPage
{
    public class IndexModel : PageModel
    {
        private readonly IAdminFaultReportService _faultService;
        public List<AdminFaultReportDTO> FaultReports { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? StatusFilter { get; set; }

        public IndexModel(IAdminFaultReportService faultService)
        {
            _faultService = faultService;
        }

        public async Task OnGetAsync()
        {
            if (string.IsNullOrEmpty(StatusFilter))
                FaultReports = await _faultService.GetAllAsync();
            else
                FaultReports = await _faultService.GetByStatusAsync(StatusFilter);
        }
    }
}

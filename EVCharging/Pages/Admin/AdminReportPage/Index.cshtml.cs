using EVCharging.BLL.AdminDTOs;
using EVCharging.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVCharging.Pages.Admin.AdminReportPage
{
    public class IndexModel : PageModel
    {
        private readonly IAdminDashboardService _dashboardService;

        public IndexModel(IAdminDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [BindProperty(SupportsGet = true)]
        public int Year { get; set; } = DateTime.Now.Year;

        public AdminDashboardDTO Dashboard { get; set; } = new();

        public async Task OnGetAsync()
        {
            Dashboard = await _dashboardService.GetDashboardSummaryAsync(Year);
        }
    }
}

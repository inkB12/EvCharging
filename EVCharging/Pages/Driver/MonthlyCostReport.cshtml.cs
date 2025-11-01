using EVCharging.BLL.DTO;
using EVCharging.BLL.Interfaces;
using EVCharging.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVCharging.Pages.Driver
{
    public class MonthlyCostReportModel(IReportService reportService) : PageModel
    {
        private readonly IReportService _reportService = reportService;

        #region Request
        [BindProperty(SupportsGet = true)]
        public int SelectedYear { get; set; } = DateTime.Now.Year;
        #endregion

        #region Show
        public List<MonthlyCostReportDTO> MonthlyReportData { get; set; } = [];
        #endregion

        public async Task<IActionResult> OnGetAsync()
        {
            int userId = HttpContext.Session.GetInt32(SessionKeys.UserId) ?? 0;
            if (userId == 0)
            {
                return RedirectToPage("/Auth/Login");
            }

            MonthlyReportData = await _reportService.GetMonthlyCostReportByYear(userId, SelectedYear);
            return Page();
        }
    }
}

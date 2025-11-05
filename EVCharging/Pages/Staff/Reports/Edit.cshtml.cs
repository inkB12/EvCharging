using EVCharging.BLL.DTO;
using EVCharging.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EVCharging.Pages.Staff.Reports
{
    public class EditModel : PageModel
    {
        private readonly IFaultReportService _faultReportService;
        private readonly IChargingPointService _pointService;

        public EditModel(IFaultReportService faultReportService, IChargingPointService pointService)
        {
            _faultReportService = faultReportService;
            _pointService = pointService;
        }

        [BindProperty]
        public FaultReportDto Report { get; set; }
        public SelectList ChargingPointList { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Report = await _faultReportService.GetFaultReportByIdAsync(id); // Giả sử tên hàm là GetFaultReportByIdAsync
            if (Report == null)
            {
                return NotFound();
            }

            // SỬA Ở ĐÂY:
            var stationId = HttpContext.Session.GetInt32("User.HomeStationId");
            var points = await _pointService.GetByStationAsync(stationId.Value); // Sửa số 1
            ChargingPointList = new SelectList(points, "Id", "PortType", Report.PointId);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _faultReportService.UpdateFaultReportAsync(Report); // Giả sử tên hàm là UpdateFaultReportAsync

            return RedirectToPage("./Index");
        }
    }
}
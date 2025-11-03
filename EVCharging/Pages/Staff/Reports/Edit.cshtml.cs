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
            // Tên hàm này (GetFaultReportByIdAsync) phải khớp với IFaultReportService
            Report = await _faultReportService.GetFaultReportByIdAsync(id); // Giả sử tên hàm là GetReportByIdAsync

            if (Report == null)
            {
                return NotFound();
            }

            // SỬA Ở ĐÂY:
            // Load danh sách điểm sạc TẠM THỜI của trạm số 1
            var points = await _pointService.GetByStationAsync(1); // <-- Sửa hàm không tồn tại

            ChargingPointList = new SelectList(points, "Id", "PortType", Report.PointId); // <-- Đổi "Name" thành "PortType"
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // SỬA Ở ĐÂY:
                // Phải load lại SelectList nếu post không thành công
                var points = await _pointService.GetByStationAsync(1); // <-- Sửa hàm không tồn tại
                ChargingPointList = new SelectList(points, "Id", "PortType", Report.PointId); // <-- Đổi "Name" thành "PortType"
                return Page();
            }

            // Tên hàm này (UpdateFaultReportAsync) phải khớp với IFaultReportService
            await _faultReportService.UpdateFaultReportAsync(Report); // Giả sử tên hàm là UpdateReportAsync

            return RedirectToPage("./Index");
        }
    }
}

using EVCharging.BLL.DTO;
using EVCharging.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EVCharging.Pages.Staff.Reports
{
    public class CreateModel : PageModel
    {
        private readonly IFaultReportService _faultReportService;
        private readonly IChargingPointService _pointService;

        public CreateModel(IFaultReportService faultReportService, IChargingPointService pointService)
        {
            _faultReportService = faultReportService;
            _pointService = pointService;
        }

        [BindProperty]
        public FaultReportDto Report { get; set; } = new FaultReportDto();

        public SelectList ChargingPointList { get; set; }

        public async Task OnGetAsync()
        {
            // SỬA Ở ĐÂY:
            // Load danh sách điểm sạc TẠM THỜI của trạm số 1 (vì chưa có login)
            var points = await _pointService.GetByStationAsync(1); // <-- Gán cứng StationId = 1

            // Giả sử ChargingPointDto có thuộc tính 'Id' và 'Name'
            // (Nếu DTO của bạn không có 'Name', bạn cần sửa "Name" thành "Id" hoặc "PortType")
            ChargingPointList = new SelectList(points, "Id", "PortType"); // <-- Đổi "Name" thành "PortType" hoặc gì đó có ý nghĩa
        }

        public async Task<IActionResult> OnPostAsync()
        {


            // GÁN (HARDCODE) ID CỦA STAFF (kiểu int) VÌ CHƯA CÓ LOGIN
            // Bạn hãy vào DB, tìm 1 UserId (kiểu int) của Staff và dán vào đây
            Report.UserId = 1; // <-- THAY ID NÀY

            Report.ReportTime = DateTime.UtcNow;
            Report.Status = "Open"; // Trạng thái ban đầu

            // Tên hàm này (CreateFaultReportAsync) phải khớp với IFaultReportService
            await _faultReportService.CreateFaultReportAsync(Report); // Giả sử tên hàm là CreateReportAsync

            return RedirectToPage("./Index");
        }
    }
}

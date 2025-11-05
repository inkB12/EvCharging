using EVCharging.BLL.DTO;
using EVCharging.BLL.Interfaces;
// XÓA: using EVCharging.Infrastructure; // (Vì bạn không dùng SessionKeys.cs)
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

        public async Task<IActionResult> OnGetAsync()
        {
            // BƯỚC 1: KIỂM TRA ROLE TRƯỚC
            var userRole = HttpContext.Session.GetString("User.Role");
            if (userRole != "Staff")
            {
                // Nếu không phải Staff, đuổi về trang chủ
                TempData["Error"] = "Bạn không có quyền truy cập trang này.";
                return RedirectToPage("/Index");
            }

            // BƯỚC 2: KIỂM TRA STATION ID
            var stationId = HttpContext.Session.GetInt32("User.HomeStationId");

            if (stationId.HasValue)
            {
                var points = await _pointService.GetByStationAsync(stationId.Value);
                ChargingPointList = new SelectList(points, "Id", "PortType"); // Dùng PortType
            }
            else
            {
                // Staff này có role nhưng chưa được gán trạm
                ChargingPointList = new SelectList(Enumerable.Empty<SelectListItem>());
                TempData["Message"] = "Tài khoản Staff của bạn chưa được gán trạm. Không thể tạo báo cáo.";
            }

            return Page(); // <-- Phải return Page() ở đây
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Lấy UserId trước để kiểm tra
            var userId = HttpContext.Session.GetInt32("User.Id");
            if (!userId.HasValue)
            {
                return RedirectToPage("/Auth/Login"); // Bắt đăng nhập lại
            }

            // KIỂM TRA MODELSTATE


            // Nếu mọi thứ OK, gán và tạo
            Report.UserId = userId.Value;
            Report.ReportTime = DateTime.UtcNow;
            Report.Status = "Open";

            await _faultReportService.CreateFaultReportAsync(Report);

            return RedirectToPage("./Index");
        }
    }
}
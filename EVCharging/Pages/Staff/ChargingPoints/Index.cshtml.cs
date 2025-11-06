using EVCharging.BLL.DTO;
using EVCharging.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVCharging.Pages.Staff.ChargingPoints
{
    public class IndexModel : PageModel
    {
        private readonly IChargingPointService _pointService;
        private readonly IChargingStationService _stationService;

        public IndexModel(
            IChargingPointService pointService,
            IChargingStationService stationService)
        {
            _pointService = pointService;
            _stationService = stationService;
        }

        public string StationName { get; set; } = "Không xác định";
        public List<ChargingPointDto> ChargingPoints { get; set; } = new List<ChargingPointDto>();

        // --- THÊM MỚI: CÁC BIẾN CHO THẺ THỐNG KÊ ---
        public int TotalPoints { get; set; }
        public int AvailablePoints { get; set; }
        public int ChargingPointsCount { get; set; }
        public int FaultyPoints { get; set; }
        // ------------------------------------------

        public async Task<IActionResult> OnGetAsync()
        {
            // 1. Kiểm tra quyền
            var userRole = HttpContext.Session.GetString("User.Role");
            if (userRole != "Staff")
            {
                TempData["Error"] = "Bạn không có quyền truy cập trang này.";
                return RedirectToPage("/Index");
            }

            // 2. Lấy StationId từ Session (dùng key "User.HomeStationId")
            int? stationId = HttpContext.Session.GetInt32("User.HomeStationId");

            if (!stationId.HasValue)
            {
                TempData["Message"] = "Nhân viên này chưa được gán vào trạm sạc nào.";
                return Page();
            }

            // 3. Lấy tên trạm
            var stationDto = await _stationService.GetByIdAsync(stationId.Value);
            if (stationDto != null)
            {
                StationName = stationDto.Name;
            }

            // 4. Lấy danh sách điểm sạc
            ChargingPoints = await _pointService.GetByStationAsync(stationId.Value);

            // 5. (THÊM MỚI) Tính toán các số liệu thống kê
            TotalPoints = ChargingPoints.Count;
            AvailablePoints = ChargingPoints.Count(p => p.Status == "Available");
            ChargingPointsCount = ChargingPoints.Count(p => p.Status == "Charging");
            FaultyPoints = ChargingPoints.Count(p => p.Status == "Faulty");

            return Page();
        }
    }
}
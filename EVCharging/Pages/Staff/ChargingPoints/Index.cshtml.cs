using EVCharging.BLL.DTO;
using EVCharging.BLL.Interfaces;
// XÓA: using EVCharging.DAL.Interfaces; // Không cần nữa
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVCharging.Pages.Staff.ChargingPoints
{
    public class IndexModel : PageModel
    {
        private readonly IChargingPointService _pointService;
        private readonly IChargingStationService _stationService;
        // XÓA: private readonly IUserRepository _userRepository;

        public IndexModel(
            IChargingPointService pointService,
            IChargingStationService stationService) // <-- XÓA _userRepository
        {
            _pointService = pointService;
            _stationService = stationService;
        }

        public string StationName { get; set; } = "Không xác định";
        public List<ChargingPointDto> ChargingPoints { get; set; } = new List<ChargingPointDto>();

        public async Task<IActionResult> OnGetAsync()
        {
            // SỬA Ở ĐÂY: Lấy StationId trực tiếp từ Session
            int? stationId = HttpContext.Session.GetInt32("User.HomeStationId");

            if (!stationId.HasValue)
            {
                TempData["Message"] = "Nhân viên này chưa được gán vào trạm sạc nào.";
                return Page();
            }

            // Mọi thứ từ đây đều là async, sẽ không còn lỗi
            var stationDto = await _stationService.GetByIdAsync(stationId.Value); // Sửa: GetByIdAsync
            if (stationDto != null)
            {
                StationName = stationDto.Name;
            }

            ChargingPoints = await _pointService.GetByStationAsync(stationId.Value);

            return Page();
        }
    }
}
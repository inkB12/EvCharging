using EVCharging.BLL.DTO;
using EVCharging.BLL.Interfaces;
using EVCharging.DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVCharging.Pages.Staff.ChargingPoints
{
    public class IndexModel : PageModel
    {
        private readonly IChargingPointService _pointService;
        private readonly IChargingStationService _stationService;
        private readonly IUserRepository _userRepository;

        public IndexModel(
            IChargingPointService pointService,
            IChargingStationService stationService,
            IUserRepository userRepository)
        {
            _pointService = pointService;
            _stationService = stationService;
            _userRepository = userRepository;
        }

        public string StationName { get; set; } = "Không xác định";
        public List<ChargingPointDto> ChargingPoints { get; set; } = new List<ChargingPointDto>();

        public async Task<IActionResult> OnGetAsync()
        {
            int staffUserId = 1; // Gán cứng UserId (sẽ sửa sau khi có login)

            // SỬA Ở ĐÂY: Gọi GetByIdAsync thay vì GetById
            var staffUser = await _userRepository.GetByIdAsync(staffUserId);

            if (staffUser == null)
            {
                TempData["Message"] = "Lỗi: Không tìm thấy nhân viên với ID này.";
                return Page();
            }

            if (!staffUser.HomeStationId.HasValue)
            {
                TempData["Message"] = "Nhân viên này chưa được gán vào trạm sạc nào.";
                return Page();
            }

            int stationId = staffUser.HomeStationId.Value;

            // Mọi thứ từ đây đều là async, sẽ không còn lỗi
            var stationDto = await _stationService.GetByIdAsync(stationId);
            if (stationDto != null)
            {
                StationName = stationDto.Name;
            }

            ChargingPoints = await _pointService.GetByStationAsync(stationId);

            return Page();
        }
    }
}

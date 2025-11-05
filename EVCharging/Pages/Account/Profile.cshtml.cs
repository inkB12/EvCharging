using EVCharging.BLL.DTO;
using EVCharging.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVCharging.Pages.Account
{
    public class ProfileModel : PageModel
    {
        private readonly IUserService _userSvc;
        private readonly IChargingStationService _stationSvc;

        public ProfileModel(IUserService userSvc, IChargingStationService stationSvc)
        {
            _userSvc = userSvc;
            _stationSvc = stationSvc;
        }

        // View models
        [BindProperty] public UpdateProfileRequest ProfileInput { get; set; } = new();
        [BindProperty] public ChangePasswordRequest PasswordInput { get; set; } = new();

        public UserProfileDTO? Profile { get; set; }
        public List<ChargingStationDto> Stations { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = HttpContext.Session.GetInt32("User.Id");
            if (userId is null)
            {
                TempData["Info"] = "Vui lòng đăng nhập.";
                return RedirectToPage("/Auth/Login");
            }

            Profile = await _userSvc.GetProfileAsync(userId.Value);
            if (Profile == null) return RedirectToPage("/Auth/Login");

            // fill form defaults
            ProfileInput = new UpdateProfileRequest
            {
                FullName = Profile.FullName,
                Phone = Profile.Phone,
                HomeStationId = Profile.HomeStationId
            };

            // lấy danh sách trạm (để chọn Home Station)
            Stations = await _stationSvc.GetAllAsync();

            return Page();
        }

        // Cập nhật thông tin chung
        public async Task<IActionResult> OnPostUpdateAsync()
        {
            var userId = HttpContext.Session.GetInt32("User.Id");
            if (userId is null) return RedirectToPage("/Auth/Login");

            var (ok, msg) = await _userSvc.UpdateProfileAsync(userId.Value, ProfileInput);
            TempData[ok ? "Info" : "Error"] = msg;

            if (ok)
            {
                // cập nhật lại session display name
                if (!string.IsNullOrWhiteSpace(ProfileInput.FullName))
                    HttpContext.Session.SetString("User.FullName", ProfileInput.FullName);
            }

            return RedirectToPage(); // reload
        }

        // Đổi mật khẩu
        public async Task<IActionResult> OnPostChangePasswordAsync()
        {
            var userId = HttpContext.Session.GetInt32("User.Id");
            if (userId is null) return RedirectToPage("/Auth/Login");

            var (ok, msg) = await _userSvc.ChangePasswordAsync(userId.Value, PasswordInput);
            TempData[ok ? "Info" : "Error"] = msg;

            return RedirectToPage();
        }
    }
}

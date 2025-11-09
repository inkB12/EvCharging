using EVCharging.BLL.AdminDTOs;
using EVCharging.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EVCharging.Pages.Admin.AdminUserPage
{
    public class EditModel : PageModel
    {
        private readonly IAdminUserService _userService;
        private readonly IAdminServicePlanService _planService;
        private readonly IAdminChargingStationService _stationService;

        public EditModel(
            IAdminUserService userService,
            IAdminServicePlanService planService,
            IAdminChargingStationService stationService)
        {
            _userService = userService;
            _planService = planService;
            _stationService = stationService;
        }

        [BindProperty]
        public AdminUserDTO User { get; set; } = new();

        public List<SelectListItem> ServicePlanList { get; set; } = new();
        public List<SelectListItem> StationList { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                TempData["Error"] = "Người dùng không tồn tại hoặc đã bị xóa.";
                return RedirectToPage("Index");
            }

            User = user;

            await LoadDropdownsAsync(user.ServicePlanId, user.HomeStationId);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync(User.ServicePlanId, User.HomeStationId);
                return Page();
            }

            await _userService.UpdateAsync(User);
            TempData["Success"] = "Cập nhật thông tin người dùng thành công!";
            return RedirectToPage("Index");
        }

        private async Task LoadDropdownsAsync(int? selectedPlanId, int? selectedStationId)
        {
            var plans = await _planService.GetAllAsync();
            ServicePlanList = plans.Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.Id.ToString(),
                Selected = (p.Id == selectedPlanId)
            }).ToList();

            var stations = await _stationService.GetAllAsync();
            StationList = stations.Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.Id.ToString(),
                Selected = (s.Id == selectedStationId)
            }).ToList();
        }
    }
}

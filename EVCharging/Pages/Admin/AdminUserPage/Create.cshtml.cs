using EVCharging.BLL.AdminDTOs;
using EVCharging.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EVCharging.Pages.Admin.AdminUserPage
{
    public class CreateModel : PageModel
    {
        private readonly IAdminUserService _userService;
        private readonly IAdminServicePlanService _planService;
        private readonly IAdminChargingStationService _stationService;

        public CreateModel(
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

        public async Task OnGetAsync()
        {
            await LoadDropdownsAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync();
                return Page();
            }

            await _userService.AddAsync(User);
            TempData["Success"] = "Thêm người dùng mới thành công!";
            return RedirectToPage("Index");
        }

        private async Task LoadDropdownsAsync()
        {
            var plans = await _planService.GetAllAsync();
            ServicePlanList = plans.Select(p => new SelectListItem
            {
                Text = p.Name ?? $"Gói #{p.Id}",
                Value = p.Id.ToString()
            }).ToList();

            var stations = await _stationService.GetAllAsync();
            StationList = stations.Select(s => new SelectListItem
            {
                Text = s.Name ?? $"Trạm #{s.Id}",
                Value = s.Id.ToString()
            }).ToList();
        }
    }
}

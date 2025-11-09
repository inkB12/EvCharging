using EVCharging.BLL.AdminDTOs;
using EVCharging.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EVCharging.Pages.Admin.AdminFaultReportPage
{
    public class CreateModel : PageModel
    {
        private readonly IAdminFaultReportService _faultService;
        private readonly IAdminUserService _userService;
        private readonly IAdminChargingPointService _pointService;

        public CreateModel(IAdminFaultReportService faultService, IAdminUserService userService, IAdminChargingPointService pointService)
        {
            _faultService = faultService;
            _userService = userService;
            _pointService = pointService;
        }

        [BindProperty]
        public AdminFaultReportDTO FaultReport { get; set; } = new();

        public List<SelectListItem> UserList { get; set; } = new();
        public List<SelectListItem> PointList { get; set; } = new();

        public async Task OnGetAsync() => await LoadDropdownsAsync();

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync();
                return Page();
            }

            await _faultService.AddAsync(FaultReport);
            TempData["Success"] = "Thêm báo lỗi mới thành công!";
            return RedirectToPage("Index");
        }

        private async Task LoadDropdownsAsync()
        {
            var users = await _userService.GetAllAsync();
            UserList = users.Select(u => new SelectListItem($"{u.FullName} ({u.Email})", u.Id.ToString())).ToList();

            var points = await _pointService.GetAllAsync();
            PointList = points.Select(p => new SelectListItem($"Point #{p.Id} ({p.PortType})", p.Id.ToString())).ToList();
        }
    }
}

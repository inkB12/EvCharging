using EVCharging.BLL.AdminDTOs;
using EVCharging.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVCharging.Pages.Admin.AdminUserPage
{
    public class DetailsModel : PageModel
    {
        private readonly IAdminUserService _userService;

        public AdminUserDTO User { get; set; } = new();

        public DetailsModel(IAdminUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return RedirectToPage("Index");
            User = user;
            return Page();
        }
    }
}

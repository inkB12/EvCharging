using EVCharging.BLL.AdminDTOs;
using EVCharging.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVCharging.Pages.Admin.AdminUserPage
{
    public class DeleteModel : PageModel
    {
        private readonly IAdminUserService _userService;

        [BindProperty]
        public AdminUserDTO User { get; set; } = new();

        public DeleteModel(IAdminUserService userService)
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

        public async Task<IActionResult> OnPostAsync()
        {
            await _userService.DeleteAsync(User.Id);
            return RedirectToPage("Index");
        }
    }
}

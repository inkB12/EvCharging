using EVCharging.BLL.AdminDTOs;
using EVCharging.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVCharging.Pages.Admin.AdminUserPage
{
    public class IndexModel : PageModel
    {
        private readonly IAdminUserService _userService;

        public List<AdminUserDTO> Users { get; set; } = new();

        public IndexModel(IAdminUserService userService)
        {
            _userService = userService;
        }

        public async Task OnGetAsync()
        {
            Users = await _userService.GetAllAsync();
        }
    }
}

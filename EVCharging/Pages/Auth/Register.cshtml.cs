using EVCharging.BLL.DTO;
using EVCharging.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVCharging.Pages.Auth
{
    public class RegisterModel : PageModel
    {
        private readonly IUserService _userService;
        public RegisterModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty] public RegisterRequest Input { get; set; } = new();
        public string? Info { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var (ok, msg, _) = await _userService.RegisterAsync(Input);
            if (!ok)
            {
                ModelState.AddModelError(string.Empty, msg);
                return Page();
            }

            Info = "Đăng ký thành công. Vui lòng đăng nhập.";
            return RedirectToPage("/Auth/Login");
        }
    }
}

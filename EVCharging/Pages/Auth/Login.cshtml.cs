using EVCharging.BLL.DTO;
using EVCharging.BLL.Interfaces;
using EVCharging.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVCharging.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly IUserService _userService;
        public LoginModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty] public LoginRequest Input { get; set; } = new();
        public string? Error { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            if (!ModelState.IsValid) return Page();

            var (ok, msg, user) = await _userService.LoginAsync(Input);
            if (!ok || user == null)
            {
                Error = msg;
                return Page();
            }

            HttpContext.Session.SetInt32(SessionKeys.UserId, user.Id);
            HttpContext.Session.SetString(SessionKeys.Email, user.Email);
            HttpContext.Session.SetString(SessionKeys.FullName, user.FullName ?? user.Email);
            HttpContext.Session.SetString(SessionKeys.Role, user.Role ?? "User");

            return LocalRedirect(returnUrl ?? "/");
        }
    }
}

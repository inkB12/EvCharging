using EVCharging.BLL.DTO;
using EVCharging.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVCharging.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly IUserService _userService;
        public LoginModel(IUserService userService) => _userService = userService;

        [BindProperty] public LoginRequest Input { get; set; } = new();
        public string? Error { get; set; }
        public string? Info { get; set; }

        public void OnGet()
        {
            Info = TempData["Info"] as string;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var (ok, msg, user) = await _userService.LoginAsync(Input);
            if (!ok)
            {
                Error = msg;
                return Page();
            }

            HttpContext.Session.SetInt32("User.Id", user.Id);
            HttpContext.Session.SetString("User.Email", user.Email);
            HttpContext.Session.SetString("User.FullName", user.FullName ?? string.Empty);
            HttpContext.Session.SetString("User.Role", user.Role ?? "User");

            return RedirectToPage("/Index");
        }
    }
}

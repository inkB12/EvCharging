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
            string userRole = user.Role ?? "User";
            HttpContext.Session.SetString("User.Role", user.Role ?? "User");
            if (user.HomeStationId.HasValue)
            {
                HttpContext.Session.SetInt32("User.HomeStationId", user.HomeStationId.Value);
            }
            else
            {
                HttpContext.Session.Remove("User.HomeStationId");
            }

            // 1. Kiểm tra vai trò và điều hướng
            if (userRole == "Staff")
            {
                // Nếu là Staff, chuyển đến trang Dashboard của Staff
                // (Chúng ta dùng Sessions/Index làm trang chính của Staff)
                return RedirectToPage("/Staff/Sessions/Index");
            }
            else if (userRole == "Admin")
            {
                // (Tùy chọn) Nếu bạn có trang Admin
                return RedirectToPage("/Admin/Index");
            }
            else
            {
                // Mặc định (vai trò "User"), chuyển đến trang chủ
                return RedirectToPage("/Index");
            }
        }
    }
}

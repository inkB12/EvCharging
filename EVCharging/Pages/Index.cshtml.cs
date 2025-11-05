using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVCharging.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return RedirectToPage("/Auth/Login");
            }
            // Redirect to actual Equipment list page
            return RedirectToPage("/Booking/Index");
        }
    }
}

using EVCharging.BLL.AdminDTOs;
using EVCharging.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVCharging.Pages.Admin.AdminServicePlanPage
{
    public class CreateModel : PageModel
    {
        private readonly IAdminServicePlanService _planService;

        [BindProperty]
        public AdminServicePlanDTO Plan { get; set; } = new();

        public CreateModel(IAdminServicePlanService planService)
        {
            _planService = planService;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            await _planService.AddAsync(Plan);
            return RedirectToPage("Index");
        }
    }
}

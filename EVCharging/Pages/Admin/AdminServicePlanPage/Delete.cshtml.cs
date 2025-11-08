using EVCharging.BLL.AdminDTOs;
using EVCharging.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVCharging.Pages.Admin.AdminServicePlanPage
{
    public class DeleteModel : PageModel
    {
        private readonly IAdminServicePlanService _planService;

        [BindProperty]
        public AdminServicePlanDTO Plan { get; set; } = new();

        public DeleteModel(IAdminServicePlanService planService)
        {
            _planService = planService;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var plan = await _planService.GetByIdAsync(id);
            if (plan == null) return RedirectToPage("Index");

            Plan = plan;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _planService.DeleteAsync(Plan.Id);
            return RedirectToPage("Index");
        }
    }
}

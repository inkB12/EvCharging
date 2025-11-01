using EVCharging.BLL.DTO;
using EVCharging.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVCharging.Pages.Admin.ServicePlans
{
    public class DeleteModel : PageModel
    {
        private readonly IServicePlanService _servicePlanService;
        public ServicePlanDto? ServicePlan { get; set; }

        public DeleteModel(IServicePlanService servicePlanService)
        {
            _servicePlanService = servicePlanService;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            ServicePlan = await _servicePlanService.GetByIdAsync(id);
            if (ServicePlan == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            await _servicePlanService.DeleteAsync(id);
            return RedirectToPage("Index");
        }
    }
}

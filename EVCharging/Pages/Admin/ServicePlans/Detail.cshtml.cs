using EVCharging.BLL.DTO;
using EVCharging.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVCharging.Pages.Admin.ServicePlans
{
    public class DetailModel : PageModel
    {
        private readonly IServicePlanService _servicePlanService;

        public DetailModel(IServicePlanService servicePlanService)
        {
            _servicePlanService = servicePlanService;
        }

        public ServicePlanDto? ServicePlan { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            ServicePlan = await _servicePlanService.GetByIdAsync(id);
            if (ServicePlan == null)
                return NotFound();

            return Page();
        }
    }
}

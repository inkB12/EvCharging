using EVCharging.BLL.DTO;
using EVCharging.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVCharging.Pages.Admin.ServicePlans
{
    public class IndexModel : PageModel
    {
        private readonly IServicePlanService _servicePlanService;
        public List<ServicePlanDto> ServicePlans { get; set; } = new();

        public IndexModel(IServicePlanService servicePlanService)
        {
            _servicePlanService = servicePlanService;
        }

        public async Task OnGet()
        {
            ServicePlans = await _servicePlanService.GetAllAsync();
        }
    }
}

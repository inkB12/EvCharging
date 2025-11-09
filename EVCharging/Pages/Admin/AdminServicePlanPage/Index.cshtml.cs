using EVCharging.BLL.AdminDTOs;
using EVCharging.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVCharging.Pages.Admin.AdminServicePlanPage
{
    public class IndexModel : PageModel
    {
        private readonly IAdminServicePlanService _servicePlanService;
        public List<AdminServicePlanDTO> Plans { get; set; } = new();

        public IndexModel(IAdminServicePlanService servicePlanService)
        {
            _servicePlanService = servicePlanService;
        }

        public async Task OnGetAsync()
        {
            Plans = await _servicePlanService.GetAllAsync();
        }
    }
}

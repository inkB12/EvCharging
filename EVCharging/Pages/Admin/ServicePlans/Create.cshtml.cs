using EVCharging.BLL.DTO;
using EVCharging.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace EVCharging.Pages.Admin.ServicePlans
{
    public class CreateModel : PageModel
    {
        private readonly IServicePlanService _servicePlanService;

        public CreateModel(IServicePlanService servicePlanService)
        {
            _servicePlanService = servicePlanService;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            [Required(ErrorMessage = "Tên gói là bắt buộc.")]
            public string Name { get; set; } = null!;

            public string? Description { get; set; }

            [Range(0, double.MaxValue, ErrorMessage = "Giá phải là số không âm.")]
            public decimal Price { get; set; }
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var dto = new ServicePlanDto
            {
                Name = Input.Name,
                Description = Input.Description,
                Price = Input.Price,
                IsDeleted = false
            };

            await _servicePlanService.CreateAsync(dto);
            return RedirectToPage("Index");
        }
    }
}

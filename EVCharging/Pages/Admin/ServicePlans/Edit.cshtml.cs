using EVCharging.BLL.DTO;
using EVCharging.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace EVCharging.Pages.Admin.ServicePlans
{
    public class EditModel : PageModel
    {
        private readonly IServicePlanService _servicePlanService;

        public EditModel(IServicePlanService servicePlanService)
        {
            _servicePlanService = servicePlanService;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            public int Id { get; set; }

            [Required]
            public string Name { get; set; } = null!;

            public string? Description { get; set; }

            public decimal Price { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var dto = await _servicePlanService.GetByIdAsync(id);
            if (dto == null) return NotFound();

            Input = new InputModel
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var dto = new ServicePlanDto
            {
                Id = Input.Id,
                Name = Input.Name,
                Description = Input.Description,
                Price = Input.Price
            };
            await _servicePlanService.UpdateAsync(dto);
            return RedirectToPage("Index");
        }
    }
}

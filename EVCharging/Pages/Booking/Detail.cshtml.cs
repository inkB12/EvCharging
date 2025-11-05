using EVCharging.BLL.DTO;
using EVCharging.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVCharging.Pages.Booking
{
    public class DetailModel : PageModel
    {
        private readonly IBookingQueryService _query;
        public DetailModel(IBookingQueryService query) => _query = query;

        public BookingDetailDTO? Item { get; private set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var userId = HttpContext.Session.GetInt32("User.Id");
            if (userId is null)
            {
                TempData["Info"] = "Vui lòng đăng nhập.";
                return RedirectToPage("/Auth/Login");
            }

            Item = await _query.GetDetailAsync(userId.Value, id);
            if (Item == null)
            {
                TempData["Error"] = "Không tìm thấy booking hoặc bạn không có quyền xem.";
                return RedirectToPage("/Bookings/Index");
            }
            return Page();
        }
    }
}

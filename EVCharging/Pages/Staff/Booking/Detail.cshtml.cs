using EVCharging.BLL.DTO;
using EVCharging.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace EVCharging.Pages.Staff.Booking
{
    public class DetailModel : PageModel
    {
        private readonly IStaffBookingQueryService _svc;
        private readonly IUserService _userSvc;

        public DetailModel(IStaffBookingQueryService svc, IUserService userSvc)
        {
            _svc = svc;
            _userSvc = userSvc;
        }

        public BookingDetailDTO? Item { get; private set; }

        // SessionId mới nhất của booking (để đặt nút nhanh ở header)
        public int LatestSessionId { get; private set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var userId = HttpContext.Session.GetInt32("User.Id");
            if (userId is null) return RedirectToPage("/Auth/Login");

            int? stationId = HttpContext.Session.GetInt32("User.HomeStationId");
            if (stationId is null)
            {
                TempData["Error"] = "Bạn chưa được gán Home Station.";
                return RedirectToPage("/Auth/Login");
            }

            var dto = await _svc.GetDetailAsync(stationId, id);
            if (dto == null)
            {
                TempData["Error"] = "Không có quyền xem hoặc booking không thuộc trạm của bạn.";
                return RedirectToPage("/Staff/Booking/Index");
            }

            Item = dto;
            LatestSessionId = Item.Sessions?
                .OrderByDescending(s => s.SessionId)
                .Select(s => s.SessionId)
                .FirstOrDefault() ?? 0;

            return Page();
        }
    }
}

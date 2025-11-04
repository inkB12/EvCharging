using EVCharging.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVCharging.Pages.Map
{
    [IgnoreAntiforgeryToken]
    public class ApiModel : PageModel
    {
        private readonly IMapSchedulingService _mapSvc;
        private readonly IUserService _userSvc;

        public ApiModel(IMapSchedulingService mapSvc, IUserService userSvc)
        {
            _mapSvc = mapSvc;
            _userSvc = userSvc;
        }

        //get station
        public async Task<IActionResult> OnGetStations()
        {
            var list = await _mapSvc.GetStationsAsync();
            return new JsonResult(list);
        }

        //get port types by station
        public async Task<IActionResult> OnGetPortTypes(int stationId)
        {
            var types = await _mapSvc.GetPortTypesByStationAsync(stationId);
            return new JsonResult(types);
        }

        //check availability
        public async Task<IActionResult> OnGetCheck(int stationId, string portType, DateOnly date, int startHour)
        {
            var res = await _mapSvc.CheckAvailabilityAsync(stationId, portType, date, startHour);
            return new JsonResult(new { ok = res.ok, msg = res.msg, candidatePointId = res.candidatePointId });
        }

        public class BookReq
        {
            public int StationId { get; set; }
            public string PortType { get; set; } = null!;
            public string Date { get; set; } = null!;
            public int StartHour { get; set; }
            public int PointId { get; set; }
        }

        //create booking
        public async Task<IActionResult> OnPostBook([FromBody] BookReq req)
        {
            if (req == null) return BadRequest();
            if (!DateOnly.TryParse(req.Date, out var date))
                return new JsonResult(new { ok = false, msg = "Ngày không hợp lệ." });

            int? userId = HttpContext.Session.GetInt32("User.Id");
            if (userId is null) return new JsonResult(new { ok = false, msg = "Bạn cần đăng nhập để đặt lịch." });

            var res = await _mapSvc.BookAsync(userId.Value, req.PointId, date, req.StartHour);
            return new JsonResult(new { ok = res.ok, msg = res.msg, bookingId = res.bookingId, sessionId = res.sessionId });
        }

    }
}

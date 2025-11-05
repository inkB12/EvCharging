using EVCharging.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVCharging.Pages.Charge
{
    [IgnoreAntiforgeryToken]
    public class ApiModel : PageModel
    {
        private readonly IChargeRuntimeService _svc;
        public ApiModel(IChargeRuntimeService svc) => _svc = svc;

        // GET /Charge/Api?handler=Session&sessionId=123
        public async Task<IActionResult> OnGetSession(int sessionId)
        {
            var (ok, msg, dto) = await _svc.GetSessionAsync(sessionId);
            if (!ok || dto == null) return new JsonResult(new { ok, msg });
            return new JsonResult(new { ok = true, data = dto });
        }

        // POST /Charge/Api?handler=Start  body: { sessionId: 123 }
        public class StartReq { public int SessionId { get; set; } }
        public async Task<IActionResult> OnPostStart([FromBody] StartReq req)
        {
            if (req == null || req.SessionId <= 0) return BadRequest();
            var (ok, msg) = await _svc.StartAsync(req.SessionId, DateTime.UtcNow);
            return new JsonResult(new { ok, msg });
        }

        // POST /Charge/Api?handler=Stop  body: { sessionId: 123 }
        public class StopReq { public int SessionId { get; set; } }
        public async Task<IActionResult> OnPostStop([FromBody] StopReq req)
        {
            if (req == null || req.SessionId <= 0) return BadRequest();
            var (ok, msg, energyKwh) = await _svc.StopAsync(req.SessionId, DateTime.UtcNow);
            return new JsonResult(new { ok, msg, energyKwh });
        }
    }
}

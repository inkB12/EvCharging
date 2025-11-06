using EVCharging.BLL.DTO;
using EVCharging.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVCharging.Pages.Charge
{
    public class IndexModel(IChargeRuntimeService svc, ITransactionService transactionService, IVNPayService vNPayService, IMomoService momoService, IChargingSessionService chargingSessionService, IStaffBookingQueryService bookingService) : PageModel
    {
        private readonly IChargeRuntimeService _svc = svc;
        private readonly IChargingSessionService _chargingSessionService = chargingSessionService;
        private readonly ITransactionService _transactionService = transactionService;
        private readonly IStaffBookingQueryService _bookingService = bookingService;


        private readonly IVNPayService _vNPayService = vNPayService;
        private readonly IMomoService _momoService = momoService;

        [BindProperty(SupportsGet = true)]
        public int SessionId { get; set; }

        [BindProperty]
        public TransactionDTO Transaction { get; set; } = default!;

        // thông tin để hiển thị sơ bộ
        public string? StationName { get; private set; }
        public string? StationLocation { get; private set; }
        public string? PortType { get; private set; }
        public int? PowerLevelKW { get; private set; }
        public int? ChargingSpeedKW { get; private set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (SessionId <= 0) return RedirectToPage("/Staff/Booking/Index");

            if (Transaction == null)
            {
                Transaction = new TransactionDTO();
            }

            var (ok, msg, dto) = await _svc.GetSessionAsync(SessionId);
            if (!ok || dto == null)
            {
                TempData["Error"] = msg;
                return RedirectToPage("/Staff/Booking/Index");
            }

            StationName = dto.StationName;
            StationLocation = dto.StationLocation;
            PortType = dto.PortType;
            PowerLevelKW = dto.PowerLevelKW;
            ChargingSpeedKW = dto.ChargingSpeedKW;

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            var session = await _chargingSessionService.GetByIdAsync(Transaction.SessionId);
            if (session == null) return Page(); ;

            Transaction.BookingId = session.BookingId;
            var transaction = await _transactionService.CreateAsync(Transaction);
            if (transaction == null) return Page();

            await _bookingService.Update(new BookingDetailDTO()
            {
                BookingId = transaction.BookingId,
                Price = transaction.Total,
                Status = transaction.PaymentMethod.Equals("CASH") ? "success" : "ongoing"
            });

            string url = "";
            switch (Transaction.PaymentMethod)
            {
                case "MOMO":
                    var response = await _momoService.CreatePaymentAsync(transaction.Id, transaction.Total);
                    if (response != null)
                    {
                        url = response.PayUrl ?? "";
                    }
                    break;
                case "VNPAY":
                    url = _vNPayService.CreateVNPayUrl(transaction.Id, transaction.Total);
                    break;
                default:
                    url = "";
                    break;
            }

            return string.IsNullOrEmpty(url.Trim())
                ? RedirectToPage("/Staff/Payment/CashRedirect", "OnGet", new
                {
                    orderId = transaction.Id,
                    resultCode = 1
                })
                : Redirect(url);
        }
    }
}

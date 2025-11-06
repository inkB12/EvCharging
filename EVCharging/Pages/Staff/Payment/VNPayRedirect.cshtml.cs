using EVCharging.BLL.DTO;
using EVCharging.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVCharging.Pages.Staff.Payment
{
    public class VNPayRedirectModel(IStaffBookingQueryService bookingService, ITransactionService transactionService) : PageModel
    {
        private readonly IStaffBookingQueryService _bookingService = bookingService;
        private readonly ITransactionService _transactionService = transactionService;

        [BindProperty]
        public string PaymentMessage { get; set; }
        [BindProperty]
        public int OrderId { get; set; }
        [BindProperty]
        public string ResultCode { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            var response = HttpContext.Request.Query;
            string status = "failed";

            // Validation
            if (response.Count == 0)
            {
                PaymentMessage = "Không tìm thấy dữ liệu phản hồi từ VNPay.";
                return Page();
            }

            ResultCode = response["vnp_ResponseCode"];

            string orderId = response["vnp_OrderInfo"];
            string subStringId = orderId[35..];

            if (ResultCode == "00")
            {
                // Succesful Payment
                PaymentMessage = "Giao dịch thanh toán thành công";
                status = "success";
            }
            else
            {
                // Fail Payment
                PaymentMessage = "Giao dịch thanh toán thất bại";
            }

            if (int.TryParse(subStringId, out int id))
            {
                // Update Transaction
                var transaction = await _transactionService.UpdateAsync(new TransactionDTO
                {
                    Id = id,
                    Status = status
                });

                if (transaction != null)
                {
                    // Update Booking
                    await _bookingService.Update(new BookingDetailDTO()
                    {
                        BookingId = transaction.BookingId,
                        Status = status == "failed" ? "cancelled" : status
                    });
                }
                OrderId = id;
            }
            else
            {
                PaymentMessage = "Giao dịch thanh toán thất bại do mã giao dịch lỗi";
            }

            return Page();
        }
    }
}

using EVCharging.BLL.DTO;
using EVCharging.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVCharging.Pages.Staff.Payment
{
    public class MomoRedirectModel(IStaffBookingQueryService bookingService, ITransactionService transactionService) : PageModel
    {
        private readonly IStaffBookingQueryService _bookingService = bookingService;
        private readonly ITransactionService _transactionService = transactionService;

        [BindProperty]
        public string PaymentMessage { get; set; }
        [BindProperty]
        public int OrderId { get; set; }
        [BindProperty]
        public int ResultCode { get; set; }


        public async Task<IActionResult> OnGetAsync(int resultCode, string orderId)
        {
            string status = "failed";
            string subStringId = orderId[35..];
            ResultCode = resultCode;

            if (resultCode == 0)
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

                //if (transaction != null)
                //{
                //    // Update Booking
                //     _bookingService.Update(new BookingDetailDTO()
                //    {
                //        BookingId = transaction.BookingId,
                //        Status = status == "failed" ? "cancelled" : status
                //    });
                //}
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

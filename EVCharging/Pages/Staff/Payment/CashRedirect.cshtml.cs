using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVCharging.Pages.Staff.Payment
{
    public class CashRedirectModel : PageModel
    {
        public string PaymentMessage { get; set; }
        [BindProperty(SupportsGet = true)]
        public int OrderId { get; set; }
        [BindProperty(SupportsGet = true)]
        public int ResultCode { get; set; }

        public IActionResult OnGet(int orderId, int resultCode)
        {
            OrderId = orderId;
            ResultCode = resultCode;
            if (resultCode == 1)
            {
                PaymentMessage = "Giao dịch thanh toán thành công";
            }
            else
            {
                PaymentMessage = "Giao dịch thanh toán thất bại";
            }

            return Page();
        }
    }
}

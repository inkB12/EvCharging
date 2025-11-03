using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVCharging.Pages.Map
{
    public class IndexModel : PageModel
    {
        public (double lat, double lng) Center { get; private set; }

        public void OnGet()
        {
        }
    }

}

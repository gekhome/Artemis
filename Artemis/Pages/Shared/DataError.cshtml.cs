using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Artemis.Pages.Shared
{
    public class DataErrorModel : PageModel
    {
        [BindProperty]
        public string ErrorMessage { get; set; } = string.Empty;

        public void OnGet(string? message = null)
        {
            if (message != null)
            {
                ErrorMessage = message;
            }
        }
    }
}

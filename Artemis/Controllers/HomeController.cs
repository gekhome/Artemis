/* 
 * Default Controller
 */

using Microsoft.AspNetCore.Mvc.Filters;

namespace Artemis.Controllers
{
    [TypeFilter(typeof(ErrorHandlerFilter))]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Diagnostic
            CultureInfo uiCultureInfo = CultureInfo.CurrentUICulture;
            CultureInfo cultureInfo = CultureInfo.CurrentCulture;

            return View();
        }

        public IActionResult Privacy()
        {
            //throw new NotImplementedException();
            return View();
        }

        public IActionResult About()
        {
            //string task = "Notification test";
            //this.AddAlertSuccess($"{task} created successfully.");

            ViewData["Message"] = "Brief information about the application.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Contact information.";

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
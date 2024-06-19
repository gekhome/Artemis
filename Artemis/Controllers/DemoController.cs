/* 
 * ------------------------
 * Telerik Demo Controller
 * ------------------------
 */

using Microsoft.AspNetCore.Mvc.Filters;

namespace Artemis.Controllers
{
    public class DemoController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!string.IsNullOrEmpty(context.HttpContext.Request.Query["culture"]))
            {
                CultureInfo.CurrentCulture = CultureInfo.CurrentUICulture = new CultureInfo(context.HttpContext.Request.Query["culture"]!);
            }
            base.OnActionExecuting(context);
        }

        public IActionResult Index()
        {
            string encoded = WebUtility.HtmlEncode("<div><p>this is some text</p></div>");
            string decoded = WebUtility.HtmlDecode(encoded);

            //IFormFile ImageFile = HttpContext.Request.Form.Files[0];

            return View();
        }

        public ActionResult Orders_Read([DataSourceRequest] DataSourceRequest request)
        {
            var result = Enumerable.Range(0, 50).Select(i => new OrderViewModel
            {
                OrderID = i,
                Freight = i * 10,
                OrderDate = new DateTime(2016, 9, 15).AddDays(i % 7),
                ShipName = "ShipName " + i,
                ShipCity = "ShipCity " + i
            });

            return Json(result.ToDataSourceResult(request));
        }
    }
}

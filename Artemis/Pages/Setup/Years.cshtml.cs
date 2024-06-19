
namespace Artemis.Pages.Setup
{
    public class YearsModel : PageModel
    {
        private readonly IYearService yearService;

        public YearsModel(IYearService yearService)
        {
            this.yearService = yearService;
        }

        public void OnGet()
        {
        }

        public void OnGetInitialize()
        {
            yearService.Initialize();
        }

        public JsonResult OnPostRead([DataSourceRequest] DataSourceRequest request)
        {
            IEnumerable<Years> data = yearService.Read();

            return new JsonResult(data.ToDataSourceResult(request));
        }

        public JsonResult OnPostCreate([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<Years> data)
        {
            if (data != null && ModelState.IsValid)
            {
                yearService.AddRange(data);
            }
            return new JsonResult(data.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnPostUpdate([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<Years> data)
        {
            if (data != null && ModelState.IsValid)
            {
                yearService.UpdateRange(data);
            }
            return new JsonResult(data.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnPostDestroy([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<Years> data)
        {
            if (data.Any())
            {
                yearService.DeleteRange(data);
            }
            return new JsonResult(data.ToDataSourceResult(request, ModelState));
        }

    }
}

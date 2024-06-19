
namespace Artemis.Pages.Setup
{
    public class LanguagesModel : PageModel
    {
        private readonly ILanguageService languageService;

        public LanguagesModel(ILanguageService languageService)
        {
            this.languageService = languageService;
        }

        public void OnGet()
        {
        }

        public JsonResult OnPostRead([DataSourceRequest] DataSourceRequest request)
        {
            IEnumerable<Languages> data = languageService.Read();

            return new JsonResult(data.ToDataSourceResult(request));
        }

        public JsonResult OnPostCreate([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<Languages> data)
        {
            if (data != null && ModelState.IsValid)
            {
                languageService.AddRange(data);
            }
            return new JsonResult(data.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnPostUpdate([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<Languages> data)
        {
            if (data != null && ModelState.IsValid)
            {
                languageService.UpdateRange(data);
            }
            return new JsonResult(data.ToDataSourceResult(request, ModelState));
        }

        public JsonResult OnPostDestroy([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<Languages> data)
        {
            if (data.Any())
            {
                languageService.DeleteRange(data);
            }
            return new JsonResult(data.ToDataSourceResult(request, ModelState));
        }
    }
}

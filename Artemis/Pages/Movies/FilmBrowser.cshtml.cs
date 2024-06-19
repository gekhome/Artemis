using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Artemis.Pages.Movies
{
    public class FilmBrowserModel : PageModel
    {
        private readonly IQueryService queryService;

        public FilmBrowserModel(IQueryService queryService)
        {
            this.queryService = queryService;
        }

        public void OnGet()
        {
        }

        public JsonResult OnPostRead([DataSourceRequest] DataSourceRequest request)
        {
            List<FilmInfoViewModel> data = queryService.GetFilms();

            return new JsonResult(data.ToDataSourceResult(request));
        }
    }
}

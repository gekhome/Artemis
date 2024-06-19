using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Artemis.Pages.Movies
{
    public class FilmActorsModel : PageModel
    {
        private readonly IQueryService queryService;

        public FilmActorsModel(IQueryService queryService)
        {
            this.queryService = queryService;
        }

        public void OnGet()
        {
        }

        public JsonResult OnPostFilms_Read([DataSourceRequest] DataSourceRequest request)
        {
            List<FilmInfoViewModel> data = queryService.GetFilms();

            return new JsonResult(data.ToDataSourceResult(request));
        }

        public JsonResult OnPostFilmActors_Read([DataSourceRequest] DataSourceRequest request, int filmId)
        {
            List<FilmActorsViewModel> data = queryService.GetFilmActors(filmId);

            return new JsonResult(data.ToDataSourceResult(request));
        }
    }
}

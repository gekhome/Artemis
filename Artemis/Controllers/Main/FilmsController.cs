using Microsoft.AspNetCore.Mvc;

namespace Artemis.Controllers.Data
{
    public class FilmsController : Controller
    {
        private readonly ArtemisDbContext db;
        private readonly IQueryService queryService;

        public FilmsController(ArtemisDbContext context, IQueryService queryService)
        {
            db = context;
            this.queryService = queryService;
        }

        public ActionResult FilmInfo_Read([DataSourceRequest] DataSourceRequest request)
        {
            List<FilmInfoViewModel> data = queryService.GetFilms();

            return Json(data.ToDataSourceResult(request));
        }

        public ActionResult FilmActors_Read([DataSourceRequest] DataSourceRequest request, int filmId)
        {
            List<FilmActorsViewModel> data = queryService.GetFilmActors(filmId);

            return Json(data.ToDataSourceResult(request));
        }

    }
}

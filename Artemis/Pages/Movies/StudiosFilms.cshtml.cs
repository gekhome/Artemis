using Artemis.Dal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Artemis.Pages.Movies
{
    public class StudiosFilmsModel : PageModel
    {
        private readonly IQueryService queryService;

        public StudiosFilmsModel(IQueryService queryService)
        {
            this.queryService = queryService;
        }

        public void OnGet()
        {
        }

        public JsonResult OnPostStudios_Read([DataSourceRequest] DataSourceRequest request)
        {
            IEnumerable<StudiosInFilms> data = queryService.GetStudiosInFilms();

            return new JsonResult(data.ToDataSourceResult(request));
        }

        public JsonResult OnPostFilms_Read([DataSourceRequest] DataSourceRequest request, int studioId)
        {
            IEnumerable<FilmViewModel> data = queryService.GetStudioFilms(studioId);

            return new JsonResult(data.ToDataSourceResult(request));
        }

    }
}

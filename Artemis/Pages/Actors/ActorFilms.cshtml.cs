using Artemis.Dal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Artemis.Pages.Actors
{
    public class ActorFilmsModel : PageModel
    {
        private readonly IQueryService queryService;

        public ActorFilmsModel(IQueryService queryService)
        {
            this.queryService = queryService;
        }

        public void OnGet()
        {
        }

        public JsonResult OnPostActors_Read([DataSourceRequest] DataSourceRequest request)
        {
            IEnumerable<ActorInfoViewModel> data = queryService.GetActors();

            return new JsonResult(data.ToDataSourceResult(request));
        }

        public JsonResult OnPostActorFilms_Read([DataSourceRequest] DataSourceRequest request, int actorId)
        {
            IEnumerable<ActorFilmViewModel> data = queryService.GetActorFilms(actorId);

            return new JsonResult(data.ToDataSourceResult(request));
        }
    }
}

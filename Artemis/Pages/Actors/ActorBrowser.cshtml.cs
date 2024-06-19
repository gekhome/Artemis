using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Artemis.Pages.Actors
{
    public class ActorBrowserModel : PageModel
    {
        private readonly ArtemisDbContext db;
        private readonly IQueryService queryService;

        public ActorBrowserModel(ArtemisDbContext context, IQueryService queryService)
        {
            db = context;
            this.queryService = queryService;
        }

        public void OnGet()
        {
        }

        public JsonResult OnPostRead([DataSourceRequest] DataSourceRequest request)
        {
            List<ActorInfoViewModel> actors = queryService.GetActors();

            return new JsonResult(actors.ToDataSourceResult(request));
        }
    }
}

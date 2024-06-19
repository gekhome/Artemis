using Artemis.Dal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Artemis.Pages.Actors
{
    public class ActorStatisticsModel : PageModel
    {
        private readonly IChartRepository chartRepository;

        public ActorStatisticsModel(IChartRepository chartRepository)
        {
            this.chartRepository = chartRepository;
        }

        public void OnGet()
        {
        }

        public JsonResult OnPostRegions_Read([DataSourceRequest] DataSourceRequest request)
        {
            List<ActorCountRegions> data = chartRepository.GetActorRegions();

            return new JsonResult(data.ToDataSourceResult(request));
        }

        public JsonResult OnPostRatings_Read([DataSourceRequest] DataSourceRequest request)
        {
            List<ActorRatingCount> data = chartRepository.GetActorRatings();

            return new JsonResult(data.ToDataSourceResult(request));
        }

        public JsonResult OnPostGenders_Read([DataSourceRequest] DataSourceRequest request)
        {
            List<ActorGenderCount> data = chartRepository.GetActorGenders();

            return new JsonResult(data.ToDataSourceResult(request));
        }

    }
}

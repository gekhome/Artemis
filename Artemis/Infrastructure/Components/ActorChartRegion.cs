using Artemis.Dal.Data;

namespace Artemis.Infrastructure.Components
{
    public class ActorChartRegion : ViewComponent
    {
        private readonly ArtemisDbContext context;
        private readonly IChartRepository chartRepository;

        public ActorChartRegion(ArtemisDbContext context, IChartRepository chartRepository)
        {
            this.context = context;
            this.chartRepository = chartRepository;
        }

        public IViewComponentResult Invoke(string width = "300", string height = "300")
        {
            ViewBag.Width = width;
            ViewBag.Height = height;

            return View(ActorRegionData());
        }

        public IEnumerable<ActorRegionViewModel> ActorRegionData()
        {
            string[] colors = new string[] { "#9de219", "#90cc38", "#068c35", "#006634", "#004d38", "#033939", "#42a7ff", "#666666" };

            List<ActorRegionViewModel> model = new();

            List<ActorCountRegions> data = chartRepository.GetActorRegions();
            
            int i = 0;
            foreach (var item  in data) 
            {
                model.Add(new ActorRegionViewModel(item.Region, Math.Round((decimal)item.Percentage! * 100.0m, 2), colors[i]));
                i++;
            }
            model[0].Explode = true;

            return model;
        }
    }
}

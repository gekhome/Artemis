using Artemis.Dal.Data;

namespace Artemis.Infrastructure.Components
{
    public class ActorChartRating : ViewComponent
    {
        private readonly ArtemisDbContext context;
        private readonly IChartRepository chartRepository;

        public ActorChartRating(ArtemisDbContext context, IChartRepository chartRepository)
        {
            this.context = context;
            this.chartRepository = chartRepository;
        }

        public IViewComponentResult Invoke(string width = "300", string height = "300")
        {
            ViewBag.Width = width;
            ViewBag.Height = height;

            return View(ActorRatingData());
        }

        public IEnumerable<ActorRatingViewModel> ActorRatingData()
        {
            string[] colors = new string[] { "#03a9f4", "#ff9800", "#4caf50" };

            List<ActorRatingViewModel> model = new();

            List<ActorRatingCount> data = chartRepository.GetActorRatings();
            
            int i = 0;
            foreach (var item  in data) 
            {
                model.Add(new ActorRatingViewModel(item.Rating, Math.Round((decimal)item.Percentage! * 100.0m, 2), colors[i]));
                i++;
            }

            return model;
        }
    }
}

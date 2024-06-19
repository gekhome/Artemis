using Artemis.Dal.Data;

namespace Artemis.Infrastructure.Components
{
    public class ActorChartGender : ViewComponent
    {
        private readonly ArtemisDbContext context;
        private readonly IChartRepository chartRepository;

        public ActorChartGender(ArtemisDbContext context, IChartRepository chartRepository)
        {
            this.context = context;
            this.chartRepository = chartRepository;
        }

        public IViewComponentResult Invoke(string width = "300", string height = "300")
        {
            ViewBag.Width = width;
            ViewBag.Height = height;

            return View(ActorGenderData());
        }

        public IEnumerable<ActorGenderViewModel> ActorGenderData()
        {
            string[] colors = new string[] { "#03a9f4", "#ff0066" };

            List<ActorGenderViewModel> model = new();

            List<ActorGenderCount> data = chartRepository.GetActorGenders();

            int i = 0;
            foreach (var item  in data) 
            {
                model.Add(new ActorGenderViewModel(item.Gender, Math.Round((decimal)item.Percentage! * 100.0m, 2), colors[i]));
                i++;
            }

            return model;
        }
    }
}

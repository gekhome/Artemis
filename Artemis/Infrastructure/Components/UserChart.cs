namespace Artemis.Infrastructure.Components
{
    public class UserChart : ViewComponent
    {
        private readonly ApplicationDbContext context;

        public UserChart(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IViewComponentResult Invoke(string width = "200", string height = "200")
        {
            ViewBag.Width = width;
            ViewBag.Height = height;

            return View(UserCountBreakdown());
        }

        private IEnumerable<UserChartModel> UserCountBreakdown()
        {
            string[] colors = new string[] { "#9de219", "#90cc38", "#068c35" };
            UserChartModel[] model = new UserChartModel[3];

            decimal totalUsers = context.UserRoles.Count();
            decimal basicUsers = context.UserRoles.Include(x => x.Role).Count(x => x.Role!.Name == RolesEnum.Basic.ToString());
            decimal administrators = context.UserRoles.Include(x => x.Role).Count(x => x.Role!.Name == RolesEnum.Admin.ToString());
            decimal moderators = context.UserRoles.Include(x => x.Role).Count(x => x.Role!.Name == RolesEnum.Moderator.ToString());

            decimal basicPercent = Math.Round(basicUsers / totalUsers * 100.0m, 0, MidpointRounding.AwayFromZero);
            decimal moderatorPercent = Math.Round(moderators / totalUsers * 100.0m, 0, MidpointRounding.AwayFromZero);
            decimal adminPercent = Math.Round(administrators / totalUsers * 100.0m, 0, MidpointRounding.AwayFromZero);


            model[0] = new UserChartModel(RolesEnum.Basic.ToString(), basicPercent, colors[0]);
            model[1] = new UserChartModel(RolesEnum.Moderator.ToString(), moderatorPercent, colors[1]);
            model[2] = new UserChartModel(RolesEnum.Admin.ToString(), adminPercent, colors[2]);

            return model;
        }

    }
}


namespace Artemis.Infrastructure.Components
{
    public class UserSummary : ViewComponent
    {
        private readonly ApplicationDbContext context;

        public UserSummary(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IViewComponentResult Invoke(string themeName = "secondary")
        {
            ViewBag.themeName = themeName;

            UserSummaryViewModel userSummary = new()
            {
                TotalUsers = context.Users.Count(),
                BasicUsers = context.UserRoles.Include(x => x.Role).Count(x => x.Role!.Name == RolesEnum.Basic.ToString()),
                Administrators = context.UserRoles.Include(x => x.Role).Count(x => x.Role!.Name == RolesEnum.Admin.ToString()),
                Moderators = context.UserRoles.Include(x => x.Role).Count(x => x.Role!.Name == RolesEnum.Moderator.ToString()),
            };

            return View(userSummary);
        }
    }
}

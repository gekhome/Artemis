
namespace Artemis.Controllers.Accounts
{
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly RoleManager<ApplicationRole> roleManager;

        public RolesController(ApplicationDbContext context, RoleManager<ApplicationRole> roleManager)
        {
            this.context = context;
            this.roleManager = roleManager;
        }

        public IActionResult ListRoles()
        {
            return View("~/Views/Accounts/ListRoles.cshtml");
        }

        #region master grid with Roles

        public async Task<ActionResult> Role_Read([DataSourceRequest] DataSourceRequest request)
        {
            List<RoleViewModel> data = await GetRoles();
            return Json(data.ToDataSourceResult(request));
        }

        [AcceptVerbs("Post")]
        public async Task<ActionResult> Role_Create([DataSourceRequest] DataSourceRequest request, RoleViewModel data)
        {
            if (data?.RoleName != null)
            {
                if (RoleExists(data.RoleName))
                {
                    ModelState.AddModelError(string.Empty, "This Role already exists. Operation cancelled.");
                }

                if (ModelState.IsValid)
                {
                    ApplicationRole entity = new()
                    {
                        Name = data.RoleName,
                    };
                    IdentityResult result = await roleManager.CreateAsync(entity);
                    if (!result.Succeeded)
                    {
                        Errors(result);
                    }
                    data.RoleId = entity.Id;
                    data.RoleNameNormalized = entity.NormalizedName;
                }
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs("Post")]
        public async Task<ActionResult> Role_Update([DataSourceRequest] DataSourceRequest request, RoleViewModel data)
        {
            if (data != null && ModelState.IsValid)
            {
                ApplicationRole? entity = await roleManager.FindByIdAsync(data.RoleId!);
                if (entity != null)
                {
                    entity.Name = data.RoleName;
                    IdentityResult result = await roleManager.UpdateAsync(entity);
                    if (!result.Succeeded) { Errors(result); }
                    data.RoleNameNormalized = entity.NormalizedName;
                }
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState));
        }

        public async Task<ActionResult> Role_Destroy([DataSourceRequest] DataSourceRequest request, RoleViewModel data)
        {
            if (data != null)
            {
                ApplicationRole? entity = await roleManager.FindByIdAsync(data.RoleId!);
                if (entity != null)
                {
                    if (CanDeleteRole(entity.Id))
                    {
                        IdentityResult result = await roleManager.DeleteAsync(entity);
                        if (!result.Succeeded) { Errors(result); }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Cannot delete Role. Users exist with this Role.");
                    }
                }
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState));
        }

        #endregion

        #region child grid with users

        public async Task<ActionResult> Users_Read([DataSourceRequest] DataSourceRequest request, string roleId)
        {
            var data = await GetUsersByRoleId(roleId);

            return Json(data.ToDataSourceResult(request));
        }

        #endregion

        public async Task<List<RoleViewModel>> GetRoles()
        {
            List<RoleViewModel> roles = await roleManager.Roles.Select(r => new RoleViewModel
            {
                RoleId = r.Id,
                RoleName = r.Name,
                RoleNameNormalized = r.NormalizedName
            }).OrderBy(x => x.RoleName).ToListAsync();

            return roles;
        }

        public async Task<RoleViewModel> GetRoleById(string roleId)
        {
            RoleViewModel? role = await roleManager.Roles.Select(r => new RoleViewModel
            {
                RoleId = r.Id,
                RoleName = r.Name,
                RoleNameNormalized = r.NormalizedName
            }).FirstOrDefaultAsync(r => r.RoleId == roleId);

            if (role == null)
            {
                return new RoleViewModel();
            }
            return role;
        }

        public async Task<List<ApplicationUser>> GetUsersByRoleId(string roleId)
        {
            // The first step: get all user id collection as userids based on role from UserRoles
            List<string> userids = await context.UserRoles.Where(a => a.RoleId == roleId).Select(b => b.UserId).Distinct().ToListAsync();
            // The second step : find all users collection from Users whose Id is contained at userids
            List<ApplicationUser> listUsers = await context.Users.Where(a => userids.Any(c => c == a.Id)).ToListAsync();

            return listUsers;
        }

        public List<ApplicationUser> GetUsersInRole(string roleId)
        {
            // The first step: get all user id collection as userids based on role from UserRoles
            List<string> userids = context.UserRoles.Where(a => a.RoleId == roleId).Select(b => b.UserId).Distinct().ToList();
            // The second step : find all users collection from Users whose Id is contained at userids
            List<ApplicationUser> listUsers = context.Users.Where(a => userids.Any(c => c == a.Id)).ToList();

            return listUsers;
        }

        public bool RoleExists(string roleName)
        {
            IdentityRole? role = context.Roles.FirstOrDefault(x => x.NormalizedName == roleName.ToUpper());
            if (role == null)
            {
                return false;
            }
            return true;

        }

        public bool CanDeleteRole(string roleId)
        {
            bool usersExist = GetUsersInRole(roleId).Any();
            if (usersExist)
            {
                return false;
            }
            return true;
        }

        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}

/* 
 * Controller for managing users, roles and claims
 */

namespace Artemis.Controllers.Accounts
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly IPasswordHasher<ApplicationUser> passwordHasher;

        public UsersController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IPasswordHasher<ApplicationUser> passwordHasher)
        {
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.passwordHasher = passwordHasher;
        }

        public IActionResult UsersRoles()
        {
            PopulateRoles();
            return View("~/Views/Accounts/UsersRoles.cshtml");
        }

        #region master grid with users

        public async Task<ActionResult> User_Read([DataSourceRequest] DataSourceRequest request)
        {
            List<AppUserViewModel> users = await GetUsers();
            return Json(users.ToDataSourceResult(request));
        }

        [AcceptVerbs("Post")]
        public async Task<ActionResult> User_Create([DataSourceRequest] DataSourceRequest request, AppUserViewModel data)
        {
            AppUserViewModel newdata = new();

            if (data != null && ModelState.IsValid)
            {
                ApplicationUser? entity = new()
                {
                    UserName = data.UserName,
                    FirstName = data.FirstName,
                    LastName = data.LastName,
                    Email = data.Email,
                    Address = data.Address,
                    Country = data.Country?.ToUpper(),
                    BirthDate = data.BirthDate,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    UsernameChangeLimit = 10
                };
                IdentityResult result = await userManager.CreateAsync(entity, data.Password);
                if (!result.Succeeded)
                {
                    Errors(result);
                    return Json(new[] { data }.ToDataSourceResult(request, ModelState));
                }
                var claim = new Claim("FullName", entity.FirstName + " " + entity.LastName);
                await userManager.AddClaimAsync(entity, claim);

                data.Id = entity.Id;
                newdata = await GetUser(data.Id);
            }
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs("Post")]
        public async Task<ActionResult> User_Update([DataSourceRequest] DataSourceRequest request, AppUserViewModel data)
        {
            if (data != null && ModelState.IsValid)
            {
                ApplicationUser? entity = await userManager.FindByIdAsync(data.Id!);
                if (entity != null)
                {
                    entity.UserName = data.UserName;
                    entity.FirstName = data.FirstName;
                    entity.LastName = data.LastName;
                    entity.Email = data.Email;
                    entity.Address = data.Address;
                    entity.Country = data.Country?.ToUpper();
                    entity.BirthDate = data.BirthDate;
                    entity.EmailConfirmed = true;
                    entity.PhoneNumberConfirmed = true;
                    entity.PasswordHash = passwordHasher.HashPassword(entity, data.Password);

                    IdentityResult result = await userManager.UpdateAsync(entity);
                    if (!result.Succeeded)
                    {
                        Errors(result);
                    }
                    AppUserViewModel newData = await GetUser(entity.Id);
                    return Json(new[] { newData }.ToDataSourceResult(request, ModelState));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "The requested user was not found!");
                }
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs("Post")]
        public async Task<ActionResult> User_Destroy([DataSourceRequest] DataSourceRequest request, AppUserViewModel data)
        {
            if (data != null)
            {
                ApplicationUser? entity = await userManager.FindByIdAsync(data.Id!);
                if (entity != null)
                {
                    IdentityResult result = await userManager.DeleteAsync(entity);
                    if (!result.Succeeded)
                    {
                        Errors(result);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "The requested user was not found!");
                }
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState));
        }

        #endregion

        #region child grid with roles

        public async Task<ActionResult> UserRole_Read([DataSourceRequest] DataSourceRequest request, string userId)
        {
            var data = await GetRolesByUser(userId);

            return Json(data.ToDataSourceResult(request));
        }

        [AcceptVerbs("Post")]
        public async Task<ActionResult> UserRole_Create([DataSourceRequest] DataSourceRequest request, UserRoleViewModel data, string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                ModelState.AddModelError(string.Empty, "You must first select a user. Operation aborted.");
            }
            if (data.RoleId != null && ModelState.IsValid)
            {
                if(UserRoleExists(userId, data.RoleId))
                {
                    ModelState.AddModelError(string.Empty, "The Role already exists for the selected user.");
                    return Json(new[] { data }.ToDataSourceResult(request, ModelState));
                }
                ApplicationUserRole entity = new()
                {
                    RoleId = data.RoleId,
                    UserId = userId
                };
                context.UserRoles.Add(entity);
                await context.SaveChangesAsync();

                data.Key = entity.UserId + "|" + entity.RoleId;
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs("Post")]
        public async Task<ActionResult> UserRole_Update([DataSourceRequest] DataSourceRequest request, UserRoleViewModel data, string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                ModelState.AddModelError(string.Empty, "You must first select a user. Operation aborted.");
            }

            ApplicationUser? user = await userManager.FindByIdAsync(userId);
            var newrole = await roleManager.FindByIdAsync(data.RoleId!);
            // find the role(s) of the selected user
            var currentRoles = await userManager.GetRolesAsync(user!);
            // clear all current roles
            foreach (var role in currentRoles)
            {
                await userManager.RemoveFromRoleAsync(user!, role);
            }
            // add the new role to the selected user
            await userManager.AddToRoleAsync(user!, newrole?.Name!);

            UserRoleViewModel newdata = await GetUserRole(data.UserId!, data.RoleId!);
            return Json(new[] { newdata }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs("Post")]
        public async Task<ActionResult> UserRole_Destroy([DataSourceRequest] DataSourceRequest request, UserRoleViewModel data)
        {
            if (data != null)
            {
                ApplicationUserRole? entity = context.UserRoles.Find(data.UserId, data.RoleId);

                if (entity != null)
                {
                    context.UserRoles.Remove(entity);
                    await context.SaveChangesAsync();
                }
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState));
        }

        #endregion


        #region browse users in ListView

        public ActionResult BrowseUsers()
        {
            return View("~/Views/Accounts/BrowseUsers.cshtml");
        }

        public async Task<ActionResult> Users_Read([DataSourceRequest] DataSourceRequest request)
        {
            List<AppUserViewModel> data = await GetUsers();

            return Json(data.ToDataSourceResult(request));
        }

        #endregion

        #region getters and populators

        public async Task<List<AppUserViewModel>> GetUsers()
        {
            var data = await userManager.Users.Select(d => new AppUserViewModel
            {
                Id = d.Id,
                UserName = d.UserName ?? string.Empty,
                Email = d.Email ?? string.Empty,
                FirstName = d.FirstName,
                LastName = d.LastName,
                BirthDate = d.BirthDate,
                PhoneNumber = d.PhoneNumber ?? "Unknown",
                Address = d.Address ?? "Unkown",
                Country = d.Country ?? "Unknown",
                ProfileImage = d.ProfilePicture == null ? Array.Empty<byte>().ToString() : Convert.ToBase64String(d.ProfilePicture)
            }).OrderBy(x => x.UserName).ToListAsync();

            return data;
        }

        public async Task<AppUserViewModel> GetUser(string userId)
        {
            AppUserViewModel? data = await userManager.Users.Select(d => new AppUserViewModel
            {
                Id = d.Id,
                UserName = d.UserName ?? string.Empty,
                FirstName = d.FirstName,
                LastName = d.LastName,
                Email = d.Email ?? string.Empty,
                BirthDate = d.BirthDate,
                Address = d.Address,
                Country = d.Country,
            }).FirstOrDefaultAsync(d => d.Id == userId);

            return data!;
        }

        public Task<List<UserRoleViewModel>> GetRolesByUser(string userId)
        {
            var userRoles = context.UserRoles.Select(d => new UserRoleViewModel
            {
                // Kendo grid does not support composite key.
                // It must be artificially constructed
                Key = d.UserId + "|" + d.RoleId,
                UserId = d.UserId,
                RoleId = d.RoleId
            }).Where(d => d.UserId == userId).ToListAsync();

            return userRoles;
        }

        public Task<UserRoleViewModel> GetUserRole(string userId, string roleId)
        {
            var userRole = context.UserRoles.Select(d => new UserRoleViewModel
            {
                Key = d.UserId + "|" + d.RoleId,
                UserId = d.UserId,
                RoleId = d.RoleId
            }).FirstOrDefaultAsync(x => x.UserId == userId && x.RoleId == roleId);

            return userRole!;
        }

        public void PopulateRoles()
        {
            List<ApplicationRole> data = roleManager.Roles.ToList();

            ViewData["roles"] = data;
        }

        #endregion

        #region custom functions

        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        public bool UserRoleExists(string userId, string roleId)
        {
            var data = context.UserRoles.FirstOrDefault(x => x.UserId == userId && x.RoleId == roleId);
            if (data != null)
            {
                return true;
            }
            return false;
        }

        public void FixClaimAndGhostUser(string userId, Claim claim)
        {
            // The order of the following steps is important!
            // First, there is a ghost user created in Users, so get the 'ghost user' Id
            ApplicationUser? ghostUser = context.Users.Where(u => u.UserName == null).FirstOrDefault();
            string? ghostUserId = ghostUser?.Id;

            ApplicationUserClaim? userClaim = context.UserClaims.FirstOrDefault(c => c.UserId == ghostUserId && c.ClaimType == claim.Type);
            // Secondly, change the UserId in Claims to the value of the selected user (userId)
            if (userClaim != null)
            {
                userClaim.UserId = userId;
                context.UserClaims.Update(userClaim);
                context.SaveChanges();
            }
            // Finally, delete the 'ghost user' from Users
            if (ghostUser != null)
            {
                userManager.DeleteAsync(ghostUser);
            }
        }

        #endregion
    }
}


namespace Artemis.Controllers.Accounts
{
    [Authorize(Roles = "Admin")]
    public class ClaimsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public ClaimsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public IActionResult ListClaims()
        {
            return View("~/Views/Accounts/ListClaims.cshtml");
        }

        public async Task<ActionResult> Claims_Read([DataSourceRequest] DataSourceRequest request, string userId)
        {
            List<UserClaimViewModel> data = await GetUserClaims(userId);

            return Json(data.ToDataSourceResult(request));
        }

        public async Task<ActionResult> Claims_Create([DataSourceRequest] DataSourceRequest request, UserClaimViewModel data, string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                ModelState.AddModelError(string.Empty, "You must select a user first. Operation cancelled.");
                return Json(new[] { data }.ToDataSourceResult(request, ModelState));
            }
            ApplicationUser? user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "The user was not found. Operation cancelled.");
                return Json(new[] { data }.ToDataSourceResult(request, ModelState));
            }

            if (await HasClaimType(user!, data.ClaimType!))
            {
                ModelState.AddModelError(string.Empty, "User already has the specified Claim");
            }

            if (data.ClaimType != null && data.ClaimValue != null && ModelState.IsValid)
            {
                Claim claim = new(data.ClaimType, data.ClaimValue, ClaimValueTypes.String);
                await userManager.AddClaimAsync(user, claim);

                // get the new Id for the grid
                data.Id = (int)context.UserClaims.FirstOrDefault(c => c.UserId == userId && c.ClaimType == claim.Type)?.Id!;
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState));
        }

        public async Task<ActionResult> Claims_Update([DataSourceRequest] DataSourceRequest request, UserClaimViewModel data, string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                ModelState.AddModelError(string.Empty, "You must select a user first. Operation cancelled.");
                return Json(new[] { data }.ToDataSourceResult(request, ModelState));
            }
            ApplicationUserClaim? entity = context.UserClaims.Find(data.Id);
            if (entity != null)
            {
                entity.ClaimType = data.ClaimType;
                entity.ClaimValue = data.ClaimValue;

                context.UserClaims.Update(entity);
                await context.SaveChangesAsync();
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState));
        }

        public async Task<ActionResult> Claims_Destroy([DataSourceRequest] DataSourceRequest request, UserClaimViewModel data)
        {
            if (data != null)
            {
                ApplicationUserClaim? entity = context.UserClaims.Find(data.Id);
                if (entity != null)
                {
                    context.UserClaims.Remove(entity);
                    await context.SaveChangesAsync();
                }
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState));
        }

        public async Task<List<UserClaimViewModel>> GetUserClaims(string userId)
        {
            List<UserClaimViewModel> data = await context.UserClaims.Select(c => new UserClaimViewModel
            {
                Id = c.Id,
                UserId = c.UserId,
                ClaimType = c.ClaimType,
                ClaimValue = c.ClaimValue
            }).Where(x => x.UserId == userId).ToListAsync();

            return data;
        }

        public async Task<bool> HasClaimType(ApplicationUser user, string claimType)
        {
            var existingUserClaims = await userManager.GetClaimsAsync(user);

            foreach (Claim existingClaim in existingUserClaims)
            {
                if (existingUserClaims.Any(c => c.Type.ToUpper() == claimType.ToUpper()))
                {
                    return true;
                }
            }
            return false;
        }

        public void FixClaimAndGhostUser(string userId, Claim claim)
        {
            // Ghost user was a side effect of initializing User in custom ApplicationUser classes

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

        public void PopulateUsers()
        {
            List<ApplicationUser> data = userManager.Users.OrderBy(x => x.UserName).ToList();

            ViewData["users"] = data;
        }
    }
}

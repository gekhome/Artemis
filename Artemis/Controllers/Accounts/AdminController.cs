using Microsoft.AspNetCore.Mvc;

namespace Artemis.Controllers.Accounts
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly IPasswordHasher<ApplicationUser> passwordHasher;

        public AdminController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IPasswordHasher<ApplicationUser> passwordHasher)
        {
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.passwordHasher = passwordHasher;
        }

        #region Getters

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
                Address = d.Address,
                Country = d.Country,
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


        #endregion

        #region Populators

        public void PopulateUsers()
        {
            List<ApplicationUser> data = userManager.Users.OrderBy(x => x.UserName).ToList();

            ViewData["users"] = data;
        }

        public void PopulateRoles()
        {
            List<ApplicationRole> data = roleManager.Roles.ToList();

            ViewData["roles"] = data;
        }

        #endregion


        #region Custom functions

        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
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

        public bool UserRoleExists(string userId, string roleId)
        {
            var data = context.UserRoles.FirstOrDefault(x => x.UserId == userId && x.RoleId == roleId);
            if (data != null)
            {
                return true;
            }
            return false;
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

        // No longer required
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
        // No longer required
        public void FixRoleAndGhostUser(string userId, string roleId)
        {
            // The order of the following steps is important!
            // First, there is a ghost user created in Users, so get the 'ghost user' Id
            ApplicationUser? ghostUser = context.Users.Where(u => u.UserName == null).FirstOrDefault();
            string? ghostUserId = ghostUser?.Id;

            ApplicationUserRole? userRole = context.UserRoles.FirstOrDefault(d => d.UserId == ghostUserId && d.RoleId == roleId);
            // Secondly, change the UserId in UserRoles to the value of the selected user (userId)
            if (userRole != null)
            {
                userRole.UserId = userId;
                context.UserRoles.Update(userRole);
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

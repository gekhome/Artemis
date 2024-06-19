using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data;

namespace Artemis.Data
{
    public class IdentitySeed
    {
        public static void CreateRoles(IServiceProvider serviceProvider)
        {
            // Important to create a scoped service, otherwise it crashes.
            serviceProvider = serviceProvider.CreateScope().ServiceProvider;
            SeedRolesAsync(serviceProvider).Wait();
        }

        public static async Task SeedRolesAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();

            ApplicationRole? roleAdmin = await roleManager.FindByNameAsync(RolesEnum.Admin.ToString());
            if (roleAdmin == null)
            {
                await roleManager.CreateAsync(new ApplicationRole(RolesEnum.Admin.ToString()));
            }
            ApplicationRole? roleModerator = await roleManager.FindByNameAsync(RolesEnum.Moderator.ToString());
            if (roleModerator == null)
            {
                await roleManager.CreateAsync(new ApplicationRole(RolesEnum.Moderator.ToString()));
            }
            ApplicationRole? roleBasic = await roleManager.FindByNameAsync(RolesEnum.Basic.ToString());
            if (roleBasic == null)
            {
                await roleManager.CreateAsync(new ApplicationRole(RolesEnum.Basic.ToString()));
            }
        }

        public static void CreateAdminAccount(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            CreateAdminAccountAsync(serviceProvider, configuration).Wait();
        }

        public static async Task CreateAdminAccountAsync(IServiceProvider services, IConfiguration configuration)
        {
            // Important to create a scoped service, otherwise it crashes.
            services = services.CreateScope().ServiceProvider;
            UserManager<ApplicationUser> userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

            string username = configuration["Data:AdminUser:Name"] ?? "admin";
            string email = configuration["Data:AdminUser:Email"] ?? "admin@example.com";
            string password = configuration["Data:AdminUser:Password"] ?? "gek-1711";
            string firstname = configuration["Data:AdminUser:FirstName"] ?? "George";
            string lastname = configuration["Data:AdminUser:LastName"] ?? "Kyriakakis";

            if (await userManager.FindByNameAsync(username) == null)
            {
                ApplicationUser defaultUser = new()
                {
                    UserName = username,
                    Email = email,
                    FirstName = firstname,
                    LastName = lastname,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                };

                IdentityResult result = await userManager.CreateAsync(defaultUser, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(defaultUser, RolesEnum.Admin.ToString());
                    await userManager.AddToRoleAsync(defaultUser, RolesEnum.Moderator.ToString());
                    await userManager.AddToRoleAsync(defaultUser, RolesEnum.Basic.ToString());
                }
            }
        }
    }
}

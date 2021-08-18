namespace Forum.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Forum.Common;
    using Forum.Data.Models;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    internal class RolesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            await SeedRoleAsync(roleManager, GlobalConstants.AdministratorRoleName);

            var email = "admin@forum.com";
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                var admin = new ApplicationUser
                {
                    Email = email,
                    UserName = email,
                };

                await userManager.CreateAsync(admin, "admin12");
                await userManager.AddToRoleAsync(admin, GlobalConstants.AdministratorRoleName);
            }
        }

        private static async Task SeedRoleAsync(RoleManager<ApplicationRole> roleManager, string roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                var result = await roleManager.CreateAsync(new ApplicationRole(roleName));
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}

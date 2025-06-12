using Core.Entities.UserEntity;
using Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Initializers
{
    public static class UsersAndRolesInitializer
    {
        public static async Task SeedUsersAndRoles(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateAsyncScope())
            {
                var context = serviceScope.ServiceProvider.GetService<DataBaseContext>();
                UserManager<User> userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync("Administrator"))
                {
                    await roleManager.CreateAsync(new IdentityRole
                    {
                        Name = "Administrator",
                        NormalizedName = "ADMINISTRATOR"
                    });
                }

                if (!await roleManager.RoleExistsAsync("User"))
                {
                    await roleManager.CreateAsync(new IdentityRole
                    {
                        Name = "User",
                        NormalizedName = "USER"
                    });
                }

                if (userManager.FindByEmailAsync("admin@ukr.net").Result == null)
                {
                    User admin = new User()
                    {
                        UserName = "admin@ukr.net",
                        FirstName = "Shop",
                        LastName = "Estro",
                        Email = "admin@ukr.net",
                        EmailConfirmed = true,
                        PhoneNumber = "0990000000",
                        PhoneNumberConfirmed = true,
                        AuthType = "standard",
                    };

                    IdentityResult adminResult = userManager.CreateAsync(admin, "Qwerty-7").Result;
                    if (adminResult.Succeeded)
                    {
                        userManager.AddToRoleAsync(admin, "Administrator").Wait();
                    }
                }
            }
        }
    }
}

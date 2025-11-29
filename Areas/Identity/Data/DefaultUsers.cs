using Microsoft.AspNetCore.Identity;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Base;

namespace asp_dot_net_core_web_app_mvc_fast_food_system.Areas.Identity.Data
{
    public static class DefaultUsers
    {
        public async static Task Initialize(IServiceProvider serviceProvider)
        {
            UserManager<SystemUser> userManager = serviceProvider.GetRequiredService<UserManager<SystemUser>>();

            RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roles = [ "Admin", "Staff" ];

            foreach (string role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            async Task CreateUser(string userName, string? userEmail, string userPassword, string role)
            {
                // Creating a SystemUser manually and assigning a password hash directly.
                // This bypasses Identity's password validation (useful for seeding users or testing purposes).
                //if (await userManager.FindByNameAsync(userName) == null && await userManager.FindByEmailAsync(userEmail) == null)
                //{
                //    SystemUser user = new SystemUser();
                //    user.UserName = userName;
                //    user.PasswordHash = new PasswordHasher<SystemUser>().HashPassword(user, userPassword);
                //    user.Cart = new Cart();

                //    IdentityResult result = await userManager.CreateAsync(user, userPassword);

                //    if (result.Succeeded)
                //    {
                //        if (roles.Contains(role))
                //        {
                //            await userManager.AddToRoleAsync(user, role);
                //        }
                //    }
                //}

                if (await userManager.FindByNameAsync(userName) == null && await userManager.FindByEmailAsync(userEmail) == null)
                {
                    SystemUser user = new SystemUser
                    {
                        UserName = userName,
                        Email = userEmail,
                        EmailConfirmed = true,
                        Cart = new Cart()
                    };

                    IdentityResult result = await userManager.CreateAsync(user, userPassword);

                    if (result.Succeeded)
                    {
                        if (roles.Contains(role))
                        {
                            await userManager.AddToRoleAsync(user, role);
                        }
                    }
                }
            }
            ;

            string adminUserName = "admin";
            string adminEmail = "admin@email.com";
            string adminPassword = "admin";
            await CreateUser(adminUserName, adminEmail, adminPassword, "Admin");

            string staffUserName = "staff";
            string staffEmail = "staff@email.com";
            string staffPassword = "staff";
            await CreateUser(staffUserName, staffEmail, staffPassword, "Staff");
        }
    }
}

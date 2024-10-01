using Microsoft.AspNetCore.Identity;
using PGPARS.Models;

namespace PGPARS.Data
{
    public class DbSeeder
    {
        public static async Task SeedAdminUser(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {

            // create the role "admin" if it does not already exist
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // create the user if it doesn't already exist
            var email = "admin@admin.com";
            var user = await userManager.FindByEmailAsync(email);
            if (user == null) 
            {
                var newUser = new AppUser
                {
                    UserName = email,
                    Email = email,
                    Nnumber = "n000000"
                };

                var result = await userManager.CreateAsync(newUser, "admin@1234");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newUser, "Admin");
                }
                else
                {
                    Console.WriteLine("Creating user failed");
                }
            }
        }
    }
}

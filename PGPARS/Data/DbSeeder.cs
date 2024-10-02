using Microsoft.AspNetCore.Identity;
using PGPARS.Models;
using System.Diagnostics;

namespace PGPARS.Data
{
    public class DbSeeder
    {
        public static async Task SeedAdminUser(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {

            Debug.WriteLine("Beginning Seeding process");

            // create the role "admin" if it does not already exist
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                var result1 = await roleManager.CreateAsync(new IdentityRole("Admin"));
                if (result1.Succeeded)
                {
                    Debug.WriteLine("Role created!");
                }

            }

            // create the user if it doesn't already exist
            var email = "admin@admin.com";
            var user = await userManager.FindByEmailAsync(email);
            if (user == null) 
            {
                Debug.WriteLine("Attempting to create new user...");
                var newUser = new AppUser
                {
                    UserName = email,
                    Email = email,
                    Nnumber = "n00000000"
                };

                var result = await userManager.CreateAsync(newUser, "Admin@1234");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newUser, "Admin");
                    Debug.WriteLine("User successfully created!");

                }
                else
                {
                    Debug.WriteLine("Creating user failed");
                    foreach(var error in result.Errors)
                    {
                        Debug.WriteLine($"Error: {error.Description}");
                    }

                }
            }
        }
    }
}

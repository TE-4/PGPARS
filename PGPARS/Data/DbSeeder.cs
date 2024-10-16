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
                    Debug.WriteLine("Admin role created!");
                }

            }

            // create the role "Faculty" if it does not already exist
            if(!await roleManager.RoleExistsAsync("Faculty"))
            {
                var result2 = await roleManager.CreateAsync(new IdentityRole("Faculty"));
                if (result2.Succeeded)
                {
                    Debug.WriteLine("Faculty role created!");
                }
            }

            // create the role "Committee" if it does not already exist
            if (!await roleManager.RoleExistsAsync("Committee"))
            {
                var result2 = await roleManager.CreateAsync(new IdentityRole("Committee"));
                if (result2.Succeeded)
                {
                    Debug.WriteLine("Committee role created!");
                }
            }

            // create the role "Staff" if it does not already exist
            if (!await roleManager.RoleExistsAsync("Staff"))
            {
                var result2 = await roleManager.CreateAsync(new IdentityRole("Staff"));
                if (result2.Succeeded)
                {
                    Debug.WriteLine("Staff role created!");
                }
            }

            // create the user if it doesn't already exist
            // replace this email w/ client's real email later
            var email = "admin@admin.com";
            var user = await userManager.FindByEmailAsync(email);
            if (user == null) 
            {
                Debug.WriteLine("Attempting to create new user...");
                var newUser = new AppUser
                {
                    FirstName = "Dan",
                    LastName = "Richard",
                    Email = email,
                    Nnumber = "n00009308",
                    UserName = email,
                    Position = "Professor"
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

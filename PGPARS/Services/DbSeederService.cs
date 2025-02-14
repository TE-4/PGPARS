using Microsoft.AspNetCore.Identity;
using PGPARS.Models;
using System.Diagnostics;

namespace PGPARS.Data
{
    public class DbSeederService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public DbSeederService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager) 
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedRolesAndUsers()
        {

            Debug.WriteLine("Beginning Seeding process");

            // create the role "admin" if it does not already exist
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                var result = await _roleManager.CreateAsync(new IdentityRole("Admin"));
                if (result.Succeeded)
                {
                    Debug.WriteLine("Admin role created!");
                }

            }

            // create the role "Faculty" if it does not already exist
            if(!await _roleManager.RoleExistsAsync("Faculty"))
            {
                var result = await _roleManager.CreateAsync(new IdentityRole("Faculty"));
                if (result.Succeeded)
                {
                    Debug.WriteLine("Faculty role created!");
                }
            }

            // create the role "Committee" if it does not already exist
            if (!await _roleManager.RoleExistsAsync("Committee"))
            {
                var result = await _roleManager.CreateAsync(new IdentityRole("Committee"));
                if (result.Succeeded)
                {
                    Debug.WriteLine("Committee role created!");
                }
            }

            // create the role "Staff" if it does not already exist
            if (!await _roleManager.RoleExistsAsync("Staff"))
            {
                var result = await _roleManager.CreateAsync(new IdentityRole("Staff"));
                if (result.Succeeded)
                {
                    Debug.WriteLine("Staff role created!");
                }
            }

            // create the user if it doesn't already exist
            
            var email = "admin@admin.com";
            var user = await _userManager.FindByEmailAsync(email);
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

                var result = await _userManager.CreateAsync(newUser, "Admin@1234");

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newUser, "Admin");
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

            //Committee Tester
            email = "com@com.com";
            user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                Debug.WriteLine("Attempting to create new user...");
                var newUser = new AppUser
                {
                    FirstName = "That",
                    LastName = "Guy",
                    Email = email,
                    Nnumber = "n00000001",
                    UserName = email,
                    Position = "Committee Positions?"
                };

                var result = await _userManager.CreateAsync(newUser, "Com@1234");

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newUser, "Committee");
                    Debug.WriteLine("User successfully created!");

                }
                else
                {
                    Debug.WriteLine("Creating user failed");
                    foreach (var error in result.Errors)
                    {
                        Debug.WriteLine($"Error: {error.Description}");
                    }

                }
            }

            email = "comm@comm.com";
            user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                Debug.WriteLine("Attempting to create new user...");
                var newUser = new AppUser
                {
                    FirstName = "That",
                    LastName = "Gal",
                    Email = email,
                    Nnumber = "n00000003",
                    UserName = email,
                    Position = "Committee Positions?"                 
                };

                var result = await _userManager.CreateAsync(newUser, "Com@12345");

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newUser, "Committee");
                    Debug.WriteLine("User successfully created!");

                }
                else
                {
                    Debug.WriteLine("Creating user failed");
                    foreach (var error in result.Errors)
                    {
                        Debug.WriteLine($"Error: {error.Description}");
                    }

                }
            }

            //Faculty Tester
            email = "Fac@Fac.com";
            user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                Debug.WriteLine("Attempting to create new user...");
                var newUser = new AppUser
                {
                    FirstName = "Big",
                    LastName = "Steppa",
                    Email = email,
                    Nnumber = "n00000002",
                    UserName = email,
                    Position = "Faculty Positions?"                
                };

                var result = await _userManager.CreateAsync(newUser, "Fac@1234");

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newUser, "Faculty");
                    Debug.WriteLine("User successfully created!");

                }
                else
                {
                    Debug.WriteLine("Creating user failed");
                    foreach (var error in result.Errors)
                    {
                        Debug.WriteLine($"Error: {error.Description}");
                    }

                }
            }
        }
    }
}

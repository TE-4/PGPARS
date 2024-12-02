using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using PGPARS.Models;
using PGPARS.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace PGPARS.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;  
        private readonly RoleManager<IdentityRole> _roleManager;

        

        public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("Dashboard", "Admin");
                }
                if (User.IsInRole("Committee"))
                {
                    return RedirectToAction("Dashboard", "Committee");
                }
                if (User.IsInRole("Faculty"))
                {
                    return RedirectToAction("Dashboard", "Faculty");
                }
                if (User.IsInRole("Staff"))
                {
                    return RedirectToAction("Dashboard", "Staff");
                }
                return RedirectToAction("Login", "Account");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);
                if(result.Succeeded)
                {
                    return RedirectToAction("Dashboard", "Admin");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid Email or Password.");
                }

                
            }
            return View(model);
        }// end method

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {              
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userExists = await _userManager.Users.FirstOrDefaultAsync(u => u.Nnumber == model.Nnumber);
                if(userExists != null)
                {
                    ModelState.AddModelError(string.Empty, "User with this N-Number already exists.");
                    return View(model);
                }


                // Find role, if exists, and returns async task containing the role
                var role = await _roleManager.FindByNameAsync(model.Role);

                var user = new AppUser { FirstName = model.FirstName,
                                         LastName = model.LastName,
                                         Email = model.Email,
                                         UserName = model.Email,
                                         Nnumber = model.Nnumber,
                                         MainRole = role.Name};

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Assign the selected role to the user
                    
                    if (role != null)
                    {
                        await _userManager.AddToRoleAsync(user, role.Name);
                        Debug.WriteLine("Assigned role to new user successfully!");
                        
                    }
                    TempData["UserCreated"] = "User successfully created!";
                    return RedirectToAction("Directory", "Account");
                }
                

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

           

            return View(model);
        }

        public async Task<IActionResult> Directory()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }


        // Add search bar functionality to Directory method or new method
        // Add filters

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) 
            {
                return NotFound();
            }
            var model = new EditViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Nnumber = user.Nnumber,
                Position = user.Position,
                Email = user.Email,
                Role = user.MainRole
            };



            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // checks if the user exists in the database
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return NotFound();
            }

            // checks if the Nnumber is already associated with another user
            var existingUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Nnumber == model.Nnumber && u.Id != model.Id);
            if (existingUser != null)
            {
                ModelState.AddModelError("Nnumber", "The Nnumber is already associated with another user.");
                return View(model);
            }

            // Update user details (other fields)
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.Nnumber = model.Nnumber;
            user.MainRole = model.Role;
            user.Position = model.Position;

            // Check if the password fields are populated for change
            if (!string.IsNullOrEmpty(model.CurrentPassword) && !string.IsNullOrEmpty(model.NewPassword))
            {
                if (model.NewPassword != model.ConfirmPassword)
                {
                    ModelState.AddModelError(string.Empty, "New password and confirm password do not match.");
                    return View(model);
                }

                var passwordCheck = await _userManager.CheckPasswordAsync(user, model.CurrentPassword);
                if (!passwordCheck)
                {
                    ModelState.AddModelError(string.Empty, "Current password is incorrect.");
                    return View(model);
                }

                var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(model);
                }
            }

            // Save other updates to the user
            var updateResult = await _userManager.UpdateAsync(user);
            if (updateResult.Succeeded)
            {
                TempData["UserUpdated"] = "User details successfully updated!";
                return RedirectToAction("Directory");
            }

            foreach (var error in updateResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["UserDeleted"] = "User account successfully deleted!";
                return RedirectToAction("Directory");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return RedirectToAction("Directory");
        }

      
        public async Task<IActionResult> Detail(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
                
            }

            return View(user);
        }

        // This method will display the linked applicants for a user
        public async Task<IActionResult> LinkedApplicants(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Get applicants that are linked to the User here
            //var linkedApplicants = await GetLinkedApplicants(user.Id);

            // Pass the linkedApplicants to the view
            //ViewData["LinkedApplicants"] = linkedApplicants;

            return View(user);
        }


        // This method will filter the users by search query and filter by role
        [HttpGet]
        public async Task<IActionResult> SearchUsers(string query, string role)
        {
            // using AsQueryable() to allow for dynamic filtering
            var userQuery = _userManager.Users.AsQueryable();

            // remove leading and trailing whitespace and convert to lowercase
            query = query?.Trim().ToLower();

            if (!string.IsNullOrEmpty(query))
            {
                userQuery = userQuery.Where(u =>
                    u.FirstName.ToLower().Contains(query) || // Matches first name
                    u.LastName.ToLower().Contains(query) || // Matches last name
                    (u.FirstName + " " + u.LastName).ToLower().Contains(query) || // Matches full name
                    u.Email.ToLower().Contains(query)); // Matches email
            }

            // this if-statement filters users by Role and works with a hierarchical structure where all committee members are faculty
            // and the admin can serve as faculty or committee
            if (!string.IsNullOrEmpty(role))
            {
                if(role == "Faculty")
                {
                    userQuery = userQuery.Where(u => u.MainRole == "Faculty" || u.MainRole == "Committee" || u.MainRole == "Admin");
                }
                else if(role == "Committee")
                {
                    userQuery = userQuery.Where(u => u.MainRole == "Committee" || u.MainRole == "Admin");
                }
                else 
                { 
                userQuery = userQuery.Where(u => u.MainRole == role);
                }
            }

            var users = await userQuery.ToListAsync();

            return View("Directory", users);
        }




    }// end class
}// end namespace

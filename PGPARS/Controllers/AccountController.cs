﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using PGPARS.Models;
using PGPARS.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using PGPARS.Data;
using System.Threading.Tasks;
using PGPARS.Services;

namespace PGPARS.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;  
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IApplicantRepository _applicantRepository;
        private readonly AuditLogService _logger;
        private readonly ApplicationDbContext _context;



        public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager
            , IApplicantRepository applicantRepo, AuditLogService auditLogService, ApplicationDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _applicantRepository = applicantRepo;
            _logger = auditLogService;
            _context = context;
        }

        private IActionResult RedirectBasedOnRole()
        {
            // Log the user login
            _logger.LogAction("Login", User.Identity.Name, "User logged in.", "INFO");

            // Redirect based on role
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("AdminDashboard", "Admin");
            }
            if (User.IsInRole("Committee"))
            {
                return RedirectToAction("CommitteeDashboard", "Committee");
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

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectBasedOnRole();
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // isPersistent set to true for easier testing - set to false for production
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: true, lockoutOnFailure: false);
                if(result.Succeeded)
                {
                    return RedirectBasedOnRole();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid Email or Password.");
                }
            }

            return View(model);
        }

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
                                         Nnumber = model.Nnumber};

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

                    // log the user creation
                    _logger.LogAction("User Creation", User.Identity.Name, $"User {user.Email} created successfully.", "INFO");

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
                Email = user.Email};



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

            // Obtain the User's current role
            var currentRoles = await _userManager.GetRolesAsync(user);

            // if role is changed, remove the user from the old role and add to the new role
            if (!currentRoles.Contains(model.Role))
            {
                // remove them from their current role
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                var role = await _roleManager.FindByNameAsync(model.Role);
                if (role != null)
                {
                    await _userManager.AddToRoleAsync(user, role.Name);
                }
            }

                // Update user details (other fields)
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.Nnumber = model.Nnumber;
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
                TempData["UserDeleted"] = "User account successfully deleted.";
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

        public async Task<IActionResult> LinkedApplicants(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound();
            }

            // Retrieve applicants linked through the ApplicantReviewer table
            var linkedApplicants = _context.ApplicantReviewers
                .Where(ar => ar.AppUserId == user.Id) // Find records where the reviewer is the current user
                .Select(ar => ar.Applicant) // Select the corresponding applicants
                .ToList();

            Debug.WriteLine($"Linked Applicants Count: {linkedApplicants.Count}");

            var viewModel = new LinkedApplicantsViewModel
            {
                User = user,
                LinkedApplicants = linkedApplicants
            };

            return View(viewModel);
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

            List<AppUser> users;

            // this if-statement filters users by Role from the drop down filter select option
            if (!string.IsNullOrEmpty(role))
            {
                var usersInRole = await _userManager.GetUsersInRoleAsync(role);
                users = usersInRole.Where(u => userQuery.Any(filteredUser => filteredUser.Id == u.Id)).ToList();
            }
            else
            {
                users = await userQuery.ToListAsync();
            }



                return View("Directory", users);
        }




    }// end class
}// end namespace

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using PGPARS.Models;
using PGPARS.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

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
                return RedirectToAction("Dashboard", "Admin");
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
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: true, lockoutOnFailure: false);
                if(result.Succeeded)
                {
                    return RedirectToAction("Dashboard", "Admin");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login Credentials.");
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
            // get roles from AspNetRoles table and store them in roles variable
            var roles = _roleManager.Roles.ToList();
            var viewModel = new RegisterViewModel
            {
                Roles = roles.Select(role => new SelectListItem
                {
                    Value = role.Id,
                    Text = role.Name
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser { FirstName = model.FirstName, LastName = model.LastName, Email = model.Email, UserName = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Assign the selected role to the user
                    var role = await _roleManager.FindByIdAsync(model.RoleId);
                    if (role != null)
                    {
                        await _userManager.AddToRoleAsync(user, role.Name);
                        Debug.WriteLine("Assigned role to new user successfully!");
                    }

                    return RedirectToAction("Dashboard", "Admin");
                }
                

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed; reload the roles to show them again in the view
            var roles = _roleManager.Roles.ToList();
            model.Roles = roles.Select(role => new SelectListItem
            {
                Value = role.Id,
                Text = role.Name
            }).ToList();

            return View(model);
        }



    }// end class
}// end namespace

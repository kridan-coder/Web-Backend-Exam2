using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BackendTest2.Models;
using BackendTest2.Models.AccountViewModels;
using BackendTest2.Data;
using BackendTest2.Models.ViewModels;

namespace BackendTest2.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ILogger logger;

        public AccountController(
            ApplicationDbContext context,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AccountController> logger)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.context = context;
        }


        [HttpGet]
        [Authorize(Roles = ApplicationRoles.Administrators)]
        public async Task<IActionResult> Index()
        {

            var allUsers = context.Users.ToList();

            ViewBag.Users = allUsers;

            var allRoles = new List<String>();



            foreach (var user in allUsers)
            {
                var roles = await userManager.GetRolesAsync(user);
                allRoles.Add(roles.First());
                
            }
            ViewBag.Roles = allRoles;
            return this.View();
        }

        [HttpPost]
        [Authorize(Roles = ApplicationRoles.Administrators)]
        public async Task<IActionResult> Index(UsersFilterViewModel model)
        {

            var allUsers = context.Users
   .Where(u => u.Email == model.Email || u.UserName == model.Name).ToList();
            if (model.Email == null && model.Name == null)
            {
                allUsers = context.Users.ToList();
            }
            var tmp = allUsers.GetRange(0, allUsers.Count);
            if (model.Role != "Everyone")
            {
                foreach (var user in allUsers)
                {
                    var userRoles = await userManager.GetRolesAsync(user);
                    var userRole = userRoles.First();
                    if (userRole != model.Role)
                    {
                        tmp.Remove(user);
                    }
                }
            }

            allUsers = tmp;




            ViewBag.Users = allUsers;

            var allRoles = new List<String>();



            foreach (var user in allUsers)
            {
                var roles = await userManager.GetRolesAsync(user);
                allRoles.Add(roles.First());

            }
            ViewBag.Roles = allRoles;
            return this.View();
        }


        [HttpGet]
        [Authorize(Roles = ApplicationRoles.Administrators)]
        public async Task<IActionResult> Delete(String id)
        {
            try
            {
                var user = await userManager.FindByIdAsync(id);
                await userManager.DeleteAsync(user);
            }
            catch (Exception e)
            {
                return this.RedirectToAction("Index", "Error", new { error = e.Message });
            }

            return this.RedirectToAction(nameof(Index));
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(String returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await this.HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            this.ViewData["ReturnUrl"] = returnUrl;
            return this.View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, String returnUrl = null)
        {
            this.ViewData["ReturnUrl"] = returnUrl;
            if (this.ModelState.IsValid)
            {
                var result = await this.signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    this.logger.LogInformation("User logged in.");
                    return this.RedirectToLocal(returnUrl);
                }

                if (result.IsLockedOut)
                {
                    this.logger.LogWarning(2, "User account locked out.");
                    return this.View("Lockout");
                }

                this.ModelState.AddModelError(String.Empty, "Invalid login attempt.");
                    return this.View(model);
                
            }

            // If we got this far, something failed, redisplay form
            return this.View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(String returnUrl = null)
        {
            this.ViewData["ReturnUrl"] = returnUrl;
            return this.View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, String returnUrl = null)
        {
            this.ViewData["ReturnUrl"] = returnUrl;
            if (this.ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.FullName, Email = model.Email };
                
                var result = await this.userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // add student role
                    var studentsRole = await roleManager.FindByNameAsync(ApplicationRoles.Students);
                    await userManager.AddToRoleAsync(user, studentsRole.Name);

                    await this.signInManager.SignInAsync(user, isPersistent: false);
                    this.logger.LogInformation("User created a new account with password.");
                    return this.RedirectToLocal(returnUrl);
                }

                this.AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await this.signInManager.SignOutAsync();
            this.logger.LogInformation("User logged out.");
            return this.RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return this.View();
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                this.ModelState.AddModelError(String.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(String returnUrl)
        {
            if (this.Url.IsLocalUrl(returnUrl))
            {
                return this.Redirect(returnUrl);
            }
            else
            {
                return this.RedirectToAction("Index", "Home");
            }
        }

        #endregion
    }
}

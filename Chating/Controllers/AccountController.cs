using Chating.DataView;
using Chating.Models;
using Chating.ViewMode;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;



namespace Chating.Controllers
{
    public class AccountController : Controller
    {


        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;

        }




        public IActionResult Regestration()
        {
            return View();
        }



        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Regestration(SignupView account)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser();

                // Mapping
                user.UserName = account.Email;
                user.Email = account.Email;
                user.FirstName = account.Firstname;
                user.LastName = account.Lastname;

                var result = await _userManager.CreateAsync(user, account.Password);

                if (result.Succeeded)
                {
                    // Add claims
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email)
            };

                    await _userManager.AddClaimsAsync(user, claims);

                    // Create cookie and sign in user
                    await _signInManager.SignInAsync(user, false);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Display errors if the registration failed
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return View(account);
                }
            }

            // If ModelState is not valid
            return View(account);
        }

        ///*************************************************************************************************************************************
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    if (result.IsLockedOut)
                    {
                        // Handle locked-out user
                        ModelState.AddModelError(string.Empty, "Account is locked out.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid1 login attempt.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }

            return View(model);
        }
    }




}


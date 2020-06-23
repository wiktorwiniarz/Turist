using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EquipmentRentalCore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace EquipmentRentalCore.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<AccountController> _logger;
        private readonly EquipmentRentalContext _context;
        private readonly RoleManager<Group> _roleManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<AccountController> logger, EquipmentRentalContext context, RoleManager<Group> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
            _roleManager = roleManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Models.AccountViewModels.LoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Login, model.Password, model.RememberLogin, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User has been logged in");
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User has been logged out");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(Models.AccountViewModels.RegisterViewModel registerViewModel, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = registerViewModel.Login
                };
                if (registerViewModel.Password == registerViewModel.ConfirmPassword)
                {
                    user.Password = registerViewModel.Password;
                    user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, registerViewModel.Password);
                    var result = await _userManager.CreateAsync(user);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User has been created");
                        var role = await _roleManager.FindByNameAsync("User");
                        result = await _userManager.AddToRoleAsync(user, role.Name);
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Index", "Home");
                    }
                    else if (result.Errors.Count() > 0)
                    {
                        foreach (var item in result.Errors)
                            ModelState.AddModelError(string.Empty, item.Description);
                    }
                }
                else
                    ModelState.AddModelError(string.Empty, "Hasła się nie zgadzają!");
            }
            return View(registerViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Manage(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                throw new ApplicationException($"Unable to load user '{_userManager.GetUserId(User)}'");

            var grpList = new List<Models.AccountViewModels.GroupViewModel>();
            var groupIDList = new List<int>();
            foreach (var item in _context.UserRoles.Where(x => x.UserId == user.Id))
                groupIDList.Add(item.RoleId);


            foreach (var item in groupIDList)
            {
                var element = await _roleManager.FindByIdAsync(item.ToString());
                grpList.Add(new Models.AccountViewModels.GroupViewModel
                {
                    GroupName = element.Name,
                    GroupID = element.Id
                });
            }
            var model = new Models.AccountViewModels.ManageUserViewModel
            {
                Username = user.UserName,
                Id = user.Id,
                FirstName = user.Name,
                Surname = user.Surname,
                GroupList = grpList,
                PasswordEditViewModel = new Models.AccountViewModels.PasswordEditViewModel()
            };

            return View(model);
        }

        public async Task<IActionResult> Manage(Models.AccountViewModels.ManageUserViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var userToEdit = await _userManager.GetUserAsync(User);
                userToEdit.Name = model.FirstName;
                userToEdit.Surname = model.Surname;
                await _userManager.UpdateAsync(userToEdit);
                return RedirectToAction("Index", "Home");
            }
            return View(model);

        }

        [HttpGet]
        public async Task<IActionResult> EditPassword(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPassword(Models.AccountViewModels.ManageUserViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                if (!model.PasswordEditViewModel.NewPassword.Equals(model.PasswordEditViewModel.NewPasswordConfirmation))
                {
                    ModelState.AddModelError(string.Empty, "Your new password does not match your password confirmation");
                    return RedirectToAction("Manage", "Account");
                }

                var result = await _userManager.ChangePasswordAsync(user, model.PasswordEditViewModel.OldPassword, model.PasswordEditViewModel.NewPassword);
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.PasswordEditViewModel.NewPassword);
                var resultHash = await _userManager.UpdateAsync(user);
                if (result.Succeeded && resultHash.Succeeded)
                    _logger.LogInformation($"Password for user {user.UserName} changed");

            }
            return RedirectToAction("Manage", "Account");
        }
    }
}
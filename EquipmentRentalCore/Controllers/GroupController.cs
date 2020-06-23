using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EquipmentRentalCore.Models;
using EquipmentRentalCore.Models.GroupModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EquipmentRentalCore.Controllers
{
    
    public class GroupController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<GroupController> _logger;
        private readonly EquipmentRentalContext _context;
        private readonly RoleManager<Group> _roleManager;

        public GroupController(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<GroupController> logger, EquipmentRentalContext context, RoleManager<Group> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var roles = await _context.Roles.ToListAsync();
            var model = new List<RolesViewModel>();
            foreach (var item in roles)
            {
                var usersInRole = await _context.UserRoles.Where(x => x.RoleId.Equals(item.Id)).ToListAsync();
                var usersList = new List<UsersInRoleViewModel>();
                foreach (var element in usersInRole)
                {
                    var user = await _userManager.FindByIdAsync(element.UserId.ToString());

                    usersList.Add(new UsersInRoleViewModel
                    {
                        UserID = user.Id,
                        Username = (user.Name != null) ? user.Name + " " + user.Surname : user.UserName
                    });
                }
                model.Add(new RolesViewModel
                {
                    RoleID = item.Id,
                    RoleName = item.Name,
                    UsersList = usersList
                });
            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> _AddUserToRole(int id, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var getUsersInRole = await _context.UserRoles.Where(x => x.RoleId.Equals(id)).ToListAsync();

            var listUsersNotInRole = new List<User>();

            foreach (var item in await _context.Users.ToListAsync())
            {
                if (!getUsersInRole.Any(x => x.UserId == item.Id))
                    listUsersNotInRole.Add(item);
            }
            if (listUsersNotInRole.Any())
            {
                List<SelectListItem> userList = new List<SelectListItem>();
                foreach (var item in listUsersNotInRole)
                    userList.Add(new SelectListItem
                    {
                        Value = item.Id.ToString(),
                        Text = (item.Name != null) ? item.Name + " " + item.Surname : item.UserName
                    });
                var model = new RolesViewModel
                {
                    ChooseAdditionalUserList = userList,
                    RoleID = id
                };
                return PartialView(model);
            }
            return PartialView("_EmptyListModal");
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _AddUserToRole(RolesViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.ChosenListID.ToString());
                var role = await _roleManager.FindByIdAsync(model.RoleID.ToString());
                var result = await _userManager.AddToRoleAsync(user, role.Name);
                if (result.Succeeded)
                    return RedirectToAction("Index", "Group");
            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> RemoveUserFromGroup(int id, int roleid, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user.Id > 0)
            {
                var role = await _roleManager.FindByIdAsync(roleid.ToString());
                if (role.Id > 0)
                {
                    await _userManager.RemoveFromRoleAsync(user, role.Name);
                    return RedirectToAction("Index", "Group");
                }
            }
            return NotFound();
        }
    }
}
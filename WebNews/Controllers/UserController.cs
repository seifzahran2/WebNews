using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebNews.Models;
using WebNews.Models.ViewModels;

namespace WebNews.Controllers
{
    [Authorize(Roles ="Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<Appuser> _userManger;
        private readonly RoleManager<IdentityRole> _roleManger;
        public UserController(UserManager<Appuser> userManger, RoleManager<IdentityRole> roleManger)
        {
            _userManger = userManger;
            _roleManger = roleManger;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManger.Users.Select(user => new UserVM
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email=user.Email,
                Roles = _userManger.GetRolesAsync(user).Result
            }).ToListAsync();
            return View(user);
        }

        public async Task<IActionResult> ManageRoles(string Userid)
        {
            var user = await _userManger.FindByIdAsync(Userid);
            if (user == null)
                return NotFound();
            var userRoles = await _roleManger.Roles.ToListAsync();

            var viewModel = new UserRolesVM
            {
                UserID = Userid,
                UserName = user.UserName,
                UserRole = userRoles.Select(role => new RoleVM
                {
                    roleId=role.Id,
                    roleName=role.Name,
                    IsSelcted = _userManger.IsInRoleAsync(user,role.Name).Result
                }).ToList()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageRoles(UserRolesVM model)
        {
            var user = await _userManger.FindByIdAsync(model.UserID);
            if (user == null)
                return NotFound();
            var userRoles = await _userManger.GetRolesAsync(user);
            foreach (var role in model.UserRole)
            {
                if(userRoles.Any(r => r ==role.roleName)&& !role.IsSelcted)
                {
                    await _userManger.RemoveFromRoleAsync(user, role.roleName);
                }
                if (!userRoles.Any(r => r == role.roleName) && role.IsSelcted)
                {
                    await _userManger.AddToRoleAsync(user, role.roleName);
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebNews.Models.ViewModels;

namespace WebNews.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController1 : Controller
    {
        private readonly RoleManager<IdentityRole> _rolemanger;
        public RolesController1(RoleManager<IdentityRole> rolemanger)
        {
            _rolemanger = rolemanger;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _rolemanger.Roles.ToListAsync());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(RoleFormVM model)
        {
            if(!ModelState.IsValid)
            {
                return View ("Index",await _rolemanger.Roles.ToListAsync());
            }
            if(await _rolemanger.RoleExistsAsync(model.Name))
            {
                ModelState.AddModelError("Name", "Role is Exists");
                return View("Index", await _rolemanger.Roles.ToListAsync());
            }
            await _rolemanger.CreateAsync(new IdentityRole(model.Name.Trim()));
            return RedirectToAction(nameof(Index));
        }
    }
}

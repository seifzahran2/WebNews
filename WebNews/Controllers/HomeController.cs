using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebNews.Models;

namespace AppUser.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext context;

        public HomeController(AppDbContext _context)
        {
            context = _context;
        }

        public IActionResult Index2()
        {
            var NewsCategory = context.Category.ToList();
           
            return View(NewsCategory);
        }

        public IActionResult Index()
        {
            var NewsCategory = context.Category.ToList();

            return View(NewsCategory);

        }
        [Authorize(Roles = "Admin")]
        public IActionResult Message()
        {
            return View(context.contactUs.ToList());
        }

        public IActionResult News(int id)
        {
            Categories cat = context.Category.Find(id);
            ViewData["news"] = cat.Name;
            var result = context.news.Where(y => y.categoryId == id).OrderByDescending(x => x.NewsDate).ToList();
            return View(result);
        }

        public IActionResult Newsall(int id)
        {
            Categories cat = context.Category.Find(id);
            //ViewData["news"] = cat.Name;
            var result = context.news.OrderByDescending(x => x.NewsDate).ToList();
            return View(result);
        }

        public IActionResult Contact()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var res = context.news.Find(id);
            context.news.Remove(res);
            context.SaveChanges();
            return RedirectToAction("News");
        }

        [HttpPost]
        public IActionResult ContactPost(ContactUs _contact)
        {
            context.contactUs.Add(_contact);
            context.SaveChanges();
            return RedirectToAction("Index");

        }


    }
}

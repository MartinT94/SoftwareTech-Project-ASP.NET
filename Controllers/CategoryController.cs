using ASP.NET_Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASP.NET_Blog.Controllers
{
    public class CategoryController : Controller
    {
        [Authorize(Roles = "Admin")]
        // GET: Category
        public ActionResult Index()
        {
            return View();
            
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                using (var database = new BlogDbContext())
                {
                    var authorId = database.Users
                        .Where(u => u.UserName == this.User.Identity.Name)
                        .First()
                        .Id;
                    

                    database.Categories.Add(category);
                    database.SaveChanges();

                    return RedirectToAction("Create");
                }
            }

            return View(category);
        }
    }
}
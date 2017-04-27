using ASP.NET_Blog.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASP.NET_Blog.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (var database = new BlogDbContext())
            {
                var articles = database.Articles
                    .Include(a => a.Author)
                    .OrderByDescending(a => a.Date).Take(6)
                    .ToList();
                var categories = database.Categories
                    .ToList();

                var tuple = new Tuple<List<Article>,
                    List<Category>>(articles,
                    categories);
                return View(tuple);
                
            }

        }

        public FileContentResult UserPhotos()
        {
            if (User.Identity.IsAuthenticated)
            {
                String userId = User.Identity.GetUserId();

                var bdUsers = HttpContext.GetOwinContext().Get<BlogDbContext>();
                var userImage = bdUsers.Users.Where(x => x.Id == userId).FirstOrDefault();

                if (userImage.UserPhoto == null)
                {
                    string fileName = HttpContext.Server.MapPath(@"~/Content/default.jpg");

                    byte[] imageData = null;
                    FileInfo fileInfo = new FileInfo(fileName);
                    long imageFileLength = fileInfo.Length;
                    FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    imageData = br.ReadBytes((int)imageFileLength);

                    return File(imageData, "image/png");

                }

                return new FileContentResult(userImage.UserPhoto, "image/jpeg");
            }
            else
            {
                string fileName = HttpContext.Server.MapPath(@"~/Content/default.jpg");

                byte[] imageData = null;
                FileInfo fileInfo = new FileInfo(fileName);
                long imageFileLength = fileInfo.Length;
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                imageData = br.ReadBytes((int)imageFileLength);
                return File(imageData, "image/png");

            }
        }

    }
}
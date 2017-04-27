using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ASP.NET_Blog.Models;
using System.IO;

namespace ASP.NET_Blog.Controllers
{
    public class ArticleController : Controller
    {
        // GET: Article
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }
        [HttpPost]
        [Authorize]
        public ActionResult Search()
        {
            
            using (var database = new BlogDbContext())
            {
                string search = Request["Search"];
                List<Article> articles = database.Articles
                        .Where(a => a.Tags.Contains(search))
                        .Include(a => a.Author)
                        .ToList();
             
                return View(new Tuple<List<Article>, string>(articles, search));
            }
        }

        public ActionResult List()
        {
            using (var database = new BlogDbContext())
            {
                string categoryName = Request.QueryString.Get("category");
                List<Article> articles = null;
                if (categoryName != null)
                {
                    articles = database.Articles
                        .Where(a => a.CategoryName == categoryName)
                        .Include(a => a.Author)
                        .ToList();
                }
                else
                {
                    articles = database.Articles
                        .Include(a => a.Author)
                        .ToList();
                }
                return View(articles);
            }

        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new BlogDbContext())
            {
                var article = database.Articles
                    .Where(a => a.Id == id)
                    .Include(a => a.Author)
                    .First();

                article.VisitingCount++;
                database.SaveChanges();


                var comments = database.Comments
                    .Where(c => c.ArticleId == id)
                    .ToList();

                foreach (var comment in comments)
                {
                    var user = database.Users
                        .Where(i => i.Id == comment.AuthorId);

                }

                article.Comments = comments;

                if (article == null)
                {
                    return HttpNotFound();
                }
                
                return View(new Tuple<Article, Comment>(article, new Comment()));
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult Create()
        {
            using (var database = new BlogDbContext())
            {
                var categories = database.Categories
                   .ToList();
                var tuple = new Tuple<Article,
                       List<Category>>(new Article(),
                       categories);
                return View(tuple);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(Article article)
        {
            if (ModelState.IsValid)
            {
                using (var database = new BlogDbContext())
                {
                    var authorId = database.Users
                        .Where(u => u.UserName == this.User.Identity.Name)
                        .First()
                        .Id;

                    article.AuthorId = authorId;

                    database.Articles.Add(article);
                    database.SaveChanges();

                    return RedirectToAction("Index");
                }
            }

            return View(article);
        }

        //Get: Article/Delete
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new BlogDbContext())
            {
                var article = database.Articles
                    .Where(a => a.Id == id)
                    .Include(a => a.Author)
                    .First();

                if (!IsUserAuthorizedToEdit(article))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                if (article == null)
                {
                    return HttpNotFound();
                }


                return View(article);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new BlogDbContext())
            {
                var article = database.Articles
                    .Where(a => a.Id == id)
                    .Include(a => a.Author)
                    .First();

                if (article == null)
                {
                    return HttpNotFound();
                }

                database.Articles.Remove(article);
                database.SaveChanges();

                return RedirectToAction("Index");
            }
        }


        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            using (var database = new BlogDbContext())
            {
                var article = database.Articles
                    .Where(a => a.Id == id)
                    .First();

                if (!IsUserAuthorizedToEdit(article))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                if (article == null)
                {
                    return HttpNotFound();
                }

                var model = new ArticleViewModel();

                model.Id = article.Id;
                model.Title = article.Title;
                model.Content = article.Content;

                return View(model);
            }
        }

        [HttpPost]
        public ActionResult Edit(ArticleViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var database = new BlogDbContext())
                {
                    var article = database.Articles
                        .FirstOrDefault(a => a.Id == model.Id);

                    article.Title = model.Title;
                    article.Content = model.Content;


                    database.Entry(article).State = EntityState.Modified;
                    database.SaveChanges();

                    return RedirectToAction("Index");
                }
            }

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public ActionResult CreateComment(int articleId)
        {
            var comment = new Comment();

            comment.ArticleId = articleId;

            return View(comment);
        }

        [HttpPost]
        [Authorize]
        public ActionResult CreateComment(Comment comment)
        {
            var db = new BlogDbContext();

            if (ModelState.IsValid)
            {
                if (this.User != null)
                {
                    var username = this.User.Identity.Name;
                    var userFromDB = db.Users
                        .Where(u => u.UserName == username)
                        .FirstOrDefault();

                    comment.AuthorId = userFromDB.Id;
                    comment.FullName = userFromDB.FullName;
                    comment.UserPhoto = userFromDB.UserPhoto;
                }

                db.Comments.Add(comment);
                db.SaveChanges();


            }

            return RedirectToAction($"Details/{comment.ArticleId}");
        }

        private bool IsUserAuthorizedToEdit(Article article)
        {
            bool isAdmin = this.User.IsInRole("Admin");
            bool isAuthor = article.IsAuthor(this.User.Identity.Name);

            return isAdmin || isAuthor;
        }
    }
}
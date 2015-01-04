using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using blog.DB;

namespace blog.Controllers
{
    public class homeController : Controller
    {
        public ActionResult Index()
        {
            using (var db = new blogEntities())
            {
                var articles = db.Articles.OrderByDescending(c => c.id).Take(5).ToList();
                ViewBag.all = (db.Articles.ToList().Count % 5 == 0) ? (db.Articles.ToList().Count / 5) : (db.Articles.ToList().Count / 5) + 1; 
                return View(articles);
            }
        }
        [HttpGet]
        public ActionResult page(int? id)
        {
             if (id.HasValue == false) return Redirect("/error/");
             int page = Convert.ToInt32(id.ToString());
             using (var db = new blogEntities())
             {
                var articles = db.Articles.OrderByDescending(c => c.id).Skip(5 * (page - 1)).Take(5).ToList();
                if (articles.Count == 0)
                {
                    return Redirect("/error/");
                }
                ViewBag.all = (db.Articles.ToList().Count % 5 == 0) ? (db.Articles.ToList().Count / 5) : (db.Articles.ToList().Count / 5) + 1;
                return View(articles);
             }
        }
    }
}
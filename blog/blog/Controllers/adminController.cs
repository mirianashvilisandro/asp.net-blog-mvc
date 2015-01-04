using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using blog.DB;

namespace blog.Controllers
{
    public class adminController : Controller
    {
        public ActionResult Index()
        {
            if (Session != null && Session["login"] != null)
            {
                return Redirect("~/admin/main");
            }
            return View();
        }
        [HttpPost]
        public string check(string username,string password)
        {
            using (var db = new blogEntities())
            {
                var Users = db.Users.ToList();
                foreach (var item in Users)
                {
                    if (item.username == username && item.password == password)
                    {
                        Session["login"] = "1";
                        Session["username"] = username.ToString();
                        return "1";
                    }
                }
                return "-1";
            }
        }
        public ActionResult main()
        {
            if (Session != null && Session["login"] != null)
            {
                using (var db = new blogEntities())
                {
                    var list = db.Articles.ToList();
                    ViewBag.size = list.Count;
                    return View(list);
                }
            }
            return Redirect("/admin/");
        }
        public ActionResult log_out()
        {
            Session.Remove("login");
            Session.Remove("username");
            return Redirect("/admin/");
        }
        [HttpPost]
        public string updateuser(string username,string password)
        {
            using (var db = new blogEntities())
            {
                var selected = db.Users.SingleOrDefault(c => c.id == 1);
                if (selected != null)
                {
                    selected.username = username;
                    selected.password = password;
                }
                db.SaveChanges();
                return "1";
            }
        }
        public ActionResult User()
        {
            if (Session != null && Session["login"] != null)
            {
                using (var db = new blogEntities())
                {
                    var person = db.Users.SingleOrDefault(c => c.id == 1);
                    var user = db.Articles.ToList();
                    ViewBag.username = person.username;
                    ViewBag.password = person.password;
                    ViewBag.size = db.Articles.ToList().Count;
                    return View(user);
                }
            }
            return Redirect("/admin/");
        }
        public ActionResult AddArticle()
        {
            if (Session != null && Session["login"] != null)
            {
                return View();
            }
            return Redirect("/admin/");
        }
        [HttpPost]
        public string addarticles(string name,string body)
        {
            using (var db = new blogEntities())
            {
                Article ar = new Article();
                ar.article_name = name;
                ar.article_body = body;
                db.Articles.Add(ar);
                db.SaveChanges();
            }
            return "1";
        }
        [HttpGet]
        public ActionResult deleteIt(int? id)
        {
            if (id.HasValue)
            {
                using (var db = new blogEntities())
                {
                    var p = db.Articles.SingleOrDefault(c => c.id == id);
                    if (p != null)
                    {
                        db.Articles.Remove(p);
                        db.SaveChanges();
                    }
                }
                return Redirect("/admin");
            }
            else
            {
                return Redirect("/admin");
            }
        }
        [HttpPost]
        public String updateIt(string name,string body,int id) 
        {
            using (var db = new blogEntities())
            {
                var selected = db.Articles.SingleOrDefault(c => c.id == id);
                if (selected != null)
                {
                    selected.article_name = name;
                    selected.article_body = body;
                }
                db.SaveChanges();
            }
            return "1";
        }
        [HttpGet]
        public ActionResult editIt(int? id)
        {
            if (id.HasValue)
            {
                using (var db = new blogEntities())
                {
                    var selected = db.Articles.SingleOrDefault(c => c.id == id);
                    if (selected != null)
                    {
                        ViewBag.name = selected.article_name;
                        ViewBag.id = selected.id;
                        ViewBag.body = selected.article_body;
                    }
                }
                return View();
            }
            else
            {
                return Redirect("/admin");
            }
        }
        [HttpGet]
        public ActionResult search(string query)
        {
            if (Session != null && Session["login"] != null)
            {
                using (var db = new blogEntities())
                {
                    List<Article> selected = new List<Article>();
                    foreach (var item in db.Articles.ToList())
                    {
                        if (item.article_name.Contains(query.ToString()) || item.article_body.Contains(query.ToString()))
                        {
                            selected.Add(item);
                        }
                    }
                    ViewBag.db = db.Articles.ToList();
                    ViewBag.size = db.Articles.ToList().Count();
                    return View(selected);
                }
            }
            else
            {
                return Redirect("/admin/");
            }
        }
    }
}
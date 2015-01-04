using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using blog.DB;

namespace blog.Controllers
{
    public class apiController : Controller
    {
        // GET: api
        public ActionResult Index()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><articles>";
            using (var db = new blogEntities())
            {
                foreach (var item in db.Articles.ToList())
                {
                    xml += "<article><article_name><![CDATA[" + item.article_name.ToString() + "]]></article_name>";
                    xml += "<article_body><![CDATA[" + item.article_body.ToString() + "]]></article_body></article>";
                }
                xml += "</articles>";
                return this.Content(xml, "text/xml");
            }
        }
    }
}
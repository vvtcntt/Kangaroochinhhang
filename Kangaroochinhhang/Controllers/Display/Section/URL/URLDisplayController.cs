using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kangaroochinhhang.Models;
namespace Kangaroochinhhang.Controllers.Display.Section.URL
{
    public class URLDisplayController : Controller
    {
        public KangaroochinhhangContext db = new KangaroochinhhangContext();
        //
        // GET: /URLDisplay/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult URL(string tag)
        {
            int id=int.Parse(tag);
            string url = db.tblUrls.Find(id).Url;
            return Redirect(url);
        }
	}
}
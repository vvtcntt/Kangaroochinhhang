using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kangaroochinhhang.Models;
namespace Kangaroochinhhang.Controllers.Display.Section.Adw
{
    public class AdwController : Controller
    {
        //
        // GET: /Adw/
        KangaroochinhhangContext db = new KangaroochinhhangContext();
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult Adw()
        {
            string left = "";
            string right = "";
            var Mangleft = db.tblImages.Where(p => p.Active == true && p.idCate==4).OrderBy(p => p.Ord).ToList();
            for (int i = 0; i < Mangleft.Count;i++ )
            {
                left += "<a href=\"" + Mangleft[i].Url + "\" title=\"" + Mangleft[i].Name + "\" ><img src=\"" + Mangleft[i].Images + "\" width=\"120\" alt=\"" + Mangleft[i].Name + "\" /></a>";
            }
            ViewBag.left = left;
            var Mangright = db.tblImages.Where(p => p.Active == true && p.idCate == 5).OrderBy(p => p.Ord).ToList();
            for (int i = 0; i < Mangright.Count; i++)
            {
                right += "<a href=\"" + Mangright[i].Url + "\" title=\"" + Mangright[i].Name + "\" ><img src=\"" + Mangright[i].Images + "\" width=\"120\" alt=\"" + Mangright[i].Name + "\" /></a>";
            }
            ViewBag.right = right;
                return PartialView();
        }
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kangaroochinhhang.Models;
namespace Kangaroochinhhang.Controllers.Display.Section.Manufactures
{
    public class ManufacturesDisplayController : Controller
    {
        //
        // GET: /ManufacturesDisplay/
        private KangaroochinhhangContext db = new KangaroochinhhangContext();
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult PartialManufactures()
        {
            var listManu = db.SelectListItem.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
            int cout = listManu.Count;
            if(Request.Browser.IsMobileDevice)
            {
                if(cout%2==0)
                { 
                    listManu = db.SelectListItem.Where(p => p.Active == true).OrderBy(p => p.Ord).Take(cout).ToList();
                }
                else
                {
                    listManu = db.SelectListItem.Where(p => p.Active == true).OrderBy(p => p.Ord).Take(cout-1).ToList();

                }
            }
            return PartialView(listManu);
        }
	}
}
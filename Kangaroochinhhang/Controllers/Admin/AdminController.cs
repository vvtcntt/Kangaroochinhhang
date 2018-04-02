using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kangaroochinhhang.Models;
namespace Kangaroochinhhang.Controllers.Admin
{
    public class AdminController : Controller
    {
        KangaroochinhhangContext db = new KangaroochinhhangContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult partialBanner()
        {
            ViewBag.donhang = db.tblOrders.Where(p => p.Status == false && p.Active==true).ToList().Count;
            return PartialView();
        }
    }
}
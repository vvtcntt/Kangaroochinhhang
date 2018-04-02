
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kangaroochinhhang.Models;

namespace Kangaroochinhhang.Controllers.Display
{
    public class DefaultController : Controller
    {
        KangaroochinhhangContext db = new KangaroochinhhangContext();
        //
        // GET: /Default/
        [OutputCache(Duration = 2200)]
        public ActionResult Index()
        {
            TblConfig TblConfig = db.TblConfigs.First();
            ViewBag.Title = "<title>" + TblConfig.Title + "</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"" + TblConfig.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + TblConfig.Keywords + "\" /> ";
            string chuoi = "";
            chuoi += "<span class=\"gp\">" + TblConfig.Slogan + "</span>";
            chuoi += "<p>HOTLINE: <span>" + TblConfig.MobileIN + " </span> (Miền Bắc) |<span>" + TblConfig.MobileOUT + " </span>(Miền Nam)</p>";
            ViewBag.Hotline = chuoi;
            ViewBag.favicon = " <link href=\"" + TblConfig.Favicon + "\" rel=\"icon\" type=\"image/x-icon\" />";

            string meta = "";
            ViewBag.canonical = "<link rel=\"canonical\" href=\"http://Kangaroochinhhang.com\" />";

            meta += "<meta itemprop=\"name\" content=\"" + TblConfig.Name + "\" />";
            meta += "<meta itemprop=\"url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta itemprop=\"description\" content=\"" + TblConfig.Description + "\" />";
            meta += "<meta itemprop=\"image\" content=\"http://Kangaroochinhhang.com" + TblConfig.Logo + "\" />";
            meta += "<meta property=\"og:title\" content=\"" + TblConfig.Title + "\" />";
            meta += "<meta property=\"og:type\" content=\"product\" />";
            meta += "<meta property=\"og:url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta property=\"og:image\" content=\"http://Kangaroochinhhang.com" + TblConfig.Logo + "\" />";
            meta += "<meta property=\"og:site_name\" content=\"http://Kangaroochinhhang.com\" />";
            meta += "<meta property=\"og:description\" content=\"" + TblConfig.Description + "\" />";
            meta += "<meta property=\"fb:admins\" content=\"\" />";
            ViewBag.Meta = meta;
            ViewBag.content = TblConfig.Content;
            if (Session["Register"] != "")
            {
                ViewBag.Register = Session["Register"];

            }
            Session["Register"] = "";
            ViewBag.h1 = TblConfig.Title;
            return View();
        }
        public PartialViewResult ProductNewsHomes()
        {
            string chuoisp = "";
            string chuoinew = "";
            var listproduct = db.tblProducts.Where(p => p.Active == true && p.ViewHomes == true).OrderByDescending(p => p.DateCreate).Take(8).ToList();
            for (int i = 0; i < listproduct.Count; i++)
            {
                chuoisp += "<div class=\"Tear_pdn\">";
                chuoisp += "<div class=\"img\">";
                chuoisp += "<a href=\"/1/" + listproduct[i].Tag + "\" title=\"" + listproduct[i].Name + "\"><img src=\"" + listproduct[i].ImageLinkThumb + "\" title=\"" + listproduct[i].Name + "\" /></a>";
                chuoisp += "</div>";
                chuoisp += "<h2><a href=\"/1/" + listproduct[i].Tag + "\" title=\"" + listproduct[i].Name + "\">" + listproduct[i].Name + "</a></h2>";
                chuoisp += "</div>";
            }
            ViewBag.chuoisp = chuoisp;
            var listnew = db.tblNews.Where(p => p.Active == true && p.idCate == 7).OrderByDescending(p => p.Ord).Take(5).ToList();
            for (int i = 0; i < listnew.Count; i++)
            {
                chuoinew += "<div class=\"Tear_N\">";
                chuoinew += "<a href=\"/3/" + listnew[i].Tag + "\" title=\"" + listnew[i].Name + "\"><img src=\"" + listnew[i].Images + "\" alt=\"" + listnew[i].Name + "\" /></a>";
                chuoinew += " <h3><a href=\"/3/" + listnew[i].Tag + "\" title=\"" + listnew[i].Name + "\" class=\"Name\">" + listnew[i].Name + " </a></h3>";
                chuoinew += "</div>";
            }
            ViewBag.chuoinew = chuoinew;
            return PartialView();
        }
        public PartialViewResult partialdefault()
        {
            return PartialView(db.TblConfigs.First());
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kangaroochinhhang.Models;
namespace Kangaroochinhhang.Controllers.Display.Header
{
    public class HeaderController : Controller
    {
        KangaroochinhhangContext db = new KangaroochinhhangContext();
        //
        // GET: /Header/
        public ActionResult Index()
        {
            return View();
        }
        [OutputCache(Duration = 2400)]

        public PartialViewResult BannerPatial()
        {
            TblConfig TblConfig = db.TblConfigs.First();
            var listmenu = db.tblGroupProducts.Where(p => p.Active == true && p.ParentID==null).OrderBy(p => p.Ord).ToList();
            string chuoi = "";
            for (int i = 0; i < listmenu.Count; i++)
            {
                chuoi += "<li class=\"li2\">";
                chuoi += "<a href=\"/0/" + listmenu[i].Tag + "\" title=\"" + listmenu[i].Name + "\">› " + listmenu[i].Name + "</a>";
                int idcate = listmenu[i].id;
                var listmenuchild = db.tblGroupProducts.Where(p => p.Active == true && p.ParentID== idcate).OrderBy(p => p.Ord).ToList();
                if (listmenuchild.Count > 0)
                {
                    chuoi += "<ul class=\"ul3\">";
                    for (int j = 0; j < listmenuchild.Count; j++)
                    {
                        chuoi += "<li class=\"li3\">";
                        chuoi += "<a href=\"/0/" + listmenuchild[j].Tag + "\" title=\"" + listmenuchild[j].Name + "\">" + listmenuchild[j].Name + "</a>";
                        chuoi += "</li>";
                    }
                    chuoi += "</ul>";
                }
                chuoi += "</li>";
            }
            ViewBag.chuoi = chuoi;
            string baogia = "";
            var listbaogia = db.tblGroupProducts.Where(p => p.Active == true && p.Baogia == true).OrderBy(p => p.Ord).ToList();
            for (int i = 0; i < listbaogia.Count; i++)
            {
                baogia += "<li class=\"li2\">";
                baogia += "<a href=\"/Bao-gia/Bao-gia-" + listbaogia[i].Tag + "\" title=\"Bảng Báo giá " + listbaogia[i].Name + "\">› Bảng báo giá " + listbaogia[i].Name + "</a>";
                baogia += "</li>";
            }
            ViewBag.baogia = baogia;
            return PartialView(TblConfig);
        }
	}
}
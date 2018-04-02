using PagedList;
using PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kangaroochinhhang.Models;

namespace Kangaroochinhhang.Controllers.Display.Section.Agency
{
    public class AgencyDisplayController : Controller
    {
        //
        // GET: /AgencyDisplay/
        KangaroochinhhangContext db = new KangaroochinhhangContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AgencyDetail(string tag)
        {

            tblAgency tblagency = db.tblAgencies.First(p => p.Tag == tag);
            ViewBag.Title = "<title>" + tblagency.Name + "</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"" + tblagency.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + tblagency.Name + "\" /> ";
            ViewBag.imageog = "<meta property=\"og:image\" content=\"http://Kangaroochinhhang.com" + tblagency.Images + "\"/>";
            ViewBag.titleog = "<meta property=\"og:title\" content=\"" + tblagency.Name + "\"/> ";
            ViewBag.site_nameog = "<meta property=\"og:site_name\" content=\"" + tblagency.Description + "\"/> ";
            ViewBag.urlog = "<meta property=\"og:url\" content=\"" + Request.Url.ToString() + "\"/> ";
            ViewBag.descriptionog = "<meta property=\"og:description\" content=\"" + tblagency.Description + "\" />";
            string meta = "";
            ViewBag.canonical = "<link rel=\"canonical\" href=\"http://Kangaroochinhhang.com/5/"+tblagency.Tag+"\" />";

            meta += "<meta itemprop=\"name\" content=\"" + tblagency.Name + "\" />";
            meta += "<meta itemprop=\"url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta itemprop=\"description\" content=\"" + tblagency.Description + "\" />";
            meta += "<meta itemprop=\"image\" content=\"http://Kangaroochinhhang.com" + tblagency.Images + "\" />";
            meta += "<meta property=\"og:title\" content=\"" + tblagency.Name + "\" />";
            meta += "<meta property=\"og:type\" content=\"product\" />";
            meta += "<meta property=\"og:url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta property=\"og:image\" content=\"http://Kangaroochinhhang.com" + tblagency.Images + "\" />";
            meta += "<meta property=\"og:site_name\" content=\"http://Kangaroochinhhang.com\" />";
            meta += "<meta property=\"og:description\" content=\"" + tblagency.Description + "\" />";
            meta += "<meta property=\"fb:admins\" content=\"\" />";
            ViewBag.Meta = meta;


            int iduser = int.Parse(tblagency.idUser.ToString());
            ViewBag.User = db.tblUsers.First(p => p.id == iduser).UserName;
            int idcate = int.Parse(tblagency.idMenu.ToString());
            DateTime thoigian = Convert.ToDateTime(tblagency.DateCreate);
            var listgroup = db.tblAgencies.Where(p => p.Active == true && p.idMenu == idcate && p.DateCreate < thoigian && p.Tag!=tag).Take(10).ToList();
            string chuoiag = "";
            for(int i=0;i<listgroup.Count;i++)
            {
                chuoiag += " <a href=\"/5/" + listgroup[i].Tag + "\" title=\"" + listgroup[i].Name + "\">- " + listgroup[i].Name + "</a>";
            }
            ViewBag.chuoiag = chuoiag;
            var groupagency = db.tblGroupAgencies.First(p => p.id == idcate);
            ViewBag.name = groupagency.Name;
            int dodai = groupagency.Level.Length / 5;
            string nUrl = "";
            for (int i = 0; i < dodai; i++)
            {
                int leht = groupagency.Level.Substring(0, (i + 1) * 5).Length;
                var NameGroups = db.tblGroupAgencies.First(p => p.Level.Substring(0, (i + 1) * 5) == groupagency.Level.Substring(0, (i + 1) * 5) && p.Level.Length == (i + 1) * 5);
                nUrl = nUrl + "  <a href=\"/4/" + NameGroups.Tag + "\" title=\"" + NameGroups.Name + "\"> " + " " + NameGroups.Name + " /</a>  ";
            }
            ViewBag.nUrl = " <a href=\"/\" title=\"Trang chu\" rel=\"nofollow\"><span class=\"icon_Homes\"></span>Trang chủ /</a> " + nUrl + " ";

            string tab = tblagency.Tabs;
            string Tabsnews = "";
            if (tab != null)
            {
                List<int> mangidPd = new List<int>();

                string[] mang = tab.Split(',');
                int id = int.Parse(tblagency.id.ToString());
                string chuoitab = "";
                Tabsnews = "<div class=\"Tabs\">";
                Tabsnews += " <span>Tags</span>";
                for (int i = 0; i < mang.Length; i++)
                {

                    Tabsnews += " <a href=\"/TabAgency/" + mang[i] + "\" title=\"" + mang[i] + "\">" + mang[i] + "</a>";

                    string tabs = mang[i];
                    var lnews = db.tblAgencies.Where(p => p.Tabs.Contains(tabs) && p.Active == true).ToList();
                    for (int j = 0; j < lnews.Count; j++)
                    {
                        int idn = int.Parse(lnews[j].id.ToString());
                        mangidPd.Add(idn);
                    }

                }
                Tabsnews += "</div>";
                ViewBag.tabsnews = Tabsnews;
                var listnews = db.tblAgencies.Where(p => mangidPd.Contains(p.id) && p.Active == true && p.id != id).OrderByDescending(p => p.DateCreate).Take(2).ToList();
                if (listnews.Count > 0)
                {
                    chuoitab += "<div class=\"Tintuclienquan\">";
                    for (int j = 0; j < listnews.Count; j++)
                    {
                        chuoitab += " <a href=\"/5/" + listnews[j].Tag + "\" title=\"" + listnews[j].Name + "\">› " + listnews[j].Name + "</a>";

                    } chuoitab += " </div>";
                }
                ViewBag.chuoitab = chuoitab;
            }
            ViewBag.favicon = " <link href=\"" + db.TblConfigs.First().Favicon + "\" rel=\"icon\" type=\"image/x-icon\" />";
            var listGroupAgency = db.tblGroupAgencies.Where(p => p.Active == true).OrderByDescending(p => p.Ord).ToList();
            string chuoi = "";
            for (int i = 0; i < listGroupAgency.Count; i++)
            {
                chuoi += "<a href=\"/4/" + listGroupAgency[i].Tag + "\" title=\"" + listGroupAgency[i].Name + "\">› " + listGroupAgency[i].Name + "</a>";
            }
            ViewBag.chuoi = chuoi;
            return View(tblagency);
        }
        public ActionResult AgencyList(string tag, int? page)
        {
            if (tag != null)
            {
                tblGroupAgency groupagency = db.tblGroupAgencies.First(p => p.Tag == tag);
                ViewBag.Title = "<title>Dịch vụ thay lõi lọc nước của VINCOM tại " + groupagency.Name + "</title>";
                ViewBag.Description = "<meta name=\"description\" content=\"" + groupagency.Description + "\"/>";
                ViewBag.Keyword = "<meta name=\"keywords\" content=\"Đại lý của VINCOM tại " + groupagency.Name + "\" /> ";
                int idcate = int.Parse(groupagency.id.ToString());
                ViewBag.Name = groupagency.Name;
                var ListAgency = db.tblAgencies.Where(p => p.idMenu == idcate && p.Active == true).OrderByDescending(p => p.DateCreate).ToList();
                const int pageSize = 20;
                var pageNumber = (page ?? 1);
                // Thiết lập phân trang
                var ship = new PagedListRenderOptions
                {
                    DisplayLinkToFirstPage = PagedListDisplayMode.Always,
                    DisplayLinkToLastPage = PagedListDisplayMode.Always,
                    DisplayLinkToPreviousPage = PagedListDisplayMode.Always,
                    DisplayLinkToNextPage = PagedListDisplayMode.Always,
                    DisplayLinkToIndividualPages = true,
                    DisplayPageCountAndCurrentLocation = false,
                    MaximumPageNumbersToDisplay = 5,
                    DisplayEllipsesWhenNotShowingAllPageNumbers = true,
                    EllipsesFormat = "&#8230;",
                    LinkToFirstPageFormat = "Trang đầu",
                    LinkToPreviousPageFormat = "«",
                    LinkToIndividualPageFormat = "{0}",
                    LinkToNextPageFormat = "»",
                    LinkToLastPageFormat = "Trang cuối",
                    PageCountAndCurrentLocationFormat = "Page {0} of {1}.",
                    ItemSliceAndTotalFormat = "Showing items {0} through {1} of {2}.",
                    FunctionToDisplayEachPageNumber = null,
                    ClassToApplyToFirstListItemInPager = null,
                    ClassToApplyToLastListItemInPager = null,
                    ContainerDivClasses = new[] { "pagination-container" },
                    UlElementClasses = new[] { "pagination" },
                    LiElementClasses = Enumerable.Empty<string>()
                };
                ViewBag.ship = ship;
                ViewBag.name = groupagency.Name;
                int dodai = groupagency.Level.Length / 5;
                string nUrl = "";
                for (int i = 0; i < dodai; i++)
                {
                    int leht = groupagency.Level.Substring(0, (i + 1) * 5).Length;
                    var NameGroups = db.tblGroupAgencies.First(p => p.Level.Substring(0, (i + 1) * 5) == groupagency.Level.Substring(0, (i + 1) * 5) && p.Level.Length == (i + 1) * 5);
                    nUrl = nUrl + "  <a href=\"/4/" + NameGroups.Tag + "\" title=\"" + NameGroups.Name + "\"> " + " " + NameGroups.Name + " /</a>  ";
                }
                ViewBag.nUrl = " <a href=\"/\" title=\"Trang chu\" rel=\"nofollow\"><span class=\"icon_Homes\"></span>Trang chủ /</a> " + nUrl + " ";
                ViewBag.favicon = " <link href=\"" + db.TblConfigs.First().Favicon + "\" rel=\"icon\" type=\"image/x-icon\" />";
                var listGroupAgency = db.tblGroupAgencies.Where(p => p.Active == true).OrderByDescending(p => p.Ord).ToList();
                string chuoi = "";
                for (int i = 0; i < listGroupAgency.Count; i++)
                {
                    chuoi += "<a href=\"/4/" + listGroupAgency[i].Tag + "\" title=\"" + listGroupAgency[i].Name + "\">› " + listGroupAgency[i].Name + "</a>";
                }
                ViewBag.chuoi = chuoi;
                string chuoilienquan = "";
                int idMenu=int.Parse(groupagency.id.ToString());
                var listAgebcys = db.tblAgencies.Where(p => p.Active == true && p.idMenu != idMenu).OrderByDescending(p => p.DateCreate).Take(10).ToList();
                for (int i = 0; i < listAgebcys.Count;i++ )
                {
                    chuoilienquan += "<a href=\"/5/" + listAgebcys[i].Tag + "\" title=\"" + listAgebcys[i].Name + "\">› " + listAgebcys[i].Name + "</a>";
                }
                ViewBag.chuoilienquan = chuoilienquan;
                    return View(ListAgency.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                //tblGroupAgency groupagency = db.tblGroupAgencies.First(p => p.Tag == tag);
                ViewBag.Title = "<title>Dịch vụ thay lõi lọc nước của VINCOM</title>";
                ViewBag.Description = "<meta name=\"description\" content=\"Tổng hợp danh sách đại lý phân phối thiết bị lọc nước, máy lọc nước, lõi lọc nước của VINCOM tại các tỉnh thành trên cả nước\"/>";
                ViewBag.Keyword = "<meta name=\"keywords\" content=\"Đại lý của VINCOM, máy lọc nước tại VINCOM, cửa hàng của VINCOM\" /> ";

                ViewBag.Name = "Danh sách đại lý phân phối của VINCOM";
                var ListAgency = db.tblAgencies.Where(p=>p.Active == true).OrderByDescending(p => p.DateCreate).ToList();
                const int pageSize = 20;
                var pageNumber = (page ?? 1);
                // Thiết lập phân trang
                var ship = new PagedListRenderOptions
                {
                    DisplayLinkToFirstPage = PagedListDisplayMode.Always,
                    DisplayLinkToLastPage = PagedListDisplayMode.Always,
                    DisplayLinkToPreviousPage = PagedListDisplayMode.Always,
                    DisplayLinkToNextPage = PagedListDisplayMode.Always,
                    DisplayLinkToIndividualPages = true,
                    DisplayPageCountAndCurrentLocation = false,
                    MaximumPageNumbersToDisplay = 5,
                    DisplayEllipsesWhenNotShowingAllPageNumbers = true,
                    EllipsesFormat = "&#8230;",
                    LinkToFirstPageFormat = "Trang đầu",
                    LinkToPreviousPageFormat = "«",
                    LinkToIndividualPageFormat = "{0}",
                    LinkToNextPageFormat = "»",
                    LinkToLastPageFormat = "Trang cuối",
                    PageCountAndCurrentLocationFormat = "Page {0} of {1}.",
                    ItemSliceAndTotalFormat = "Showing items {0} through {1} of {2}.",
                    FunctionToDisplayEachPageNumber = null,
                    ClassToApplyToFirstListItemInPager = null,
                    ClassToApplyToLastListItemInPager = null,
                    ContainerDivClasses = new[] { "pagination-container" },
                    UlElementClasses = new[] { "pagination" },
                    LiElementClasses = Enumerable.Empty<string>()
                };
                ViewBag.ship = ship;
               
                ViewBag.nUrl = " <a href=\"/\" title=\"Trang chu\" rel=\"nofollow\"><span class=\"icon_Homes\"></span>Trang chủ /</a> ";
                ViewBag.favicon = " <link href=\"" + db.TblConfigs.First().Favicon + "\" rel=\"icon\" type=\"image/x-icon\" />";
                var listGroupAgency = db.tblGroupAgencies.Where(p => p.Active == true).OrderByDescending(p => p.Ord).ToList();
                string chuoi = "";
                for (int i = 0; i < listGroupAgency.Count;i++ )
                {
                    chuoi += "<a href=\"/4/" + listGroupAgency[i].Tag + "\" title=\"" + listGroupAgency[i].Name + "\">› " + listGroupAgency[i].Name + "</a>";
                }
                ViewBag.chuoi = chuoi;
                string chuoilienquan = "";
               
                var listAgebcys = db.tblAgencies.Where(p => p.Active == true ).OrderByDescending(p => p.DateCreate).Take(10).ToList();
                for (int i = 0; i < listAgebcys.Count; i++)
                {
                    chuoilienquan += "<a href=\"/5/" + listAgebcys[i].Tag + "\" title=\"" + listAgebcys[i].Name + "\">› " + listAgebcys[i].Name + "</a>";
                }
                ViewBag.chuoilienquan = chuoilienquan;
                    return View(ListAgency.ToPagedList(pageNumber, pageSize));

            }
               
         }
        public ActionResult TabsAgency(string tag, int? page)
        {
           var ListAgency = db.tblAgencies.Where(p => p.Tabs.Contains(tag) && p.Active==true).OrderByDescending(p=>p.DateCreate).ToList();
            const int pageSize = 20;
            var pageNumber = (page ?? 1);
            // Thiết lập phân trang
            var ship = new PagedListRenderOptions
            {
                DisplayLinkToFirstPage = PagedListDisplayMode.Always,
                DisplayLinkToLastPage = PagedListDisplayMode.Always,
                DisplayLinkToPreviousPage = PagedListDisplayMode.Always,
                DisplayLinkToNextPage = PagedListDisplayMode.Always,
                DisplayLinkToIndividualPages = true,
                DisplayPageCountAndCurrentLocation = false,
                MaximumPageNumbersToDisplay = 5,
                DisplayEllipsesWhenNotShowingAllPageNumbers = true,
                EllipsesFormat = "&#8230;",
                LinkToFirstPageFormat = "Trang đầu",
                LinkToPreviousPageFormat = "«",
                LinkToIndividualPageFormat = "{0}",
                LinkToNextPageFormat = "»",
                LinkToLastPageFormat = "Trang cuối",
                PageCountAndCurrentLocationFormat = "Page {0} of {1}.",
                ItemSliceAndTotalFormat = "Showing items {0} through {1} of {2}.",
                FunctionToDisplayEachPageNumber = null,
                ClassToApplyToFirstListItemInPager = null,
                ClassToApplyToLastListItemInPager = null,
                ContainerDivClasses = new[] { "pagination-container" },
                UlElementClasses = new[] { "pagination" },
                LiElementClasses = Enumerable.Empty<string>()
            };
            ViewBag.ship = ship;
            ViewBag.name = tag;

            ViewBag.Title = "<title>" + tag + "</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"" + tag + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + tag + "\" /> ";
            ViewBag.favicon = " <link href=\"" + db.TblConfigs.First().Favicon + "\" rel=\"icon\" type=\"image/x-icon\" />";
            return View(ListAgency.ToPagedList(pageNumber, pageSize));

            }

        
	}
}
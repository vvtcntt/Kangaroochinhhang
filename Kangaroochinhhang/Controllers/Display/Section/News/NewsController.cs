using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kangaroochinhhang.Models;
using PagedList;
using PagedList.Mvc;
using System.Text;

namespace Kangaroochinhhang.Controllers.Display.Section.News
{
    public class NewsController : Controller
    {
        //
        // GET: /News/
        KangaroochinhhangContext db = new KangaroochinhhangContext();
        public ActionResult Index()
        {
            return View();
        }
        string nUrl = "";
        public string UrlNews(int idCate)
        {
            var ListMenu = db.tblGroupNews.Where(p => p.id == idCate).ToList();
            for (int i = 0; i < ListMenu.Count; i++)
            {
                nUrl = " <li itemprop=\"itemListElement\" itemscope itemtype=\"http://schema.org/ListItem\"> <a itemprop=\"item\" href=\"/" + ListMenu[i].Tag + ".html\"> <span itemprop=\"name\">" + ListMenu[i].Name + "</span></a> <meta itemprop=\"position\" content=\"\" /> </li> › " + nUrl;
                string ids = ListMenu[i].ParentID.ToString();
                if (ids != null && ids != "")
                {
                    int id = int.Parse(ListMenu[i].ParentID.ToString());
                    UrlNews(id);
                }
            }
            return nUrl;
        }
        public ActionResult NewsDetail(string tag)
        {
            tblNew dblnew = db.tblNews.First(p => p.Tag == tag);
            ViewBag.Title = "<title>" + dblnew.Title + "</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"" + dblnew.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + dblnew.Keyword + "\" /> ";
            ViewBag.dcTitle = "<meta name=\"DC.title\" content=\"" + dblnew.Title + "\" />";
            if(dblnew.Meta!=null && dblnew.Meta!="")
            {
                int phut = DateTime.Now.Minute * 2;
                ViewBag.refresh = "<meta http-equiv=\"refresh\" content=\"" + phut + "; url=" + dblnew.Meta + "\">"; 
            }
            ViewBag.dcDescription = "<meta name=\"DC.description\" content=\"" + dblnew.Description + "\" />";
            string meta = "";
            ViewBag.canonical = "<link rel=\"canonical\" href=\"http://Kangaroochinhhang.com/3/" + StringClass.NameToTag(tag) + "\" />";

            meta += "<meta itemprop=\"name\" content=\"" + dblnew.Name + "\" />";
            meta += "<meta itemprop=\"url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta itemprop=\"description\" content=\"" + dblnew.Description + "\" />";
            meta += "<meta itemprop=\"image\" content=\"http://Kangaroochinhhang.com" + dblnew.Images + "\" />";
            meta += "<meta property=\"og:title\" content=\"" + dblnew.Title + "\" />";
            meta += "<meta property=\"og:type\" content=\"product\" />";
            meta += "<meta property=\"og:url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta property=\"og:image\" content=\"http://Kangaroochinhhang.com" + dblnew.Images + "\" />";
            meta += "<meta property=\"og:site_name\" content=\"http://Kangaroochinhhang.com\" />";
            meta += "<meta property=\"og:description\" content=\"" + dblnew.Description + "\" />";
            meta += "<meta property=\"fb:admins\" content=\"\" />";
            ViewBag.Meta = meta;


            StringBuilder schame = new StringBuilder();
            schame.Append("<script type=\"application/ld+json\">");
            schame.Append("{");
            schame.Append("\"@context\": \"http://schema.org\",");
            schame.Append("\"@type\": \"NewsArticle\",");
            schame.Append("\"headline\": \"" + dblnew.Description + "\",");
            schame.Append(" \"datePublished\": \"" + dblnew.DateCreate + "\",");
            schame.Append("\"image\": [");
            schame.Append(" \"" + dblnew.Images + "\"");
            schame.Append(" ]");
            schame.Append("}");
            schame.Append("</script> ");
            ViewBag.schame = schame.ToString();
            int iduser = int.Parse(dblnew.idUser.ToString());
            ViewBag.User = db.tblUsers.First(p => p.id == iduser).UserName;
            int idcate = int.Parse(dblnew.idCate.ToString());
            var GroupNews = db.tblGroupNews.First(p => p.id == idcate);
            ViewBag.name = GroupNews.Name;
             ViewBag.nUrl = "<ol itemscope itemtype=\"http://schema.org/BreadcrumbList\">   <li itemprop=\"itemListElement\" itemscope  itemtype=\"http://schema.org/ListItem\"> <a itemprop=\"item\" href=\"http://Kangaroochinhhang.com\">  <span itemprop=\"name\">Trang chủ</span></a> <meta itemprop=\"position\" content=\"1\" />  </li>   ›" + UrlNews(idcate) + "</ol> ";


            string tab = dblnew.Tabs;
           string Tabsnews = "";
           if (tab != null)
           {  
               List<int> mangidPd = new List<int>();

               string[] mang = tab.Split(',');
               int id = int.Parse(dblnew.id.ToString());
               string chuoitab = "";
               Tabsnews="<div class=\"Tabs\">";
               Tabsnews += " <span>Tags</span>";
               for (int i = 0; i < mang.Length; i++)
               {

                   Tabsnews += "<h2> <a href=\"/TagNews/" + StringClass.NameToTag(mang[i]) + "\" title=\"" + mang[i] + "\">" + mang[i] + "</a></h2>";
                
                   string tabs = mang[i];
                   var lnews = db.tblNews.Where(p => p.Tabs.Contains(tabs) && p.Active==true).ToList();
                   for(int j=0;j<lnews.Count;j++)
                   {
                       int idn=int.Parse(lnews[j].id.ToString());
                       mangidPd.Add(idn);
                   }

               }
               Tabsnews+="</div>";
               ViewBag.tabsnews = Tabsnews;
               var listnews = db.tblNews.Where(p => mangidPd.Contains(p.id) && p.Active == true && p.id!=id).OrderByDescending(p => p.DateCreate).Take(2).ToList();
               if (listnews.Count > 0)
               {
                   chuoitab += "<div class=\"Tintuclienquan\">";
                   for (int j = 0; j < listnews.Count; j++)
                   {
                       chuoitab += " <h3><a href=\"/3/" + listnews[j].Tag + "\" title=\"" + listnews[j].Name + "\">› " + listnews[j].Name + "</a><h3>";

                   } chuoitab += " </div>";
               }
               ViewBag.chuoitab = chuoitab;
           }
           ViewBag.favicon = " <link href=\"" + db.TblConfigs.First().Favicon + "\" rel=\"icon\" type=\"image/x-icon\" />";
            return View(dblnew);
        }
        public PartialViewResult RightNews()
        {
            return PartialView();
        }
        public PartialViewResult Tintuclienquan(string tag)
        {
            tblNew tblnew = db.tblNews.First(p => p.Tag == tag);

            int idcate = int.Parse(tblnew.idCate.ToString());
            string chuoi = "";
            var listnew = db.tblNews.Where(p => p.idCate == idcate && p.Active == true && p.Tag != tag).OrderByDescending(p => p.DateCreate).Take(10).ToList();
            for (int i = 0; i < listnew.Count; i++)
            {
                chuoi += "    <h3><a href=\"/3/" + listnew[i].Tag + "\" rel=\"nofollow\" title=\"" + listnew[i].Name + "\">" + listnew[i].Name + " </a></h3> ";
            }
            ViewBag.chuoi = chuoi;
            return PartialView();
        }
        public ActionResult ListNews(string tag, int? page)
        {
            tblGroupNew groupnews = db.tblGroupNews.First(p => p.Tag == tag);
            ViewBag.Title = "<title>" + groupnews.Title + "</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"" + groupnews.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + groupnews.Keyword + "\" /> ";
            int idcate = int.Parse(groupnews.id.ToString());
            ViewBag.Name = groupnews.Name;
            var Listnews = db.tblNews.Where(p => p.idCate == idcate && p.Active==true).OrderByDescending(p => p.DateCreate).ToList();
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
            ViewBag.name = groupnews.Name;

            ViewBag.nUrl = "<ol itemscope itemtype=\"http://schema.org/BreadcrumbList\">   <li itemprop=\"itemListElement\" itemscope  itemtype=\"http://schema.org/ListItem\"> <a itemprop=\"item\" href=\"http://Kangaroochinhhang.com\">  <span itemprop=\"name\">Trang chủ</span></a> <meta itemprop=\"position\" content=\"1\" />  </li>   ›" + UrlNews(idcate) + "</ol> ";
            ViewBag.favicon = " <link href=\"" + db.TblConfigs.First().Favicon + "\" rel=\"icon\" type=\"image/x-icon\" />";
            return View(Listnews.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Tabsnews(string tag, int? page)
        {
            tag = StringClass.NameToTag(tag); ;
            string[] Mang1 = StringClass.COnvertToUnSign1(tag).Split('-');
            string chuoitag = "";
            for (int i = 0; i < Mang1.Length; i++)
            {
                if (i == 0)
                    chuoitag += Mang1[i];
                else
                    chuoitag += " " + Mang1[i];
            }
            int dem = 1;
            string name = "";
            List<tblNew> ListNew = (from c in db.tblNews where c.Active == true select c).ToList();
            List<tblNew> listnews = ListNew.FindAll(delegate(tblNew math)
            {
                if(math.Tabs!=null&& math.Tabs!="")
                {
                    if (StringClass.COnvertToUnSign1(math.Tabs.ToUpper()).Contains(chuoitag.ToUpper()))
                    {

                        string[] Manghienthi = math.Tabs.Split(',');
                        foreach (var item in Manghienthi)
                        {
                            if (dem == 1)
                            {
                                var kiemtra = StringClass.COnvertToUnSign1(item.ToUpper()).Contains(chuoitag.ToUpper());
                                if (kiemtra == true)
                                {
                                    name = item;
                                    dem = 0;
                                }
                            }
                        }

                        return true;
                    }
                    else
                        return false;
                }
              

                else
                    return false;
            }
            );
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
            ViewBag.name = name;

            ViewBag.Title = "<title>" + name + "</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"" + name + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + name + "\" /> ";
            ViewBag.favicon = " <link href=\"" + db.TblConfigs.First().Favicon + "\" rel=\"icon\" type=\"image/x-icon\" />";
            ViewBag.canonical = "<link rel=\"canonical\" href=\"http://Kangaroochinhhang.com/TagNews/" + StringClass.NameToTag(chuoitag) + "\" />"; ;
            StringBuilder schame = new StringBuilder();
            schame.Append("<script type=\"application/ld+json\">");
            schame.Append("{");
            schame.Append("\"@context\": \"http://schema.org\",");
            schame.Append("\"@type\": \"NewsArticle\",");
            schame.Append("\"headline\": \"" + name + "\",");
            schame.Append(" \"datePublished\": \"\",");
            schame.Append("\"image\": [");
            schame.Append(" \"\"");
            schame.Append(" ]");
            schame.Append("}");
            schame.Append("</script> ");
            ViewBag.schame = schame.ToString();
            return View(listnews.ToPagedList(pageNumber, pageSize));
         }
        public PartialViewResult Boxfacebook()
        {
            TblConfig TblConfig = db.TblConfigs.First();

            return PartialView(TblConfig);
        }
	}
}
using PagedList;
using PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Kangaroochinhhang.Models;
namespace Kangaroochinhhang.Controllers.DisplayCustom
{
    public class newsCustomController : Controller
    {
        private KangaroochinhhangContext db = new KangaroochinhhangContext();
        // GET: newsCustom
        public ActionResult Index()
        {
            return View();
        }
        private string nUrl = "";

        public string Urlnews(int idCate)
        {
            var ListMenu = db.tblGroupNews.Where(p => p.id == idCate).ToList();
            for (int i = 0; i < ListMenu.Count; i++)
            {
                nUrl = " <li itemprop=\"itemListElement\" itemscope itemtype=\"http://schema.org/ListItem\"> <a itemprop=\"item\" href=\"/2/" + ListMenu[i].Tag + "\"> <span itemprop=\"name\">" + ListMenu[i].Name + "</span></a> <meta itemprop=\"position\" content=\"\" /> </li>  " + nUrl;
                string ids = ListMenu[i].ParentID.ToString();
                if (ids != null && ids != "")
                {
                    int id = int.Parse(ListMenu[i].ParentID.ToString());
                    Urlnews(id);
                }
            }
            return nUrl;
        }

        public ActionResult newsDetail(string tag)
        {
            tblNew tblnews = db.tblNews.First(p => p.Tag == tag);
            int id = tblnews.id;
            int idmenu = int.Parse(tblnews.idCate.ToString());
            tblGroupNew groupNews = db.tblGroupNews.First(p => p.id == idmenu);
            ViewBag.menuName = groupNews.Name;
            ViewBag.tagMenuName = groupNews.Tag;
            var listId = db.tblConnectManuToNews.Where(p => p.idNews == id).Select(p => p.idManu).Take(1).ToList();
            Session["idManu"] = "";
            int idManu = int.Parse(listId[0].Value.ToString());
            var manufactures = db.SelectListItem.Find(idManu);
            ViewBag.manuName = manufactures.Name;
            ViewBag.color = manufactures.Color;
            Session["idManu"] = idManu;
            if(tblnews.Style==true)
            {
                ViewBag.style = "width:100% !important; margin:0px";
                ViewBag.style1= "display:none";
            }
            ViewBag.Title = "<title>" + tblnews.Title + "</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"" + tblnews.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + tblnews.Keyword + "\" /> ";
            ViewBag.dcTitle = "<meta name=\"DC.title\" content=\"" + tblnews.Title + "\" />";
            if (tblnews.Meta != null && tblnews.Meta != "")
            {
                int phut = DateTime.Now.Minute * 2;
                ViewBag.refresh = "<meta http-equiv=\"refresh\" content=\"" + phut + "; url=" + tblnews.Meta + "\">";
            }
            ViewBag.dcDescription = "<meta name=\"DC.description\" content=\"" + tblnews.Description + "\" />";
            string meta = "";
            ViewBag.canonical = "<link rel=\"canonical\" href=\"http://Kangaroochinhhang.com/3/" + StringClass.NameToTag(tag) + "\" />";

            meta += "<meta itemprop=\"name\" content=\"" + tblnews.Name + "\" />";
            meta += "<meta itemprop=\"url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta itemprop=\"description\" content=\"" + tblnews.Description + "\" />";
            meta += "<meta itemprop=\"image\" content=\"http://Kangaroochinhhang.com" + tblnews.Images + "\" />";
            meta += "<meta property=\"og:title\" content=\"" + tblnews.Title + "\" />";
            meta += "<meta property=\"og:type\" content=\"product\" />";
            meta += "<meta property=\"og:url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta property=\"og:image\" content=\"http://Kangaroochinhhang.com" + tblnews.Images + "\" />";
            meta += "<meta property=\"og:site_name\" content=\"http://Kangaroochinhhang.com\" />";
            meta += "<meta property=\"og:description\" content=\"" + tblnews.Description + "\" />";
            meta += "<meta property=\"fb:admins\" content=\"\" />";
            ViewBag.Meta = meta;
            StringBuilder schame = new StringBuilder();
            schame.Append("<script type=\"application/ld+json\">");
            schame.Append("{");
            schame.Append("\"@context\": \"http://schema.org\",");
            schame.Append("\"@type\": \"NewsArticle\",");
            schame.Append("\"headline\": \"" + tblnews.Description + "\",");
            schame.Append(" \"datePublished\": \"" + tblnews.DateCreate + "\",");
            schame.Append("\"image\": [");
            schame.Append(" \"" + tblnews.Images + "\"");
            schame.Append(" ]");
            schame.Append("}");
            schame.Append("</script> ");
            ViewBag.schame = schame.ToString();
            int iduser = int.Parse(tblnews.idUser.ToString());
            ViewBag.user = db.tblUsers.First(p => p.id == iduser).UserName;
            string tab = tblnews.Tabs;
            string Tabsnews = "";
            if (tab != null)
            {
 
                string[] mang = tab.Split(',');
                for (int i = 0; i < mang.Length; i++)
                {

                    Tabsnews += " <a href=\"/TagNews/" + StringClass.NameToTag(mang[i]) + "\" title=\"" + mang[i] + "\">" + mang[i] + "</a>";

                    

                }
                 ViewBag.tags = Tabsnews;
                
            }
            StringBuilder result = new StringBuilder();
            var listIdNews = db.tblConnectManuToNews.Where(p => listId.Contains(p.idManu)).Select(p => p.idNews).ToList();

            var listnews = db.tblNews.Where(p => listIdNews.Contains(p.id) && p.Active == true && p.id != id).OrderByDescending(p => p.DateCreate).Take(5).ToList();
            if (listnews.Count > 0)
            {
                for (int j = 0; j < listnews.Count; j++)
                {
                    result.Append(" <li><a href=\"/3/" + listnews[j].Tag + "\" title=\"" + listnews[j].Name + "\"><i class=\"fa fa-angle-right\" aria-hidden=\"true\"></i>  " + listnews[j].Name + "</a><span>");
                           result.Append("  <i class=\"fa fa-clock-o\" aria-hidden=\"true\"></i>  Viết ngày : " + listnews[j].DateCreate+"    </span><li>");

                }
            }
            ViewBag.result = result.ToString();
            ViewBag.favicon = " <link href=\"" + manufactures.Favicon + "\" rel=\"icon\" type=\"image/x-icon\" />";
            ViewBag.nUrl = "<ol itemscope itemtype=\"http://schema.org/BreadcrumbList\">   <li itemprop=\"itemListElement\" itemscope  itemtype=\"http://schema.org/ListItem\"> <a itemprop=\"item\" href=\"http://Kangaroochinhhang.com\">  <span itemprop=\"name\">Trang chủ</span></a> <meta itemprop=\"position\" content=\"1\" />  </li>   ›" + Urlnews(idmenu) + "</ol>";
            int visit = int.Parse(tblnews.Visit.ToString());
            if (visit > 0)
            {
                tblnews.Visit = tblnews.Visit + 1;
                db.SaveChanges();
            }
            else
            {
                tblnews.Visit = tblnews.Visit + 1;
                db.SaveChanges();
            }
            return View(tblnews);
        }
        public ActionResult listNews(string tag, int? page)
        {
            tblGroupNew groupnews = db.tblGroupNews.First(p => p.Tag == tag);
            ViewBag.Title = "<title>" + groupnews.Title + "</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"" + groupnews.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + groupnews.Keyword + "\" /> ";
      
            int idcate = int.Parse(groupnews.id.ToString());
            ViewBag.Name = groupnews.Name;
            var Listnews = db.tblNews.Where(p => p.idCate == idcate && p.Active == true).OrderByDescending(p => p.DateCreate).ToList();
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

            ViewBag.nUrl = "<ol itemscope itemtype=\"http://schema.org/BreadcrumbList\">   <li itemprop=\"itemListElement\" itemscope  itemtype=\"http://schema.org/ListItem\"> <a itemprop=\"item\" href=\"http://Kangaroochinhhang.com\">  <span itemprop=\"name\">Trang chủ</span></a> <meta itemprop=\"position\" content=\"1\" />  </li>   ›" + Urlnews(idcate) + "</ol>";
            ViewBag.favicon = " <link href=\"" + db.TblConfigs.First().Favicon + "\" rel=\"icon\" type=\"image/x-icon\" />";

            int idmenu = int.Parse(groupnews.id.ToString());
            if (Session["idManu"]!=null && Session["idManu"]!="")
            {
                ViewBag.layout = "~/Views/Shared/_LayoutDefaultCustom.cshtml";
                int idManu = int.Parse(Session["idManu"].ToString());
                var manufactures = db.SelectListItem.Find(idManu);
                ViewBag.manuName = manufactures.Name;
                ViewBag.color = manufactures.Color;

                var listIdNews = db.tblConnectManuToNews.Where(p => p.idManu==idManu).Select(p => p.idNews).ToList();
                Session["color"] = manufactures.Color;
                Listnews = db.tblNews.Where(p => p.idCate == idcate && p.Active == true && listIdNews.Contains(p.id)).OrderByDescending(p => p.DateCreate).ToList();

            }
            else
            {
                ViewBag.layout = "~/Views/Shared/_LayoutDefault.cshtml";
            }             
            return View(Listnews.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult tagNews(string tag, int? page)
        {
            var listIdNews = db.tblNewsTags.Where(p => p.Tag == tag || p.Name.Contains(tag)).Select(p => p.idn).ToList();
            var listNews = db.tblNews.Where(p => p.Active == true && listIdNews.Contains(p.id)).OrderBy(p => p.Ord).ToList();
            if (Session["idManu"] != null && Session["idManu"] != "")
            {
                int idManu= int.Parse(Session["idManu"].ToString());
                var manufactures = db.SelectListItem.Find(idManu);
                ViewBag.manuName = manufactures.Name;
                ViewBag.color = manufactures.Color;
                var listIdNewss = db.tblConnectManuToNews.Where(p => p.idManu == idManu).Select(p => p.idNews).ToList();
                Session["color"] = manufactures.Color;
                listNews = listNews.Where(p => listIdNewss.Contains(p.id)).OrderByDescending(p => p.Visit).ToList();

            }
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
            string name = tag;
            ViewBag.name = name;    
            ViewBag.Title = "<title>" + name + "</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"" + name + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + name + "\" /> ";
            ViewBag.favicon = " <link href=\"" + db.TblConfigs.First().Favicon + "\" rel=\"icon\" type=\"image/x-icon\" />";
            //ViewBag.canonical = "<link rel=\"canonical\" href=\"http://Kangaroochinhhang.com/TagNews/" + StringClass.NameToTag(tag) + "\" />"; ;
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
            return View(listNews.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult partialMenuRightNews()
        {

            return PartialView();
        }
        public ActionResult partialListNewsRightNews(string tag)
        {
            var tblnews = db.tblNews.FirstOrDefault(p => p.Tag == tag);
            int idCate = int.Parse(tblnews.idCate.ToString());
            int id = tblnews.id;
            var listId = db.tblConnectManuToNews.Where(p => p.idNews == id).Select(p => p.idManu).Take(1).ToList();
            int idManu = int.Parse(listId[0].Value.ToString());
            var manufactures = db.SelectListItem.Find(idManu);
            ViewBag.color = manufactures.Color;
            var listIdNews = db.tblConnectManuToNews.Where(p => listId.Contains(p.idManu)).Select(p => p.idNews).ToList();

            var listNewsLienQuan = db.tblNews.Where( p => p.Active == true && listIdNews.Contains(p.id)).OrderByDescending(p => p.Visit).Take(5).ToList();
            StringBuilder result = new StringBuilder();
            for(int i=0;i<listNewsLienQuan.Count;i++)
            {
                result.Append("<li><a href=\"/3/"+listNewsLienQuan[i].Tag+ "\" title=\"" + listNewsLienQuan[i].Name + "\"><i class=\"fa fa-caret-right\" aria-hidden=\"true\"></i> " + listNewsLienQuan[i].Name + "</a></li>");
            }
            ViewBag.result = result.ToString();


            ///Mới đăng
            var ListNewsNew = db.tblNews.Where(p => p.Active == true && listIdNews.Contains(p.id)).OrderByDescending(p => p.DateCreate).Take(5).ToList();
            StringBuilder resultNews = new StringBuilder();
            for (int i = 0; i < ListNewsNew.Count; i++)
            {
                resultNews.Append("<li><a href=\"/3/" + ListNewsNew[i].Tag + "\" title=\"" + ListNewsNew[i].Name + "\"><i class=\"fa fa-angle-right\" aria-hidden=\"true\"></i> " + ListNewsNew[i].Name + "</a></li>");
            }
            ViewBag.resultNews = resultNews.ToString();
            return PartialView(manufactures);
        }
    }
}
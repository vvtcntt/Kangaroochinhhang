using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kangaroochinhhang.Models;
using PagedList;
using PagedList.Mvc;
using System.Globalization;
using System.Data;
using System.Text;

namespace Kangaroochinhhang.Controllers.Display.Section.Product
{
    public class ProductController : Controller
    {
        //
        // GET: /Product/
        KangaroochinhhangContext db = new KangaroochinhhangContext();
        public ActionResult Index()
        {

            return View();
        }
        string nUrl = "";
        public string UrlProduct(int idCate)
        {
            var ListMenu = db.tblGroupProducts.Where(p => p.id == idCate).ToList();
            for (int i = 0; i < ListMenu.Count; i++)
            {
                nUrl = " <li itemprop=\"itemListElement\" itemscope itemtype=\"http://schema.org/ListItem\"> <a itemprop=\"item\" href=\"/" + ListMenu[i].Tag + ".html\"> <span itemprop=\"name\">" + ListMenu[i].Name + "</span></a> <meta itemprop=\"position\" content=\"\" /> </li> › " + nUrl;
                string ids = ListMenu[i].ParentID.ToString();
                if (ids != null && ids != "")
                {
                    int id = int.Parse(ListMenu[i].ParentID.ToString());
                    UrlProduct(id);
                }
            }
            return nUrl;
        }
        List<string> Mangphantu = new List<string>();
        public List<string> Arrayid(int idParent)
        {

            var ListMenu = db.tblGroupProducts.Where(p => p.ParentID == idParent).ToList();

            for (int i = 0; i < ListMenu.Count; i++)
            {
                Mangphantu.Add(ListMenu[i].id.ToString());
                int id = int.Parse(ListMenu[i].id.ToString());
                Arrayid(id);

            }

            return Mangphantu;
        }
        //void Manufacture(int id)
        //{
        //   int idManu= clsManufactures.idManu(id);
        //    var tblManufacture = db.SelectListItem.FirstOrDefault(p => p.id == idManu);
        //    //ViewBag.Title = "<title>" + tblManufacture.Title + "</title>";
        //    //ViewBag.Description = "<meta name=\"description\" content=\"" + tblManufacture.Description + "\"/>";
        //    ViewBag.Logo = tblManufacture.Images;
        //    ViewBag.NameCompanyCustom=tblManufacture.Company;
        //    ViewBag.LayOut = tblManufacture.Layout;
        //    ViewBag.Color = tblManufacture.Color;
        //    ViewBag.NameCustom = tblManufacture.Name;
        //}
        [OutputCache(CacheProfile = "CacheTag")]
        public ActionResult ListProduct(string tag,int? page)
        {
            var ListProduct = db.tblProducts.ToList();
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
             var MenuParent = db.tblGroupProducts.First(p => p.Tag == tag);
            ViewBag.Title = "<title>" + MenuParent.Title + "</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"" + MenuParent.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + MenuParent.Keyword + "\" /> ";
            ViewBag.favicon = " <link href=\"" + MenuParent.Favicon + "\" rel=\"icon\" type=\"image/x-icon\" />";
            string meta = "";
            ViewBag.canonical = "<link rel=\"canonical\" href=\"http://Kangaroochinhhang.com/0/" + MenuParent.Tag + "\" />";
            meta += "<meta itemprop=\"name\" content=\""+ MenuParent.Name+ "\" />";
            meta += "<meta itemprop=\"url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta itemprop=\"description\" content=\"" + MenuParent.Description + "\" />";
            meta += "<meta itemprop=\"image\" content=\"http://Kangaroochinhhang.com" + MenuParent.Images + "\" />";
            meta += "<meta property=\"og:title\" content=\"" + MenuParent.Title + "\" />";
            meta += "<meta property=\"og:type\" content=\"product\" />";
            meta += "<meta property=\"og:url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta property=\"og:image\" content=\"http://Kangaroochinhhang.com" + MenuParent.Images + "\" />";
            meta += "<meta property=\"og:site_name\" content=\"http://Kangaroochinhhang.com\" />";
            meta += "<meta property=\"og:description\" content=\"" + MenuParent.Description + "\" />";
            meta += "<meta property=\"fb:admins\" content=\"\" />";
            ViewBag.Meta = meta;
            StringBuilder schame = new StringBuilder();
            schame.Append("<script type=\"application/ld+json\">");
            schame.Append("{");
            schame.Append("\"@context\": \"http://schema.org\",");
            schame.Append("\"@type\": \"NewsArticle\",");
            schame.Append("\"headline\": \""+MenuParent.Description+"\",");
            schame.Append(" \"datePublished\": \"" + MenuParent.DateCreate + "\",");
            schame.Append("\"image\": [");
            schame.Append(" \"" + MenuParent.Images + "\"");
            schame.Append(" ]");
            schame.Append("}");
            schame.Append("</script> ");
            ViewBag.schame = schame.ToString();
            string chuoi = "";
            chuoi += "<div id=\"HeadShort\">";
            chuoi += "<div id=\"Left_HeadShort\">";
            chuoi += "<img src=\"" + MenuParent.Images + "\" alt=\"" + MenuParent.Name + "\" title=\"" + MenuParent.Name + "\" />";
            chuoi += "</div>";
            chuoi += "<div id=\"Right_HeadShort\">";
            chuoi += "<div id=\"nVar_HeadShort\">";
            chuoi += "<h1>" + MenuParent.Name + "</h1>";
            chuoi += "</div>";
            chuoi += "<div class=\"line3\"></div>";
            chuoi += "<div id=\"Content_HeadShort\">" + MenuParent.Content + "</div>";
            chuoi += "</div>";
            chuoi += "</div>";
            int idcate = MenuParent.id;
            //Manufacture(idcate);
            var listMenu = db.tblGroupProducts.Where(p => p.ParentID== idcate && p.Active == true ).OrderBy(p => p.Ord).ToList();
            if (listMenu.Count > 0)
            {
                chuoi += "<div class=\"ListProduct\">";
                for (int i = 0; i < listMenu.Count; i++)
                {
                    chuoi += "<div class=\"nVar2\">";
                    chuoi += "<div class=\"Names\"><h2>Danh sách sản phẩm <a href=\"/0/" + listMenu[i].Tag + "\" title=\"" + listMenu[i].Name + "\">" + listMenu[i].Name + "</a></h2></div>";
                    chuoi += "<hr />";
                    chuoi += "</div>";
                    chuoi += "<div class=\"Product_Tear\">";
                    int icate = int.Parse(listMenu[i].id.ToString());
                      ListProduct = db.tblProducts.Where(p => p.idCate == icate && p.Active == true).OrderBy(p => p.Ord).ToList();
                    for (int j = 0; j < ListProduct.Count; j++)
                    {
                        chuoi += "<div class=\"Tear_1\">";
                        chuoi += "<div class=\"img\">";
                        chuoi += "<div class=\"content_img\"><a href=\"/1/" + ListProduct[j].Tag + "\" title=\"" + ListProduct[j].Name + "\"><img src=\"" + ListProduct[j].ImageLinkThumb + "\" alt=\"" + ListProduct[j].Name + "\" /></a></div>";
                        chuoi += "<div class=\"detail\">";
                        chuoi += "<span class=\"title\">" + ListProduct[j].Name + "</span>";
                        chuoi += "<p>" + ListProduct[j].Info+ "</p>";
                        chuoi += "<a href=\"/1/" + ListProduct[j].Tag + "\" title=\"" + ListProduct[j].Name + "\">Xem tiếp</a>";
                        chuoi += "</div>";
                        chuoi += "</div>";
                        chuoi += "<h3><a href=\"/1/" + ListProduct[j].Tag + "\" title=\"" + ListProduct[j].Name + "\" class=\"Name\" >" + ListProduct[j].Name + "</a></h3>";
                        chuoi += "<span class=\"Price\">Giá : " + string.Format("{0:#,#}", ListProduct[j].Price) + "đ</span>";
                        chuoi += "<span class=\"PriceSale\"><img /> " + string.Format("{0:#,#}", ListProduct[j].PriceSale) + "đ</span>";
                        chuoi += " </div>";
                    }
                    chuoi += "</div>";
                }
                chuoi += "</div>";
            }
            else
            {
                chuoi += "<div class=\"ListProduct\">";
               
                    chuoi += "<div class=\"nVar2\">";
                    chuoi += "<div class=\"Names\"><h2>Danh sách sản phẩm <a href=\"/0/" + MenuParent.Tag + "\" title=\"" + MenuParent.Name + "\">" + MenuParent.Name + "</a></h2></div>";
                    chuoi += "<hr />";
                    chuoi += "</div>";
                    chuoi += "<div class=\"Product_Tear\">";
                    int icate = int.Parse(MenuParent.id.ToString());
                         ListProduct = db.tblProducts.Where(p => p.idCate == icate && p.Active == true).OrderBy(p => p.Ord).ToList();
                    for (int j = 0; j < ListProduct.ToPagedList(pageNumber, pageSize).Count; j++)
                    {
                        chuoi += "<div class=\"Tear_1\">";
                        chuoi += "<div class=\"img\">";
                        chuoi += "<div class=\"content_img\"><a href=\"/1/" + ListProduct.ToPagedList(pageNumber, pageSize)[j].Tag + "\" title=\"" + ListProduct.ToPagedList(pageNumber, pageSize)[j].Name + "\"><img src=\"" + ListProduct.ToPagedList(pageNumber, pageSize)[j].ImageLinkThumb + "\" alt=\"" + ListProduct.ToPagedList(pageNumber, pageSize)[j].Name + "\" /></a></div>";
                        chuoi += "<div class=\"detail\">";
                        chuoi += "<span class=\"title\">" + ListProduct.ToPagedList(pageNumber, pageSize)[j].Name + "</span>";
                        chuoi += "<p>" + ListProduct.ToPagedList(pageNumber, pageSize)[j].Info + "</p>";
                        chuoi += "<a href=\"/1/" + ListProduct.ToPagedList(pageNumber, pageSize)[j].Tag + "\" title=\"" + ListProduct.ToPagedList(pageNumber, pageSize)[j].Name + "\">Xem tiếp</a>";
                        chuoi += "</div>";
                        chuoi += "</div>";
                        chuoi += "<h3><a href=\"/1/" + ListProduct.ToPagedList(pageNumber, pageSize)[j].Tag + "\" title=\"" + ListProduct.ToPagedList(pageNumber, pageSize)[j].Name + "\" class=\"Name\" >" + ListProduct.ToPagedList(pageNumber, pageSize)[j].Name + "</a></h3>";
                        chuoi += "<span class=\"Price\">Giá : " + string.Format("{0:#,#}", ListProduct.ToPagedList(pageNumber, pageSize)[j].Price) + "đ</span>";
                        chuoi += "<span class=\"PriceSale\"><img /> " + string.Format("{0:#,#}", ListProduct.ToPagedList(pageNumber, pageSize)[j].PriceSale) + "đ</span>";
                        chuoi += " </div>";
                    }
                    chuoi += "</div>";
                    chuoi += "</div>";
                
 
            }
            ViewBag.chuoihienthi = chuoi;
            ViewBag.nUrl = "<ol itemscope itemtype=\"http://schema.org/BreadcrumbList\">   <li itemprop=\"itemListElement\" itemscope  itemtype=\"http://schema.org/ListItem\"> <a itemprop=\"item\" href=\"http://Kangaroochinhhang.com\">  <span itemprop=\"name\">Trang chủ</span></a> <meta itemprop=\"position\" content=\"1\" />  </li>   ›" + UrlProduct(MenuParent.id) + "</ol> ";

            return View(ListProduct.ToPagedList(pageNumber, pageSize));
        }
                [OutputCache(CacheProfile = "CacheTag")]

        public ActionResult ListProductAgency(string tag)
        {
            var listManufacture = db.SelectListItem.First(p => p.Tag == tag);
            int idManu = int.Parse(listManufacture.id.ToString());
            var listIdCate = db.tblConnectManuProducts.Where(p => p.idManu == idManu).Select(p => p.idCate).ToList();
            var MenuParent = db.tblGroupProducts.Where(p => listIdCate.Contains(p.id) && p.Active == true).OrderBy(p => p.Ord).ToList();
            ViewBag.Title = "<title>" + listManufacture.Name + "</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"" + listManufacture.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + listManufacture.Name + "\" /> ";
            string chuoi = "";
            chuoi += " <div class=\"Box_Manufactures\">";
            chuoi += "<img src=\"" + listManufacture.Images + "\" alt=\"" + listManufacture.Name + "\" />";
             chuoi += "<h1>" + listManufacture.Name + "</h1>";
         
            chuoi += "</div>";
            for (int i = 0; i < MenuParent.Count; i++)
            {
                chuoi += "<div class=\"ListProduct\">";
                chuoi += "<div class=\"nVar2\">";
                chuoi += "<div class=\"Names\"><h2> <a href=\"/0/" + MenuParent[i].Tag + "\" title=\"" + MenuParent[i].Name + "\">" + MenuParent[i].Name + "</a></h2></div>";
                chuoi += "<hr />";
                chuoi += "</div>";
                chuoi += "<div class=\"Product_Tear\">";
                int idCate = int.Parse(MenuParent[i].id.ToString());
                var listProduct = db.tblProducts.Where(p => p.Active == true && p.idCate == idCate).OrderBy(p => p.Ord).ToList();
                for (int j = 0; j < listProduct.Count; j++)
                {
                    chuoi += "<div class=\"Tear_1\">";
                    chuoi += "<div class=\"img\">";
                    chuoi += "<div class=\"content_img\"><a href=\"/1/" + listProduct[j].Tag + "\" title=\"" + listProduct[j].Name + "\"><img src=\"" + listProduct[j].ImageLinkThumb + "\" alt=\"" + listProduct[j].Name + "\" /></a></div>";

                    chuoi += "<div class=\"detail\">";
                    chuoi += "<span class=\"title\">" + listProduct[j].Name + "</span>";
                    chuoi += "<p>" + listProduct[j].Info + "</p>";
                    chuoi += " <a href=\"/1/" + listProduct[j].Tag + "\" title=\"" + listProduct[j].Name + "\">Xem tiếp</a>";
                    chuoi += " </div>";
                    chuoi += "</div>";
                    chuoi += "<h3><a href=\"/1/" + listProduct[j].Tag + "\" class=\"Name\" title=\"" + listProduct[j].Name + "\">" + listProduct[j].Name + "</a></h3>";
                    chuoi += "<span class=\"Price\">Giá : " + string.Format("{0:#,#}", listProduct[j].Price) + "đ</span>";
                    chuoi += "<span class=\"PriceSale\"><img /> " + string.Format("{0:#,#}", listProduct[j].PriceSale) + "đ</span>";
                    chuoi += "</div>";
                }
                chuoi += "</div>";
                chuoi += "</div>";
            }

            ViewBag.chuoi = chuoi;
            ViewBag.nUrl = "<ol itemscope itemtype=\"http://schema.org/BreadcrumbList\">   <li itemprop=\"itemListElement\" itemscope  itemtype=\"http://schema.org/ListItem\"> <a itemprop=\"item\" href=\"http://Kangaroochinhhang.com\">  <span itemprop=\"name\">Trang chủ</span></a> <meta itemprop=\"position\" content=\"1\" />  </li>   ›" + UrlProduct(idManu) + "</ol> ";

            TblConfig TblConfig = db.TblConfigs.First();
            ViewBag.favicon = " <link href=\"" + TblConfig.Favicon + "\" rel=\"icon\" type=\"image/x-icon\" />";

            return View();
        }
                [OutputCache(CacheProfile = "CacheTag")]

        public ActionResult ListTabs(string tag)
        {
            var kiemtra = db.tblProductTags.Where(p => p.Tag == tag).Select(p => p.idp).ToList();
            var listProduct = db.tblProducts.Take(0).ToList();
            string name = "";

            if(kiemtra.Count>0)
            {
                listProduct = db.tblProducts.Where(p => kiemtra.Contains(p.id) && p.Active == true).OrderBy(p => p.Ord).ToList();
                name= db.tblProductTags.FirstOrDefault(p => p.Tag == tag).Name;

            }
             ViewBag.Name = name;
            ViewBag.Title = "<title>" + name + "</title>";
            ViewBag.dcTitle = "<meta name=\"DC.title\" content=\"" + name + "\" />";
            ViewBag.Description = "<meta name=\"description\" content=\"Danh sách sản phẩm " + name + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + name + "\" /> ";
            ViewBag.canonical = "<link rel=\"canonical\" href=\"http://Kangaroochinhhang.com/Tabs/" +tag + "\" />";
            string meta = "";
            meta += "<meta itemprop=\"name\" content=\"" + name + "\" />";
            meta += "<meta itemprop=\"url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta itemprop=\"description\" content=\"" + name + "\" />";
            meta += "<meta itemprop=\"image\" content=\"\" />";
            meta += "<meta property=\"og:title\" content=\"" + name + "\" />";
            meta += "<meta property=\"og:type\" content=\"product\" />";
            meta += "<meta property=\"og:url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta property=\"og:image\" content=\"\" />";
            meta += "<meta property=\"og:site_name\" content=\"http://Kangaroochinhhang.com\" />";
            meta += "<meta property=\"og:description\" content=\"" + name + "\" />";
            meta += "<meta property=\"fb:admins\" content=\"\" />";
            ViewBag.Meta = meta;  
            string chuoi = "";
            chuoi += " <div class=\"Box_Manufactures\">";
             chuoi += "<div class=\"bg_manu\">";
             chuoi += "<h1>" + name + "</h1>";
            chuoi += "<div class=\"line_Manu\"></div>";
            chuoi += "</div>";
            chuoi += "</div>";
           
                chuoi += "<div class=\"ListProduct\">";
                chuoi += "<div class=\"nVar2\">";
                chuoi += "<div class=\"Names\"><h2> Tag sản phẩm : " + name + "</h2></div>";
                chuoi += "<hr />";
                chuoi += "</div>";
                chuoi += "<div class=\"Product_Tear\">";
              
                //var listProduct = db.tblProducts.Where(p =>mangidPd.Contains(p.id) && p.Active == true ).OrderBy(p => p.Ord).ToList();
                for (int j = 0; j < listProduct.Count; j++)
                {
                    chuoi += "<div class=\"Tear_1\">";
                    chuoi += "<div class=\"img\">";
                    chuoi += "<div class=\"content_img\"><a href=\"/1/" + listProduct[j].Tag + "\" title=\"" + listProduct[j].Name + "\"><img src=\"" + listProduct[j].ImageLinkThumb + "\" alt=\"" + listProduct[j].Name + "\" /></a></div>";

                    chuoi += "<div class=\"detail\">";
                    chuoi += "<span class=\"title\">" + listProduct[j].Name + "</span>";
                    chuoi += "<p>" + listProduct[j].Info + "</p>";
                    chuoi += " <a href=\"/1/" + listProduct[j].Tag + "\" title=\"" + listProduct[j].Name + "\">Xem tiếp</a>";
                    chuoi += " </div>";
                    chuoi += "</div>";
                    chuoi += "<a href=\"/1/" + listProduct[j].Tag + "\" class=\"Name\" title=\"" + listProduct[j].Name + "\">" + listProduct[j].Name + "</a>";
                    chuoi += "<span class=\"Price\">Giá : " + string.Format("{0:#,#}", listProduct[j].Price) + "đ</span>";
                    chuoi += "<span class=\"PriceSale\"><img /> " + string.Format("{0:#,#}", listProduct[j].PriceSale) + "đ</span>";
                    chuoi += "</div>";
                }
                chuoi += "</div>";
                chuoi += "</div>";
            

            ViewBag.chuoi = chuoi;

            ViewBag.nUrl = "<ol itemscope itemtype=\"http://schema.org/BreadcrumbList\">   <li itemprop=\"itemListElement\" itemscope  itemtype=\"http://schema.org/ListItem\"> <a itemprop=\"item\" href=\"http://Kangaroochinhhang.com\">  <span itemprop=\"name\">Trang chủ</span></a> <meta itemprop=\"position\" content=\"1\" />  </li>   ›" + name + "</ol> ";
            return View();
        }
                [OutputCache(CacheProfile = "CacheTag")]

        public ActionResult ProductDetail(string tag)
        {
            tblProduct ProductDetail = db.tblProducts.First(p => p.Tag == tag);
            int idmenu = int.Parse(ProductDetail.idCate.ToString());
            tblGroupProduct groupProduct = db.tblGroupProducts.First(p => p.id == idmenu);
            ViewBag.Nhomsp = groupProduct.Name;
            ViewBag.favicon = " <link href=\"" + groupProduct.Favicon + "\" rel=\"icon\" type=\"image/x-icon\" />";
            var ListManu = db.tblConnectManuProducts.Where(p => p.idCate == idmenu).Take(1).ToList();
            if (ListManu.Count > 0)
            {
                int idManu = int.Parse(ListManu[0].idManu.ToString());
                ViewBag.Manu = "<li><span>Thương hiệu : " + db.SelectListItem.Find(idManu).Name + "</span></li>";
                ViewBag.Manufactures = db.SelectListItem.Find(idManu).Name;
            }     
            ViewBag.Title = "<title>" + ProductDetail.Title + "</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"" + ProductDetail.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + ProductDetail.Keyword + "\" /> ";
            ViewBag.imageog = "<meta property=\"og:image\" content=\"" + ProductDetail.ImageLinkDetail + "\"/>";
            ViewBag.titleog = "<meta property=\"og:title\" content=\"" + ProductDetail.Title + "\"/> ";
            ViewBag.site_nameog = "<meta property=\"og:site_name\" content=\"" + ProductDetail.Name + "\"/> ";
            ViewBag.urlog = "<meta property=\"og:url\" content=\"" + Request.Url.ToString() + "\"/> ";
            ViewBag.descriptionog = "<meta property=\"og:description\" content=\"" + ProductDetail.Description + "\" />";
            ViewBag.nUrl = "<ol itemscope itemtype=\"http://schema.org/BreadcrumbList\">   <li itemprop=\"itemListElement\" itemscope  itemtype=\"http://schema.org/ListItem\"> <a itemprop=\"item\" href=\"http://Kangaroochinhhang.com\">  <span itemprop=\"name\">Trang chủ</span></a> <meta itemprop=\"position\" content=\"1\" />  </li>   ›" + UrlProduct(groupProduct.id) + "</ol> ";
            ViewBag.canonical = "<link rel=\"canonical\" href=\"http://Kangaroochinhhang.com/1/" + ProductDetail.Tag + "\" />";
            StringBuilder schame = new StringBuilder();
            schame.Append("<script type=\"application/ld+json\">");
            schame.Append("{");
            schame.Append("\"@context\": \"http://schema.org\",");
            schame.Append("\"@type\": \"ProductArticle\",");
            schame.Append("\"headline\": \"" + ProductDetail.Description + "\",");
            schame.Append(" \"datePublished\": \"" + ProductDetail.DateCreate + "\",");
            schame.Append("\"image\": [");
            schame.Append(" \"" + ProductDetail.ImageLinkThumb + "\"");
            schame.Append(" ]");
            schame.Append("}");
            schame.Append("</script> ");
            ViewBag.schame = schame.ToString();
             

            int visit = int.Parse(ProductDetail.Visit.ToString());
            if (visit > 0)
            {
                ProductDetail.Visit = ProductDetail.Visit + 1;
                db.SaveChanges();

            }
            else
            {
                ProductDetail.Visit = ProductDetail.Visit + 1;
                db.SaveChanges();


            }
            string chuoilienquan = "";
            var listProductlq = db.tblProducts.Where(p => p.Active == true && p.idCate == idmenu && p.Tag != tag).OrderByDescending(p => p.id).Take(8).ToList();
            for(int i=0;i<listProductlq.Count;i++)
            {
                chuoilienquan += "<div class=\"Tear_1\">";
                chuoilienquan += "<div class=\"img\">";
                chuoilienquan += "<div class=\"content_img\"><a href=\"/1/" + listProductlq[i].Tag + "\" title=\"" + listProductlq[i].Name + "\"><img src=\"" + listProductlq[i].ImageLinkThumb + "\" alt=\"" + listProductlq[i].Name + "\" /></a></div>";

                chuoilienquan += "<div class=\"detail\">";
                chuoilienquan += "<span class=\"title\">" + listProductlq[i].Name + "</span>";
                chuoilienquan += "<p>" + listProductlq[i].Info + "</p>";
                chuoilienquan += "<a href=\"/1/" + listProductlq[i].Tag + "\" title=\"" + listProductlq[i].Name + "\">Xem tiếp</a>";
                chuoilienquan += "</div>";
                chuoilienquan += "</div>";
                chuoilienquan += "<h3><a href=\"/1/" + listProductlq[i].Tag + "\" class=\"Name\" title=\"" + listProductlq[i].Name + "\">" + listProductlq[i].Name + "</a></h3>";
                chuoilienquan += "<span class=\"Price\">Giá : " + string.Format("{0:#,#}", listProductlq[i].Price) + " đ</span>";
                chuoilienquan += "<span class=\"PriceSale\"><img /> " + string.Format("{0:#,#}", listProductlq[i].PriceSale) + " đ</span>";
                chuoilienquan += "</div>";
            }
            ViewBag.chuoilienquan = chuoilienquan;
            int id = ProductDetail.id;
            var listProductTags = db.tblProductTags.Where(p => p.idp == id).ToList();
            StringBuilder resultTag = new StringBuilder();
            for(int i=0;i<listProductTags.Count;i++)
            {
                resultTag.Append( "<h3> <a href=\"/Tabs/" + listProductTags[i].Tag + "\" title=\""+listProductTags[i].Name+"\">" + listProductTags[i].Name + "</a></h3>");

            }


            ViewBag.chuoitab = resultTag.ToString();
          
            return View(ProductDetail);
        }
        public PartialViewResult RightProductDetail(string tag)
        {

            tblProduct ProductDetail = db.tblProducts.First(p => p.Tag == tag);
           int id=ProductDetail.id;
            int idcate = int.Parse(ProductDetail.idCate.ToString());
            var listmenu = db.tblGroupProducts.Where(p => p.Active == true && p.id == idcate).OrderBy(p => p.Ord).ToList();
            string chuoimenu = "";
            for(int i=0;i<listmenu.Count;i++)
            {
                chuoimenu += "<a href=\"/0/" + listmenu[i].Tag + "\" title=\"" + listmenu[i].Name + "\">› " + listmenu[i].Name + "</a>";
            }
            ViewBag.chuoimenu = chuoimenu;
             
                string splq = "";
                int idPro = int.Parse(ProductDetail.id.ToString());
                var ListProductlq = db.tblProducts.Where(p => p.idCate==idcate && p.id!=id).OrderBy(p => p.Ord).Take(8).ToList();
                for (int i = 0; i < ListProductlq.Count; i++)
                {
                    splq += "<div class=\"Tear_1\">";
                    splq += "<div class=\"img\">";
                    splq += "<div class=\"content_img\"><a href=\"/1/" + ListProductlq[i].Tag + "\" title=\"" + ListProductlq[i].Name + "\"><img src=\"" + ListProductlq[i].ImageLinkThumb + "\" alt=\"" + ListProductlq[i].Name + "\" /></a></div>";

                    splq += " <div class=\"detail\">";
                    splq += "<span class=\"title\">" + ListProductlq[i].Name + "</span>";
                    splq += "<p>" + ListProductlq[i].Info + "</p>";
                    splq += "<a href=\"/1/" + ListProductlq[i].Tag + "\" title=\"/1/" + ListProductlq[i].Name + "\">Xem chi tiết</a>";
                    splq += "</div>";
                    splq += "</div>";
                    splq += "<h3><a href=\"/1/" + ListProductlq[i].Tag + "\" class=\"Name\" title=\"/1/" + ListProductlq[i].Name + "\">" + ListProductlq[i].Name + "</a></h3>";
                    splq += "<span class=\"Price\">Giá : " + string.Format("{0:#,#}", ListProductlq[i].PriceSale) + "đ</span>";
                    splq += "<span class=\"PriceSale\"><img /> " + string.Format("{0:#,#}", ListProductlq[i].Price) + "đ</span>";
                    splq += " </div>";
                }
                ViewBag.chuoisp = splq;
            //}
            string chuoisupport = "";
            var listSupport = db.tblSupports.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
            for (int i = 0; i < listSupport.Count; i++)
            {
                chuoisupport += "<div class=\"Line_Buttom\"></div>";
                chuoisupport += "<div class=\"Tear_Supports\">";
                chuoisupport += "<div class=\"Left_Tear_Support\">";
                chuoisupport += "<span class=\"htv1\">" + listSupport[i].Mission + ":</span>";
                chuoisupport += "<span class=\"htv2\">" + listSupport[i].Name + " :</span>";
                chuoisupport += "</div>";
                chuoisupport += "<div class=\"Right_Tear_Support\">";
                chuoisupport += "<a href=\"ymsgr:sendim?" + listSupport[i].Yahoo + "\">";
                chuoisupport += "<img src=\"http://opi.yahoo.com/online?u=" + listSupport[i].Yahoo + "&m=g&t=1\" alt=\"Yahoo\" class=\"imgYahoo\" />";
                chuoisupport += " </a>";
                chuoisupport += "<a href=\"Skype:" + listSupport[i].Skyper + "?chat\">";
                chuoisupport += "<img class=\"imgSkype\" src=\"/Content/Display/iCon/skype-icon.png\" title=\"Kangaroo\" alt=\"" + listSupport[i].Name + "\">";
                chuoisupport += "</a>";
                chuoisupport += "</div>";
                chuoisupport += "</div>";
            }
            ViewBag.chuoisupport = chuoisupport;
            return PartialView(db.TblConfigs.First());
        }
        public PartialViewResult PartialProductHomes()
        {
            var listproduct = db.tblProducts.Where(p => p.Active == true && p.idCate == 1 && p.ViewHomes==true).OrderBy(p => p.Ord).ToList();
            int count = listproduct.Count;
            if(Request.Browser.IsMobileDevice)
            { 
            if(count%2!=0)
            {
                  listproduct = db.tblProducts.Where(p => p.Active == true && p.idCate == 1 && p.ViewHomes == true).OrderBy(p => p.Ord).Take(count-1).ToList();
            }
            }
            string chuoi = "";
            for (int i = 0; i < listproduct.Count;i++)
            {
                chuoi += "<div class=\"Tear_Func\">";
                chuoi += "<div class=\"Tear_Func1\">";
                chuoi += " <a href=\"\" class=\"Name\">" + listproduct[i].Name + "</a>";
                chuoi += "<h2><a href=\"/1/" + listproduct[i].Tag + "\" title=\"" + listproduct[i].Name + "\"> " + listproduct[i].Code + " </a></h2>";
                chuoi += "<div class=\"Conten_Tear_Funct\">";
                chuoi += "<a href=\"/1/" + listproduct[i].Tag + "\" title=\"" + listproduct[i].Name + "\">   <img src=\"" + listproduct[i].ImageLinkThumb + "\" alt=\"" + listproduct[i].Name + "\" /></a>";
                chuoi += "</div>";
                chuoi += "<span class=\"fuc\">" + listproduct[i].Info + "</span>";
                chuoi += "</div>";
                chuoi += "</div>";
            }
            ViewBag.chuoi = chuoi;
                return PartialView();
        }
        public ActionResult Command(FormCollection collection, string tag)
        {
            if (collection["btnOrder"] != null)
            {

                Session["idProduct"] = collection["idPro"];
                Session["idMenu"] = collection["idCate"];
                Session["OrdProduct"] = collection["txtOrd"];
                Session["Url"] = Request.Url.ToString();
                return RedirectToAction("OrderIndex", "Order");


            }
            return View();
        }
	}
}
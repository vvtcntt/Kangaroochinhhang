using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Kangaroochinhhang.Models;

namespace Kangaroochinhhang.Controllers.DisplayCustom
{
    public class productCustomController : Controller
    {
        // GET: productCustom
        public ActionResult Index()
        {
            return View();
        }

        private KangaroochinhhangContext db = new KangaroochinhhangContext();

        private string nUrl = "";

        public string UrlProduct(int idCate)
        {
            var ListMenu = db.tblGroupProducts.Where(p => p.id == idCate).ToList();
            for (int i = 0; i < ListMenu.Count; i++)
            {
                nUrl = " <li itemprop=\"itemListElement\" itemscope itemtype=\"http://schema.org/ListItem\"> <a itemprop=\"item\" href=\"/0/" + ListMenu[i].Tag + "\"> <span itemprop=\"name\">" + ListMenu[i].Name + "</span></a> <meta itemprop=\"position\" content=\"\" /> </li>  " + nUrl;
                string ids = ListMenu[i].ParentID.ToString();
                if (ids != null && ids != "")
                {
                    int id = int.Parse(ListMenu[i].ParentID.ToString());
                    UrlProduct(id);
                }
            }
            return nUrl;
        }

        private List<string> Mangphantu = new List<string>();

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

        public ActionResult productDetail(string tag)
        {
            tblProduct ProductDetail = db.tblProducts.First(p => p.Tag == tag);
            int id = ProductDetail.id;
            int idmenu = int.Parse(ProductDetail.idCate.ToString());
            tblGroupProduct groupProduct = db.tblGroupProducts.First(p => p.id == idmenu);
            ViewBag.Nhomsp = groupProduct.Name;
            var listId = db.tblConnectManuProducts.Where(p => p.idCate == idmenu).Select(p => p.idManu).Take(1).ToList();
            Session["idManu"] = "";
            int idManu = int.Parse(listId[0].Value.ToString());
            var manufactures = db.SelectListItem.Find(idManu);
            ViewBag.manuName = manufactures.Name;
            ViewBag.color = manufactures.Color;
            ViewBag.color1 = manufactures.Color1;
            ViewBag.color3 = manufactures.Color2;
            ViewBag.imageGenuine = manufactures.imageGenuine;
            Session["idManu"] = idManu;
            ViewBag.Manu = "<li><span>Thương hiệu : " + manufactures.Name + "</span></li>";
            ViewBag.Manufactures = db.SelectListItem.Find(idManu).Name;
            ViewBag.favicon = " <link href=\"" + manufactures.Favicon + "\" rel=\"icon\" type=\"image/x-icon\" />";

            ViewBag.Title = "<title>" + ProductDetail.Title + "</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"" + ProductDetail.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + ProductDetail.Keyword + "\" /> ";
            ViewBag.imageog = "<meta property=\"og:image\" content=\"http://Kangaroochinhhang.com" + ProductDetail.ImageLinkThumb + "\"/>";
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
            var ListGroupCri = db.tblGroupCriterias.Where(p => p.idCate == idmenu).Select(p => p.idCri).ToList();

            var ListCri = db.tblCriterias.Where(p => ListGroupCri.Contains(p.id) && p.Active == true).OrderBy(p => p.Ord).ToList();
            StringBuilder chuoi = new StringBuilder();
            #region[Lọc thuộc tính]

            for (int i = 0; i < ListCri.Count; i++)
            {
                int idCre = int.Parse(ListCri[i].id.ToString());
                var ListCr = (from a in db.tblConnectCriterias
                              join b in db.tblInfoCriterias on a.idCre equals b.id
                              where a.idpd == id && b.idCri == idCre && b.Active == true
                              select new
                              {
                                  b.Name,
                                  b.Url,
                                  b.Ord
                              }).OrderBy(p => p.Ord).ToList();
                if (ListCr.Count > 0)
                {
                    chuoi.Append("<tr>");
                    chuoi.Append("<td>" + ListCri[i].Name + "</td>");
                    chuoi.Append("<td>");
                    int dem = 0;
                    string num = "";
                    if (ListCr.Count > 1)
                        num = "⊹ ";
                    foreach (var item in ListCr)
                        if (item.Url != null && item.Url != "")
                        {
                            chuoi.Append("<a href=\"" + item.Url + "\" title=\"" + item.Name + "\">");
                            if (dem == 1)
                                chuoi.Append(num + item.Name);
                            else
                                chuoi.Append(num + item.Name);
                            dem = 1;
                            chuoi.Append("</a>");
                        }
                        else
                        {
                            if (dem == 1)
                                chuoi.Append(num + item.Name + "</br> ");
                            else
                                chuoi.Append(num + item.Name + "</br> ");
                            dem = 1;
                        }
                    chuoi.Append("</td>");
                    chuoi.Append(" </tr>");
                }
            }
            ViewBag.chuoi = chuoi.ToString();

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
            var listImages = db.tblImageProducts.Where(p => p.idProduct == id).ToList();
            StringBuilder chuoiimages = new StringBuilder();
            if(listImages.Count>0)
            {
                chuoiimages.Append("<li class=\"getImg" + ProductDetail.id + "\"><a href=\"javascript:;\" onclick=\"javascript:return getImage('" + ProductDetail.ImageLinkDetail + "', 'getImg" + ProductDetail.id + "')\" title=\"" + ProductDetail.Name + "\"><img src=\"" + ProductDetail.ImageLinkDetail + "\" alt=\"" + ProductDetail.Name + "\" /></a></li>");

                for (int i = 0; i < listImages.Count; i++)
                {
                    chuoiimages.Append("<li class=\"getImg" + listImages[i].id + "\"><a href=\"javascript:;\" onclick=\"javascript:return getImage('" + listImages[i].Images + "', 'getImg" + listImages[i].id + "')\" title=\"" + ProductDetail.Name + "\"><img src=\"" + listImages[i].Images + "\" alt=\"" + ProductDetail.Name + "\" /></a></li>");
                }
            }
            
            ViewBag.chuoiimages = chuoiimages;
            ViewBag.hotline = db.TblConfigs.First().HotlineIN;
            //load root
            StringBuilder chuoitag = new StringBuilder();
            if (ProductDetail.Tabs != null)
            {
                string Chuoi = ProductDetail.Tabs;
                string[] Mang = Chuoi.Split(',');
                List<int> araylist = new List<int>();
                for (int i = 0; i < Mang.Length; i++)
                {
                    string tagsp = StringClass.NameToTag(Mang[i]);
                    chuoitag.Append("<a href=\"/tabs/" + tagsp + "\" title=\"" + Mang[i] + "\">" + Mang[i] + "</a>");
                }
            }
            ViewBag.chuoitag = chuoitag;
            StringBuilder result = new StringBuilder();
            result.Append("<div class=\"tearListProduct\">");

            result.Append("<div class=\"contentListProductContent\">");
            var listProduct = db.tblProducts.Where(p => p.Active == true && p.idCate == idmenu && p.Tag != tag).Take(6).OrderBy(p => p.Ord).ToList();
            for (int j = 0; j < listProduct.Count; j++)
            {
                result.Append("<div class=\"tear\">");
                result.Append("<div class=\"contentTear\">");
                float price = float.Parse(listProduct[j].Price.ToString());
                float pricesale = float.Parse(listProduct[j].PriceSale.ToString());
                float phantram = 100 - ((pricesale * 100) / price);
                result.Append(" <div class=\"sale\">" + Convert.ToInt32(phantram) + "%</div>");
                result.Append("<div class=\"img\">");
                result.Append("<a href=\"/1/" + listProduct[j].Tag + "\" title=\"" + listProduct[j].Name + "\"><img src=\"" + listProduct[j].ImageLinkThumb + "\" alt=\"" + listProduct[j].Name + "\" /></a>");
                result.Append(" </div>");
                result.Append("<a href=\"/1/" + listProduct[j].Tag + "\" title=\"" + listProduct[j].Name + "\" class=\"name\">" + listProduct[j].Name + "</a>");
                result.Append("<div class=\"boxItem\">");
                result.Append("<div class=\"boxPrice\">");
                result.Append("<span class=\"priceSale\">" + string.Format("{0:#,#}", listProduct[j].PriceSale) + "đ</span>");
                result.Append("<span class=\"price\">" + string.Format("{0:#,#}", listProduct[j].Price) + "đ</span>");
                result.Append("</div>");
                result.Append("<div class=\"boxSale\">");
                result.Append("<a href=\"\" title=\"\"></a>");
                result.Append("</div>");
                result.Append("</div>");
                result.Append(" </div>");
                result.Append("</div>");
            }
            result.Append("</div>");
            result.Append("</div>");
            ViewBag.resultLienquan = result.ToString();
            #endregion

            return View(ProductDetail);
        }

        public ActionResult productList(string tag)
        {
            tblGroupProduct groupProduct = db.tblGroupProducts.First(p => p.Tag == tag);
            ViewBag.name = groupProduct.Name;
            ViewBag.headshord = groupProduct.Content;
            int idCate = groupProduct.id;
            ViewBag.check = true;

            var listId = db.tblConnectManuProducts.Where(p => p.idCate == idCate).Select(p => p.idManu).Take(1).ToList();
            if (listId.Count < 1)
            {
                ViewBag.check = false;
                ViewBag.layout = "~/Views/Shared/_LayoutDefault.cshtml";
                string chuoi = "";
                chuoi += "<div id=\"HeadShort\">";
                chuoi += "<div id=\"Left_HeadShort\">";
                chuoi += "<img src=\"" + groupProduct.Background + "\" alt=\"" + groupProduct.Name + "\" title=\"" + groupProduct.Name + "\" />";
                chuoi += "</div>";
                chuoi += "<div id=\"Right_HeadShort\">";
                chuoi += "<div id=\"nVar_HeadShort\">";
                chuoi += "<h1>" + groupProduct.Name + "</h1>";
                chuoi += "</div>";
                chuoi += "<div class=\"line3\"></div>";
                chuoi += "<div id=\"Content_HeadShort\">" + groupProduct.Content + "</div>";
                chuoi += "</div>";
                chuoi += "</div>";
                int idcate = groupProduct.id;
                //Manufacture(idcate);
                var listMenu = db.tblGroupProducts.Where(p => p.ParentID == idcate && p.Active == true).OrderBy(p => p.Ord).ToList();
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
                        var ListProduct = db.tblProducts.Where(p => p.idCate == icate && p.Active == true).OrderBy(p => p.Ord).ToList();
                        for (int j = 0; j < ListProduct.Count; j++)
                        {
                            chuoi += "<div class=\"Tear_1\">";
                            chuoi += "<div class=\"img\">";
                            chuoi += "<div class=\"content_img\"><a href=\"/1/" + ListProduct[j].Tag + "\" title=\"" + ListProduct[j].Name + "\"><img src=\"" + ListProduct[j].ImageLinkThumb + "\" alt=\"" + ListProduct[j].Name + "\" /></a></div>";
                            chuoi += "<div class=\"detail\">";
                            chuoi += "<span class=\"title\">" + ListProduct[j].Name + "</span>";
                            chuoi += "<p>" + ListProduct[j].Info + "</p>";
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
                ViewBag.chuoihienthi = chuoi;
            }
            else
            {
                ViewBag.layout = "~/Views/Shared/_LayoutDefaultCustom.cshtml";
                Session["idManu"] = "";
                int idManu = int.Parse(listId[0].Value.ToString());
                var manufactures = db.SelectListItem.Find(idManu);
                ViewBag.manuName = manufactures.Name;
                ViewBag.color = manufactures.Color;
                Session["idManu"] = idManu;
                ViewBag.favicon = " <link href=\"" + manufactures.Favicon + "\" rel=\"icon\" type=\"image/x-icon\" />";
                ViewBag.Title = "<title>" + groupProduct.Title + "</title>";
                ViewBag.Description = "<meta name=\"description\" content=\"" + groupProduct.Description + "\"/>";
                ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + groupProduct.Keyword + "\" /> ";
                ViewBag.imageog = "<meta property=\"og:image\" content=\"" + groupProduct.Images + "\"/>";
                ViewBag.titleog = "<meta property=\"og:title\" content=\"" + groupProduct.Title + "\"/> ";
                ViewBag.site_nameog = "<meta property=\"og:site_name\" content=\"" + groupProduct.Name + "\"/> ";
                ViewBag.urlog = "<meta property=\"og:url\" content=\"" + Request.Url.ToString() + "\"/> ";
                ViewBag.descriptionog = "<meta property=\"og:description\" content=\"" + groupProduct.Description + "\" />";
                ViewBag.nUrl = "<ol itemscope itemtype=\"http://schema.org/BreadcrumbList\">   <li itemprop=\"itemListElement\" itemscope  itemtype=\"http://schema.org/ListItem\"> <a itemprop=\"item\" href=\"http://Kangaroochinhhang.com\">  <span itemprop=\"name\">Trang chủ</span></a> <meta itemprop=\"position\" content=\"1\" />  </li>   ›" + UrlProduct(groupProduct.id) + "</ol> ";
                ViewBag.canonical = "<link rel=\"canonical\" href=\"http://Kangaroochinhhang.com/0/" + groupProduct.Tag + "\" />";
                StringBuilder result = new StringBuilder();

                var listGroupProduct = db.tblGroupProducts.Where(p => p.ParentID == idCate && p.Active == true).OrderBy(p => p.Ord).ToList();
                if (listGroupProduct.Count > 0)
                {
                    for (int i = 0; i < listGroupProduct.Count; i++)
                    {
                        result.Append("<div class=\"tearListProduct\">");
                        result.Append("<div class=\"saleProduct\">");
                        int idmanu = listGroupProduct[i].id;
                        var listIdImages = db.tblConnectImages.Where(p => p.idCate == idmanu).Select(p => p.idImg).ToList();
                        var listImage = db.tblImages.Where(p => p.Active == true && p.idCate == 9 && listIdImages.Contains(p.id)).OrderBy(p => p.Ord).ToList();
                        for (int j = 0; j < listImage.Count; j++)
                        {
                            result.Append("<a href=\"" + listImage[j].Url + "\" title=\"" + listImage[j].Name + "\"><img src=\"" + listImage[j].Images + "\" alt=\"" + listImage[j].Name + "\" /></a>");
                        }
                        result.Append("</div>");
                        result.Append("<div class=\"filter\">");
                        result.Append("<span class=\"name\">Lọc sản phẩm " + listGroupProduct[i].Name + " : </span>");
                        result.Append("</div>");
                        result.Append("<div class=\"contentListProductContent\">");
                        var listProduct = db.tblProducts.Where(p => p.Active == true && p.idCate == idmanu).OrderBy(p => p.Ord).Take(12).ToList();
                        for (int j = 0; j < listProduct.Count; j++)
                        {
                            result.Append("<div class=\"tear\">");
                            result.Append("<div class=\"contentTear\">");
                            float price = float.Parse(listProduct[j].Price.ToString());
                            float pricesale = float.Parse(listProduct[j].PriceSale.ToString());
                            float phantram = 100 - ((pricesale * 100) / price);
                            result.Append(" <div class=\"sale\">" + Convert.ToInt32(phantram) + "%</div>");
                            result.Append("<div class=\"img\">");
                            result.Append("<a href=\"/1/" + listProduct[j].Tag + "\" title=\"" + listProduct[j].Name + "\"><img src=\"" + listProduct[j].ImageLinkThumb + "\" alt=\"" + listProduct[j].Name + "\" /></a>");
                            result.Append(" </div>");
                            result.Append("<a href=\"/1/" + listProduct[j].Tag + "\" title=\"" + listProduct[j].Name + "\" class=\"name\">" + listProduct[j].Name + "</a>");
                            result.Append("<div class=\"boxItem\">");
                            result.Append("<div class=\"boxPrice\">");
                            result.Append("<span class=\"priceSale\">" + string.Format("{0:#,#}", listProduct[j].PriceSale) + "đ</span>");
                            result.Append("<span class=\"price\">" + string.Format("{0:#,#}", listProduct[j].Price) + "đ</span>");
                            result.Append("</div>");
                            result.Append("<div class=\"boxSale\">");
                            result.Append("<a href=\"\" title=\"\"></a>");
                            result.Append("</div>");
                            result.Append("</div>");
                            result.Append(" </div>");
                            result.Append("</div>");
                        }
                        result.Append("</div>");
                        result.Append("</div>");
                    }
                }
                else
                {
                    result.Append("<div class=\"tearListProduct\">");
                    result.Append("<div class=\"saleProduct\">");
                    int idmanu = groupProduct.id;
                    var listIdImages = db.tblConnectImages.Where(p => p.idCate == idmanu).Select(p => p.idImg).ToList();
                    var listImage = db.tblImages.Where(p => p.Active == true && p.idCate == 8 && listIdImages.Contains(p.id)).OrderBy(p => p.Ord).ToList();
                    for (int j = 0; j < listImage.Count; j++)
                    {
                        result.Append("<a href=\"" + listImage[j].Url + "\" title=\"" + listImage[j].Name + "\"><img src=\"" + listImage[j].Images + "\" alt=\"" + listImage[j].Name + "\" /></a>");
                    }
                    result.Append("</div>");
                    result.Append("<div class=\"filter\">");
                    result.Append("<span class=\"name\">Lọc sản phẩm " + groupProduct.Name + " : </span>");
                    result.Append("</div>");
                    result.Append("<div class=\"contentListProductContent\">");
                    var listProduct = db.tblProducts.Where(p => p.Active == true && p.idCate == idmanu).OrderBy(p => p.Ord).ToList();
                    for (int j = 0; j < listProduct.Count; j++)
                    {
                        result.Append("<div class=\"tear\">");
                        result.Append("<div class=\"contentTear\">");
                        float price = float.Parse(listProduct[j].Price.ToString());
                        float pricesale = float.Parse(listProduct[j].PriceSale.ToString());
                        float phantram = 100 - ((pricesale * 100) / price);
                        result.Append(" <div class=\"sale\">" + Convert.ToInt32(phantram) + "%</div>");
                        result.Append("<div class=\"img\">");
                        result.Append("<a href=\"/1/" + listProduct[j].Tag + "\" title=\"" + listProduct[j].Name + "\"><img src=\"" + listProduct[j].ImageLinkThumb + "\" alt=\"" + listProduct[j].Name + "\" /></a>");
                        result.Append(" </div>");
                        result.Append("<a href=\"/1/" + listProduct[j].Tag + "\" title=\"" + listProduct[j].Name + "\" class=\"name\">" + listProduct[j].Name + "</a>");
                        result.Append("<div class=\"boxItem\">");
                        result.Append("<div class=\"boxPrice\">");
                        result.Append("<span class=\"priceSale\">" + string.Format("{0:#,#}", listProduct[j].PriceSale) + "đ</span>");
                        result.Append("<span class=\"price\">" + string.Format("{0:#,#}", listProduct[j].Price) + "đ</span>");
                        result.Append("</div>");
                        result.Append("<div class=\"boxSale\">");
                        result.Append("<a href=\"\" title=\"\"></a>");
                        result.Append("</div>");
                        result.Append("</div>");
                        result.Append(" </div>");
                        result.Append("</div>");
                    }
                    result.Append("</div>");
                    result.Append("</div>");
                }

                ViewBag.result = result.ToString();
            }

            return View();
        }

        public ActionResult productTag(string tag)
        {
            StringBuilder result = new StringBuilder();

            var listIDProduct = db.tblProductTags.Where(p => p.Tag == tag || p.Name.Contains(tag)).Select(p => p.idp).ToList();
            var listTagProduct = db.tblProductTags.Where(p => p.Tag == tag).Take(1).ToList();
            var listProduct = db.tblProducts.Where(p => p.Active == true && (listIDProduct.Contains(p.id) || p.Code==tag)).OrderBy(p => p.Ord).ToList();
            string name = tag;
            string title = tag;
            string description = tag;
            ViewBag.layout = "~/Views/Shared/_LayoutDefault.cshtml";
            if (listProduct.Count > 0)
            {
                int idcate = int.Parse(listProduct[0].idCate.ToString());
                var listId = db.tblConnectManuProducts.Where(p => p.idCate == idcate).Take(1).Select(p => p.idManu).Single();
                int idManu = listId.Value;
                var manufactures = db.SelectListItem.Find(idManu);
                ViewBag.manuName = manufactures.Name;
                ViewBag.color = manufactures.Color;
                if(listTagProduct.Count>0 )
                {
                    name = listTagProduct[0].Name;
                    title = listTagProduct[0].Name;
                    description = listTagProduct[0].Name;
                }
                else
                {
                    name = tag;
                    title = tag;
                    description = tag;
                }
               
                Session["idManu"] = idManu;
                ViewBag.layout = "~/Views/Shared/_LayoutDefaultCustom.cshtml";
                var tbltags = db.tblTags.Where(p => p.Tag == tag && p.Active == true).Take(1).ToList();
                if (tbltags.Count > 0)
                {
                    name = tbltags[0].Name;
                    title = tbltags[0].Title;
                    description = tbltags[0].Description;
                }

                ViewBag.favicon = " <link href=\"" + manufactures.Favicon + "\" rel=\"icon\" type=\"image/x-icon\" />";
                ViewBag.Title = "<title>" + title + "</title>";
                ViewBag.dcTitle = "<meta name=\"DC.title\" content=\"" + title + "\" />";
                ViewBag.Description = "<meta name=\"description\" content=\"Danh sách sản phẩm " + description + "\"/>";
                ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + name + "\" /> ";
                string meta = "";
                meta += "<meta itemprop=\"name\" content=\"" + name + "\" />";
                meta += "<meta itemprop=\"url\" content=\"" + Request.Url.ToString() + "\" />";
                meta += "<meta itemprop=\"description\" content=\"" + name + "\" />";
                meta += "<meta itemprop=\"image\" content=\"\" />";
                meta += "<meta property=\"og:title\" content=\"" + title + "\" />";
                meta += "<meta property=\"og:type\" content=\"product\" />";
                meta += "<meta property=\"og:url\" content=\"" + Request.Url.ToString() + "\" />";
                meta += "<meta property=\"og:image\" content=\"\" />";
                meta += "<meta property=\"og:site_name\" content=\"http://Kangaroochinhhang.com\" />";
                meta += "<meta property=\"og:description\" content=\"" + name + "\" />";
                meta += "<meta property=\"fb:admins\" content=\"\" />";
                ViewBag.Meta = meta;
                result.Append("<div class=\"tearListProduct\">");
                result.Append("<div class=\"filter\">");
                result.Append("<h1 class=\"name\">   " + name + "   </h1>");
                result.Append("</div>");
                result.Append("<div class=\"contentListProductContent\">");
                for (int j = 0; j < listProduct.Count; j++)
                {
                    result.Append("<div class=\"tear\">");
                    result.Append("<div class=\"contentTear\">");
                    float price = float.Parse(listProduct[j].Price.ToString());
                    float pricesale = float.Parse(listProduct[j].PriceSale.ToString());
                    float phantram = 100 - ((pricesale * 100) / price);
                    result.Append(" <div class=\"sale\">" + Convert.ToInt32(phantram) + "%</div>");
                    result.Append("<div class=\"img\">");
                    result.Append("<a href=\"/1/" + listProduct[j].Tag + "\" title=\"" + listProduct[j].Name + "\"><img src=\"" + listProduct[j].ImageLinkThumb + "\" alt=\"" + listProduct[j].Name + "\" /></a>");
                    result.Append(" </div>");
                    result.Append("<a href=\"/1/" + listProduct[j].Tag + "\" title=\"" + listProduct[j].Name + "\" class=\"name\">" + listProduct[j].Name + "</a>");
                    result.Append("<div class=\"boxItem\">");
                    result.Append("<div class=\"boxPrice\">");
                    result.Append("<span class=\"priceSale\">" + string.Format("{0:#,#}", listProduct[j].PriceSale) + "đ</span>");
                    result.Append("<span class=\"price\">" + string.Format("{0:#,#}", listProduct[j].Price) + "đ</span>");
                    result.Append("</div>");
                    result.Append("<div class=\"boxSale\">");
                    result.Append("<a href=\"\" title=\"\"></a>");
                    result.Append("</div>");
                    result.Append("</div>");
                    result.Append(" </div>");
                    result.Append("</div>");
                }
                result.Append("</div>");
                result.Append("</div>");
                ViewBag.result = result.ToString();
                ViewBag.nUrl = "<ol itemscope itemtype=\"http://schema.org/BreadcrumbList\">   <li itemprop=\"itemListElement\" itemscope  itemtype=\"http://schema.org/ListItem\"> <a itemprop=\"item\" href=\"http://Kangaroochinhhang.com\">  <span itemprop=\"name\">Trang chủ</span></a> <meta itemprop=\"position\" content=\"1\" />  </li>   ›" +name + "</ol> ";
                return View(listProduct);
            }
            else
            {
                var tbltags = db.tblTags.Where(p => p.Tag == tag && p.Active == true).Take(1).ToList();
                if (tbltags.Count > 0)
                {
                    name = tbltags[0].Name;
                    title = tbltags[0].Title;
                    description = tbltags[0].Description;
                }
                ViewBag.Title = "<title>" + title + "</title>";
                ViewBag.dcTitle = "<meta name=\"DC.title\" content=\"" + title + "\" />";
                ViewBag.Description = "<meta name=\"description\" content=\"Danh sách sản phẩm " + description + "\"/>";
                ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + tag + "\" /> ";
                ViewBag.canonical = "<link rel=\"canonical\" href=\"http://Kangaroochinhhang.com/Tabs/" + tag + "\" />";
                result.Append("<div class=\"tearListProduct\">");
                result.Append("<div class=\"filter\">");
                result.Append("<h1 class=\"name\">   " + tag + "   </h1>");
                result.Append("</div>");
                result.Append("<div class=\"contentListProductContent\">");
                result.Append("</div>");
                result.Append("</div>");
                ViewBag.result = result.ToString();
            }
            return View(listProduct);
        }

        public ActionResult search(string tag)
        {
            if (Session["Search"] != null)
                tag = Session["Search"].ToString();
            var listProduct = db.tblProducts.Where(p => p.Name.Contains(tag) && p.Active == true).ToList();
            ViewBag.Name = tag;

            if (Session["idManu"] != null && Session["idManu"] != "")
            {
                ViewBag.layout = "~/Views/Shared/_LayoutDefaultCustom.cshtml";
                int idmanu = int.Parse(Session["idManu"].ToString());
                var manufactures = db.SelectListItem.Find(idmanu);
                ViewBag.manuName = manufactures.Name;
                ViewBag.color = manufactures.Color;
                var listId = db.tblConnectManuProducts.Where(p => p.idManu == idmanu).Select(p => p.idCate).ToList();
                listProduct = db.tblProducts.Where(p => p.Active == true && listId.Contains(p.idCate.Value) && p.Name.Contains(tag)).OrderBy(p => p.Ord).ToList();
                StringBuilder result = new StringBuilder();

                ViewBag.favicon = " <link href=\"" + manufactures.Favicon + "\" rel=\"icon\" type=\"image/x-icon\" />";
                ViewBag.Title = "<title>" + tag + "</title>";
                ViewBag.dcTitle = "<meta name=\"DC.title\" content=\"" + tag + "\" />";
                ViewBag.Description = "<meta name=\"description\" content=\"Danh sách sản phẩm " + tag + "\"/>";
                ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + tag + "\" /> ";
                ViewBag.canonical = "<link rel=\"canonical\" href=\"http://Kangaroochinhhang.com/search/" + tag + "\" />";
                string meta = "";
                meta += "<meta itemprop=\"name\" content=\"" + tag + "\" />";
                meta += "<meta itemprop=\"url\" content=\"" + Request.Url.ToString() + "\" />";
                meta += "<meta itemprop=\"description\" content=\"" + tag + "\" />";
                meta += "<meta itemprop=\"image\" content=\"\" />";
                meta += "<meta property=\"og:title\" content=\"" + tag + "\" />";
                meta += "<meta property=\"og:type\" content=\"product\" />";
                meta += "<meta property=\"og:url\" content=\"" + Request.Url.ToString() + "\" />";
                meta += "<meta property=\"og:image\" content=\"\" />";
                meta += "<meta property=\"og:site_name\" content=\"http://Kangaroochinhhang.com\" />";
                meta += "<meta property=\"og:description\" content=\"" + tag + "\" />";
                meta += "<meta property=\"fb:admins\" content=\"\" />";
                ViewBag.Meta = meta;
                result.Append("<div class=\"tearListProduct\">");
                result.Append("<div class=\"filter\">");
                result.Append("<h1 class=\"name\">  Từ khóa bạn tìm kiếm :  " + tag + "   </h1>");
                result.Append("</div>");
                result.Append("<div class=\"contentListProductContent\">");
                for (int j = 0; j < listProduct.Count; j++)
                {
                    result.Append("<div class=\"tear\">");
                    result.Append("<div class=\"contentTear\">");
                    float price = float.Parse(listProduct[j].Price.ToString());
                    float pricesale = float.Parse(listProduct[j].PriceSale.ToString());
                    float phantram = 100 - ((pricesale * 100) / price);
                    result.Append(" <div class=\"sale\">" + Convert.ToInt32(phantram) + "%</div>");
                    result.Append("<div class=\"img\">");
                    result.Append("<a href=\"/1/" + listProduct[j].Tag + "\" title=\"" + listProduct[j].Name + "\"><img src=\"" + listProduct[j].ImageLinkThumb + "\" alt=\"" + listProduct[j].Name + "\" /></a>");
                    result.Append(" </div>");
                    result.Append("<a href=\"/1/" + listProduct[j].Tag + "\" title=\"" + listProduct[j].Name + "\" class=\"name\">" + listProduct[j].Name + "</a>");
                    result.Append("<div class=\"boxItem\">");
                    result.Append("<div class=\"boxPrice\">");
                    result.Append("<span class=\"priceSale\">" + string.Format("{0:#,#}", listProduct[j].PriceSale) + "đ</span>");
                    result.Append("<span class=\"price\">" + string.Format("{0:#,#}", listProduct[j].Price) + "đ</span>");
                    result.Append("</div>");
                    result.Append("<div class=\"boxSale\">");
                    result.Append("<a href=\"\" title=\"\"></a>");
                    result.Append("</div>");
                    result.Append("</div>");
                    result.Append(" </div>");
                    result.Append("</div>");
                }
                result.Append("</div>");
                result.Append("</div>");
                ViewBag.result = result.ToString();
                ViewBag.nUrl = "<ol itemscope itemtype=\"http://schema.org/BreadcrumbList\">   <li itemprop=\"itemListElement\" itemscope  itemtype=\"http://schema.org/ListItem\"> <a itemprop=\"item\" href=\"http://Kangaroochinhhang.com\">  <span itemprop=\"name\">Trang chủ</span></a> <meta itemprop=\"position\" content=\"1\" />  </li>   ›Bạn đang tìm kiếm : " + tag + "</ol> ";
            }
            return View(listProduct);
        }
    }
}
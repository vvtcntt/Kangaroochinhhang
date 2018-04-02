using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Kangaroochinhhang.Models;

namespace Kangaroochinhhang.Controllers.Display.Section.Baogia
{
    public class BaogiaController : Controller
    {
        //
        // GET: /Baogia/
        private KangaroochinhhangContext db = new KangaroochinhhangContext();

        public PartialViewResult Homes_Baogia()
        {
            string chuoi = "";
            var listbaogia = db.tblGroupProducts.Where(p => p.Active == true && p.Baogia == true).OrderBy(p => p.Ord).ToList();
            for (int i = 0; i < listbaogia.Count; i++)
            {
                int idcate = int.Parse(listbaogia[i].id.ToString());
                var kiemtra = db.tblConnectManuProducts.FirstOrDefault(p => p.idCate == idcate);
                if (kiemtra != null)
                {
                    int idManu = int.Parse(db.tblConnectManuProducts.FirstOrDefault(p => p.idCate == idcate).idManu.ToString());
                    var listManufacture = db.SelectListItem.First(p => p.id == idManu);

                    chuoi += "<div class=\"Tear_Baogias\">";
                    chuoi += "<a href=\"/Bao-gia/Bao-gia-" + listbaogia[i].Tag + "\" title=\"Báo giá " + listbaogia[i].Name + "\"><img src=\"" + listManufacture.Images + "\" alt=\"Báo giá " + listbaogia[i].Name + "\" /></a>";
                    chuoi += "<a href=\"/Bao-gia/Bao-gia-" + listbaogia[i].Tag + "\" title=\"Báo giá " + listbaogia[i].Name + "\" class=\"Name\">Báo giá " + listbaogia[i].Name + "</a>";
                    chuoi += "</div>";
                }
            }
            ViewBag.chuoi = chuoi;
            return PartialView();
        }

        public ActionResult Index(string tag)
        {
            TblConfig tblcongif = db.TblConfigs.First();

            tblGroupProduct groupproduct = db.tblGroupProducts.First(p => p.Tag == tag.Substring(8, (tag.Length - 1)));
            int idmenu = int.Parse(groupproduct.id.ToString());
             var kiemtra = db.tblConnectManuProducts.Where(p => p.idCate == idmenu).ToList();
            if (kiemtra.Count > 0)
            {
                Session["idManu"] = "";
                int idManu = int.Parse(kiemtra[0].idManu.ToString());
                tblManufacture manufacture = db.SelectListItem.Find(idManu);
                ViewBag.Namemanu = manufacture.Name;
                ViewBag.Info = manufacture.Content;
                ViewBag.name = groupproduct.Name;
                ViewBag.color = manufacture.Color;
                ViewBag.email = manufacture.Email;
                ViewBag.website = manufacture.Website;
                Session["idManu"] = idManu;
                ViewBag.favicon = " <link href=\"" + manufacture.Favicon + "\" rel=\"icon\" type=\"image/x-icon\" />";
                ViewBag.imagemanu = manufacture.Images;

                string moth = "";
                int moths = int.Parse(DateTime.Now.Month.ToString());
                if (moths <= 3)
                {
                    moth = "tháng 1,2,3 ";
                }
                
                else if (moths > 3 && moths <= 6)
                {
                    moth = "tháng 4,5,6 ";
                }
                else if (moths > 6 && moths <= 9)
                {
                    moth = "tháng 7,8,9 ";
                }
                else if (moths >= 9 && moths <= 12)
                {
                    moth = "tháng 10,11,12 ";
                }

                ViewBag.Title = "<title>Bảng báo giá " + groupproduct.Name + " " + moth + "năm " + DateTime.Now.Year + " rẻ nhất HN</title>";
                ViewBag.Description = "<meta name=\"description\" content=\"Bảng báo giá " + groupproduct.Name + ", dịch vụ thay lõi lọc nước " + manufacture.Name + " tại nhà . " + tblcongif.Name + " là nhà phân phối chính thức Lõi máy lọc nước " + manufacture.Name + "  .\"/>";
                ViewBag.Keyword = "<meta name=\"keywords\" content=\"Bảng Báo giá sản phẩm " + groupproduct.Name + "\" /> ";
                string meta = "";
                ViewBag.canonical = "<link rel=\"canonical\" href=\"http://Kangaroochinhhang.com/bao-gia/" + groupproduct.Tag + "\" />";
                meta += "<meta itemprop=\"name\" content=\"Bảng báo giá " + groupproduct.Name + " " + moth + "năm " + DateTime.Now.Year + " rẻ nhất HN\" />";
                meta += "<meta itemprop=\"url\" content=\"" + Request.Url.ToString() + "\" />";
                meta += "<meta itemprop=\"description\" content=\"Bảng báo giá " + groupproduct.Name + ", dịch vụ thay lõi lọc nước " + manufacture.Name + " tại nhà . " + tblcongif.Name + " là nhà phân phối chính thức Lõi máy lọc nước " + manufacture.Name + "\" />";
                meta += "<meta itemprop=\"image\" content=\"http://Kangaroochinhhang.com" + groupproduct.Images + "\" />";
                meta += "<meta property=\"og:title\" content=\"Bảng báo giá " + groupproduct.Name + " " + moth + "năm " + DateTime.Now.Year + " rẻ nhất HN\" />";
                meta += "<meta property=\"og:type\" content=\"product\" />";
                meta += "<meta property=\"og:url\" content=\"" + Request.Url.ToString() + "\" />";
                meta += "<meta property=\"og:image\" content=\"http://Kangaroochinhhang.com" + groupproduct.Images + "\" />";
                meta += "<meta property=\"og:site_name\" content=\"http://Kangaroochinhhang.com\" />";
                meta += "<meta property=\"og:description\" content=\"" + groupproduct.Description + "\" />";
                meta += "<meta property=\"fb:admins\" content=\"\" />";
                ViewBag.Meta = meta;
                ViewBag.nUrl = "<ol itemscope itemtype=\"http://schema.org/BreadcrumbList\">   <li itemprop=\"itemListElement\" itemscope  itemtype=\"http://schema.org/ListItem\"> <a itemprop=\"item\" href=\"http://Kangaroochinhhang.com\">  <span itemprop=\"name\">Trang chủ</span></a> <meta itemprop=\"position\" content=\"1\" />  </li>   › Báo giá " + groupproduct.Name + "</ol> ";

                StringBuilder schame = new StringBuilder();
                schame.Append("<script type=\"application/ld+json\">");
                schame.Append("{");
                schame.Append("\"@context\": \"http://schema.org\",");
                schame.Append("\"@type\": \"NewsArticle\",");
                schame.Append("\"headline\": \"" + groupproduct.Name + "\",");
                schame.Append(" \"datePublished\": \"" + groupproduct.DateCreate + "\",");
                schame.Append("\"image\": [");
                schame.Append(" \"" + manufacture.Images.Remove(0, 1) + "\"");
                schame.Append(" ]");
                schame.Append("}");
                schame.Append("</script> ");
                ViewBag.schame = schame.ToString();

                var listId = db.tblGroupProducts.Where(p => p.ParentID == idmenu).Select(p => p.id).ToList();
                listId.Add(idmenu);
                string chuoi = "";
                var listproduct = db.tblProducts.Where(p => p.Active == true && listId.Contains(p.idCate.Value)).OrderBy(p => p.Ord).ToList();
                for (int i = 0; i < listproduct.Count; i++)
                {
                    chuoi += "<tr>";
                    chuoi += "<td class=\"Ords\">" + (i + 1) + "</td>";
                    chuoi += "<td class=\"Names\">";
                    string note = "";
                    if (listproduct[i].Sale != null && listproduct[i].Sale != "")
                        note = "<span>" + listproduct[i].Access + "</span>";

                    chuoi += "<a href=\"/1/" + listproduct[i].Tag + "\" title=\"" + listproduct[i].Name + "\">" + listproduct[i].Name + " " + note + " </a>";
                    chuoi += "<span class=\"n2\">Chức năng : " + listproduct[i].NoteInfo + "</span>";
                    chuoi += "<span class=\"n3\">Chính hãng " + manufacture.Name + " </span>";
                    chuoi += " </td>";
                    chuoi += "<td class=\"Codes\"><a href=\"/Tabs/" + listproduct[i].Code + "\" title=\"" + listproduct[i].Code + "\">" + listproduct[i].Code + "</a></td>";
                    chuoi += "<td class=\"Wans\"><a href=\"/1/" + listproduct[i].Tag + "\" title=\"" + listproduct[i].Name + "\"><img src=\"" + listproduct[i].ImageLinkThumb + "\" alt=\"" + listproduct[i].Name + "\" title=\"" + listproduct[i].Name + "\"/></a>" + listproduct[i].Time + "</td>";
                    chuoi += "<td class=\"Prices\">" + string.Format("{0:#,#}", listproduct[i].PriceSale) + "đ  <span class=\"n4\">Miễn phí lắp đặt tại Hà Nội, Miễn phí chuyển hàng toàn quốc</span></td>";
                    //chuoi += "<td class=\"Qualitys\">01</td>";
                    //chuoi += "<td class=\"SumPrices\">" + string.Format("{0:#,#}", listproduct[i].PriceSale) + "đ</td>";
                    chuoi += "<td class=\"Images\"><a href=\"/1/" + listproduct[i].Tag + "\" title=\"" + listproduct[i].Name + "\"><img src=\"" + listproduct[i].ImageLinkThumb + "\" alt=\"" + listproduct[i].Name + "\" title=\"" + listproduct[i].Name + "\"/></a></td>";
                    chuoi += "</tr>";
                }
                ViewBag.chuoi = chuoi;
            }
            return View(tblcongif);
        }
    }
}
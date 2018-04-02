using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Kangaroochinhhang.Models;

namespace Kangaroochinhhang.Controllers.DisplayCustom
{
    public class manufacturesController : Controller
    {
        private KangaroochinhhangContext db = new KangaroochinhhangContext();

        // GET: manufactures
        public ActionResult Index(string tag)
        {
            Session["idManu"] = "";
            var manufactures = db.SelectListItem.First(p => p.Tag == tag);
            int idManu = int.Parse(manufactures.id.ToString());
            var listIdCate = db.tblConnectManuProducts.Where(p => p.idManu == idManu).Select(p => p.idCate).ToList();
            var MenuParent = db.tblGroupProducts.Where(p => listIdCate.Contains(p.id) && p.Active == true).OrderBy(p => p.Ord).ToList();
            ViewBag.Title = "<title>" + manufactures.Title + "</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"" + manufactures.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + manufactures.Name + "\" /> ";
            Session["idManu"] = manufactures.id;
            ViewBag.canonical = "<link rel=\"canonical\" href=\"http://Kangaroochinhhang.com/" + manufactures.Tag + "\" />";

            ViewBag.favicon = " <link href=\"" + manufactures.Favicon + "\" rel=\"icon\" type=\"image/x-icon\" />";
            return View();
        }
        public PartialViewResult partialdefault()
        {
             
            return PartialView(db.SelectListItem.First());
        }
        public PartialViewResult partialHeadHome()
        {
            int id = 0;
            if (Session["idManu"] != null && Session["idManu"] != "")
            {
                id = int.Parse(Session["idManu"].ToString());
            }

            var listId = db.tblConnectManuToImages.Where(p => p.idManu == id).Select(p => p.idImage).ToList();
            var listImage = db.tblImages.Where(p => p.idCate == 1 && listId.Contains(p.id) && p.Active == true).OrderBy(p => p.Ord).ToList();
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < listImage.Count; i++)
            {
                result.Append("<a href=\"" + listImage[i].Url + "\" title=\"" + listImage[i].Name + "\"><img src=\"" + listImage[i].Images + "\" data-thumb=\"" + listImage[i].Images + "\" alt=\"" + listImage[i].Name + "\" /></a>");
            }
            ViewBag.result = result.ToString();
            var listImageAdw = db.tblImages.Where(p => p.idCate == 6 && listId.Contains(p.id) && p.Active == true).OrderBy(p => p.Ord).ToList();
            StringBuilder resultAdw = new StringBuilder();
            for (int i = 0; i < listImageAdw.Count; i++)
            {
                resultAdw.Append("<a href=\"" + listImageAdw[i].Url + "\" title=\"" + listImageAdw[i].Name + "\"><img src=\"" + listImageAdw[i].Images + "\" data-thumb=\"" + listImageAdw[i].Images + "\" alt=\"" + listImageAdw[i].Name + "\" /></a>");
            }
            ViewBag.resultAdw = resultAdw.ToString();
            return PartialView();
        }

        public PartialViewResult productSaleHomes()
        {
            int idManu = 0;
            if (Session["idManu"] != null && Session["idManu"] != "")
            {
                idManu = int.Parse(Session["idManu"].ToString());

                var listId = db.tblConnectManuProducts.Where(p => p.idManu == idManu).Select(p => p.idCate).ToList();
                var listProduct = db.tblProducts.Where(p => p.Active == true && p.ProductSale == true && listId.Contains(p.idCate)).OrderByDescending(p => p.DateCreate).Take(12).ToList();
                StringBuilder result = new StringBuilder();
                for (int i = 0; i < listProduct.Count; i++)
                {
                    result.Append(" <div class=\"item\">");
                    float price = float.Parse(listProduct[i].Price.ToString());
                    float pricesale = float.Parse(listProduct[i].PriceSale.ToString());
                    float phantram = 100 - ((pricesale * 100) / price);
                    result.Append(" <div class=\"sale\">" + Convert.ToInt32(phantram) + "%</div>");
                    result.Append("<div class=\"img\">");
                    result.Append("<a href=\"/1/" + listProduct[i].Tag + "\" title=\"" + listProduct[i].Name + "\"><img src=\"" + listProduct[i].ImageLinkThumb + "\" alt=\"" + listProduct[i].Name + "\" /></a>");
                    result.Append("</div>");
                    result.Append("<a class=\"name\" href=\"/1/" + listProduct[i].Tag + "\" title=\"" + listProduct[i].Name + "\">" + listProduct[i].Name + "</a>");
                    result.Append("<div class=\"boxItem\">");
                    result.Append("<div class=\"boxPrice\">");
                    result.Append("<span class=\"priceSale\">" + string.Format("{0:#,#}", listProduct[i].PriceSale) + "đ</span>");
                    result.Append("<span class=\"price\">" + string.Format("{0:#,#}", listProduct[i].Price) + "đ</span>");
                    result.Append("</div>");
                    result.Append("<div class=\"boxSale\">");
                    result.Append("<a href=\"\" title=\"\"></a>");
                    result.Append("</div>");
                    result.Append("</div>");
                    result.Append("</div>");
                }
                ViewBag.result = result.ToString();
                return PartialView(db.SelectListItem.FirstOrDefault(p => p.id == idManu));
            }
            return PartialView();
        }

        public PartialViewResult productHomes()
        {
            int idManu = 0;
            if (Session["idManu"] != null && Session["idManu"] != "")
            {
                idManu = int.Parse(Session["idManu"].ToString());
                var manufactures = db.SelectListItem.Find(idManu);
                var listId = db.tblConnectManuProducts.Where(p => p.idManu == idManu).Select(p => p.idCate).ToList();
                var groupProduct = db.tblGroupProducts.Where(p => p.Active == true && p.Priority == true && listId.Contains(p.id)).OrderBy(p => p.Ord).ToList();
                StringBuilder result = new StringBuilder();
                StringBuilder resultIcon = new StringBuilder();
                for (int i = 0; i < groupProduct.Count; i++)
                {
                    resultIcon.Append("<li class=\"current\"><a href=\"#neo-" + i + "\" class=\"neo\"><img src=\"" + groupProduct[i].iCon + "\" alt=\"Tên tầng " + i + "\" title=\"Tên tầng " + i + "\"></a></li>");
                    result.Append("<div class=\"products\">");
                    result.Append("<div id=\"neo-" + i + "\" class=\"cneo\"></div>");
                    result.Append("<div class=\"Floor\" style=\"background:" + manufactures.Color + "\">");
                    result.Append("<div class=\"Content_Floor\">");
                    result.Append("<div class=\"LeftFloor\" style=\"background:" + manufactures.Color + "\">");
                    result.Append("<div class=\"Leftfloor1\"><span>"+ manufactures.Name + " </span></div>");
                    result.Append("<div class=\"circle\" style=\"color:" + manufactures.Color + "\">"+(i+1)+"</div>");
                    result.Append("</div>");
                    result.Append("<div class=\"Center_Floor\">");
                    result.Append("<h3><a href=\"/0/" + groupProduct[i].Tag + "\" title=\"" + groupProduct[i].Name + "\">" + groupProduct[i].Name + "</a></h3>");
                    result.Append("</div>");
                    result.Append("<div class=\"Menufloor\">");
                    int idCate = groupProduct[i].id;
                    var listIdChild = db.tblGroupProducts.Where(p => p.ParentID == idCate && p.Active == true).Select(p => p.id).ToList();
                    var groupProductChild = db.tblGroupProducts.Where(p => p.Active == true && listIdChild.Contains(p.id)).OrderBy(p => p.Ord).ToList();
                    for (int j = 0; j < groupProductChild.Count; j++)
                    {
                        result.Append("<a href=\"/0/" + groupProductChild[j].Tag + "\" title=\"" + groupProductChild[j].Name + "\">" + groupProductChild[j].Name + "</a>");
                    }

                    result.Append(" </div>");
                    result.Append("<div class=\"RightFloor\">");
                    result.Append("<div class=\"stairs\">");
                    if(i<groupProduct.Count-1)
                    result.Append("<a href=\"#neo-" + (i + 1) + "\" title=\"Xuống tầng\"><i class=\"down\"></i> </a>");
                    result.Append("<i class=\"Elevator\"></i>");
                    if(i>0)
                    result.Append("<a href=\"#neo-" + (i - 1) + "\" title=\"Lên tầng\"><i class=\"up\"></i></a>");
                    result.Append("</div>");
                    result.Append("</div>");
                    result.Append("</div>");
                    result.Append("</div>");
                    result.Append("<div class=\"contentProducts\">");

                    result.Append("<div class=\"adwProducts\">");
                    var listIdChildImage = db.tblConnectImages.Where(p => p.idCate == idCate).Select(p => p.idImg).ToList();
                    var listImages = db.tblImages.Where(p => p.Active == true && p.idCate == 7 && listIdChildImage.Contains(p.id)).OrderBy(p => p.Ord).ToList();
                    for (int j = 0; j < listImages.Count; j++)
                    {
                        result.Append("<a href=\"" + listImages[j].Url + "\" title=\"" + listImages[j].Name + "\"><img src=\"" + listImages[j].Images + "\" alt =\"" + listImages[j].Name + "\" /></a>");
                    }

                    result.Append("</div>");

                    result.Append("<div class=\"listProducts\">");
                    listIdChild.Add(idCate);

                    var listProduct = db.tblProducts.Where(p => p.Active == true && p.ViewHomes == true && listIdChild.Contains(p.idCate.Value)).OrderBy(p => p.Ord).ToList();
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
                    result.Append(" </div>");

                    result.Append("  </div>");

                    result.Append("  </div>");
                }
                ViewBag.result = result.ToString();
                ViewBag.resultIcon = resultIcon.ToString();
            }
            return PartialView();
        }

        public PartialViewResult newsVideoHomes()
        {
            int idManu = 0;
            if (Session["idManu"] != null && Session["idManu"] != "")
            {
                idManu = int.Parse(Session["idManu"].ToString());
                var manufactures = db.SelectListItem.Find(idManu);
                ViewBag.name = manufactures.Name;
                ViewBag.code = manufactures.Video;
                ViewBag.color = manufactures.Color;
                var listId = db.tblConnectManuToNews.Where(p => p.idManu == idManu).Select(p => p.idNews).ToList();
                var listNews = db.tblNews.Where(p => p.Active == true && listId.Contains(p.id)).OrderByDescending(p => p.DateCreate).Take(3).ToList();
                StringBuilder result = new StringBuilder();
                for(int i=0;i<listNews.Count;i++)
                {
                    result.Append("<div class=\"tearNews\">");
                    result.Append("<a href=\"/3/"+listNews[i].Tag+ "\" title=\"" + listNews[i].Name + "\"><img src=\"" + listNews[i].Images + "\" alt=\"\" /></a>");
                    result.Append("<a class=\"name\" href=\"/3/" + listNews[i].Tag + "\" title=\"" + listNews[i].Name + "\">" + listNews[i].Name + "</a>");
                    result.Append("<span> " + listNews[i].Description + "</span>");
                    result.Append(" <span class=\"times\">Ngày cập nhật : " + listNews[i].DateCreate + "</span>");
                    result.Append("</div>");
                }
                ViewBag.result = result;
            }
            return PartialView();
        }
    }
}
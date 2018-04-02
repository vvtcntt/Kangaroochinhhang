using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kangaroochinhhang.Models;
using PagedList;
using PagedList.Mvc;
using System.Globalization;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
 using System.Runtime.InteropServices;
namespace Kangaroochinhhang.Controllers.Admin.Product
{
    public class ProductadController : Controller
    {
        List<SelectListItem> carlist = new List<SelectListItem>();
        private KangaroochinhhangContext db = new KangaroochinhhangContext();
        // GET: Productad
        public ActionResult Index(int? page, string text, string idCate, string pageSizes, FormCollection collection)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            if (ClsCheckRole.CheckQuyen(4, 0, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
            {
                #region[Load Menu]

                var pro = db.tblGroupProducts.OrderByDescending(p => p.Ord).Take(1).ToList();
                var menuModel = db.tblGroupProducts.Where(m => m.ParentID == null).OrderBy(m => m.id).ToList();
                 carlist.Clear();
                string strReturn = "---";
                foreach (var item in menuModel)
                {
                    carlist.Add(new SelectListItem { Text = item.Name, Value = item.id.ToString() });
                    StringClass.DropDownListFor(item.id, carlist, strReturn);
                    strReturn = "---";
                }
                if (idCate != "")
                {
                    ViewBag.drMenu = new SelectList(carlist, "Value", "Text", idCate);
                    ViewBag.idCate = idCate;
                    ViewBag.idMenu = idCate;
                }
                else
                {
                    ViewBag.drMenu = carlist;
                }
                #endregion
                if (Session["Thongbao"] != null && Session["Thongbao"] != "")
                {

                    ViewBag.thongbao = Session["Thongbao"].ToString();
                    Session["Thongbao"] = "";
                }
                if (collection["btnDelete"] != null)
                {
                    foreach (string key in Request.Form.Keys)
                    {
                        var checkbox = "";
                        if (key.StartsWith("chk_"))
                        {
                            checkbox = Request.Form["" + key];
                            if (checkbox != "false")
                            {
                                if (ClsCheckRole.CheckQuyen(4, 3, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
                                {
                                    int id = Convert.ToInt32(key.Remove(0, 4));
                                    tblProduct tblproduct = db.tblProducts.Find(id);
                                    int ord = int.Parse(tblproduct.Ord.ToString());
                                    int idCates = int.Parse(tblproduct.idCate.ToString());
                                    var kiemtra = db.tblProducts.Where(p => p.Ord > ord && p.idCate == idCates).ToList();
                                    if (kiemtra.Count > 0)
                                    {
                                        var listproduct = db.tblProducts.Where(p => p.Ord > ord && p.idCate == idCates).ToList();
                                        for (int i = 0; i < listproduct.Count; i++)
                                        {
                                            int idp = int.Parse(listproduct[i].id.ToString());
                                            var ProductUpdate = db.tblProducts.Find(idp);
                                            ProductUpdate.Ord = ProductUpdate.Ord - 1;
                                            db.SaveChanges();
                                        }
                                    }
                                    db.tblProducts.Remove(tblproduct);
                                    db.SaveChanges();
                                    clsSitemap.DeteleSitemap(id.ToString(), "Product");

                                }
                                else
                                {
                                    return Redirect("/Users/Erro");

                                }
                            }
                        }
                    }
                    //dsd
                }
                return View();
            }
            else
            {
                return Redirect("/Users/Erro");

            }
        }

        public PartialViewResult PartialProductData(int? page, string text, string idCate, string pageSizes)
        {
            var listProduct = db.tblProducts.OrderByDescending(p => p.DateCreate).ToList();
            int pageSize = 20;
            var pageNumber = (page ?? 1);
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


            if (Request.IsAjaxRequest())
            {
                int idCatelogy;
                if (pageSizes != null)
                {
                    ViewBag.pageSizes = pageSizes;
                    pageSize = int.Parse(pageSizes.ToString());
                    ViewBag.chuoicout = "<span style='color: #A52A2A;'>" + pageSize + "</span> / <span style='color: #333;'>" + listProduct.Count.ToString() + "</span>";
                    return PartialView("PartialProductData", listProduct.ToPagedList(pageNumber, pageSize));

                }
                if (text != null && text != "")
                {
                    listProduct = db.tblProducts.Where(p => p.Name.ToUpper().Contains(text.ToUpper())).OrderByDescending(p => p.DateCreate).ToList();
                    ViewBag.chuoicout = "<span style='color: #A52A2A;'>" + listProduct.Count + "</span> ";

                    return PartialView("PartialProductData", listProduct.ToPagedList(pageNumber, pageSize));
                }
                if (idCate != null && idCate != "")
                {
                    ViewBag.idcate = idCate;
                    idCatelogy = int.Parse(idCate);
                    listProduct = db.tblProducts.Where(p => p.idCate == idCatelogy).OrderByDescending(p => p.DateCreate).ToList();
                    ViewBag.chuoicout = "<span style='color: #A52A2A;'>" + listProduct.Count + "</span> ";
                    ViewBag.idMenu = idCate;
                    return PartialView("PartialProductData", listProduct.ToPagedList(pageNumber, pageSize));
                }
                 
                else
                {
                    listProduct = db.tblProducts.OrderByDescending(p => p.Ord).ToList();

                }

            }



            if (pageSizes != null)
            {
                ViewBag.pageSizes = pageSizes;
                pageSize = int.Parse(pageSizes.ToString());

            }
            ViewBag.chuoicout = "<span style='color: #A52A2A;'>" + pageSize + "</span> / <span style='color: #333;'>" + listProduct.Count.ToString() + "</span>";

            var menuModel = db.tblGroupProducts.Where(m => m.ParentID == null).OrderBy(m => m.id).ToList();
            carlist.Clear();
            string strReturn = "---";
            foreach (var item in menuModel)
            {
                carlist.Add(new SelectListItem { Text = item.Name, Value = item.id.ToString() });
                StringClass.DropDownListFor(item.id, carlist, strReturn);
                strReturn = "---";
            }
            if (text != null && text != "")
            {
                listProduct = db.tblProducts.Where(p => p.Name.ToUpper().Contains(text.ToUpper()) ).OrderByDescending(p => p.DateCreate).ToList();
                ViewBag.chuoicout = "<span style='color: #A52A2A;'>" + listProduct.Count + "</span> ";
                ViewBag.Text = text;
                return PartialView("PartialProductData", listProduct.ToPagedList(pageNumber, pageSize));
            }
            if (idCate != null && idCate != "")
            {

                int idcates = int.Parse(idCate);
                listProduct = db.tblProducts.Where(p => p.idCate == idcates ).OrderByDescending(p => p.DateCreate).ToList();
                ViewBag.idMenu = idCate;
                ViewBag.idcate = idCate;
                ViewBag.ddlMenu = carlist;
                return PartialView(listProduct.ToPagedList(pageNumber, pageSize));


            }
            else
            {
                ViewBag.ddlMenu = carlist;
                listProduct = db.tblProducts.OrderByDescending(p => p.DateCreate).ToList();
                return PartialView(listProduct.ToPagedList(pageNumber, pageSize));
            }

            return PartialView(listProduct.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult UpdateInfoProduct(string code,string productid,string chkPri, string price, string saleprice, string cbIsActive, string chkHome, string chkSale, string ordernumber, string idCate, string Status)
        {
            if (ClsCheckRole.CheckQuyen(4, 2, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
            {
                int id = int.Parse(productid);
                var Product = db.tblProducts.Find(id);
                Product.Status = bool.Parse(Status);
                Product.Price = int.Parse(price);
                Product.PriceSale = int.Parse(saleprice);
                Product.Code = code;
                Product.ViewHomes = bool.Parse(chkHome);
                Product.Active = bool.Parse(cbIsActive);
                Product.ProductSale = bool.Parse(chkSale);
                Product.idCate = int.Parse(idCate);
                int idcates = int.Parse(idCate); ;
                int Ord = int.Parse(ordernumber);
                var Kiemtra = db.tblProducts.Where(p => p.Ord == Ord && p.idCate == idcates && p.id!=id).ToList();
                if (Kiemtra.Count > 0)
                {
                    var listProduct = db.tblProducts.Where(p => p.Ord >= Ord && p.idCate == idcates).ToList();   
                    for(int i=0;i<listProduct.Count;i++)
                    {
                        int idp = int.Parse(listProduct[i].id.ToString());
                        var productUpdate = db.tblProducts.Find(idp);
                        productUpdate.Ord = productUpdate.Ord + 1;
                        db.SaveChanges();
                    }
                }
                Product.Ord = Ord;
                db.SaveChanges();
                //db.Entry(Product).State = System.Data.EntityState.Modified;
                var result = string.Empty;
                result = "Thành công";
                if(chkPri=="true")
                {
                    var getHtmlWeb = new HtmlWeb();
                    int idcate = int.Parse(idCate);
                    var listconnectweb = db.tblConnectWebs.Where(p => p.idCate == idcate).ToList();
                    List<int> Mang = new List<int>();
                    for (int i = 0; i < listconnectweb.Count; i++)
                    {
                        Mang.Add(int.Parse(listconnectweb[i].idWeb.ToString()));
                    }
                    var listweb = db.tblWebs.Where(p => Mang.Contains(p.id)).ToList();
                    for (int i = 0; i < listweb.Count; i++)
                    {

                        getHtmlWeb.Load("" + listweb[i].Url + "/Productad/UpdateProduct?Code=" + code + "&Price=" + price + "&PriceSale=" + saleprice + "&Active=" + cbIsActive + "&Status=" + Status + "");

                    }

                }
                return Json(new { result = result });
            }
            else
            {
                var result = string.Empty;
                result = "Bạn không có quyền truy cập tính năng này";
                return Json(new { result = result });
            }
        }
        public ActionResult EditPrice(string chkPri)
        {
            var result = string.Empty;
            if(chkPri=="true")
            { Session["Price"] = "1";
            result = "Bạn đã chọn thay đổi giá tại trang con";

            }
            else
            { Session["Price"] = "0";
            result = "Bạn đã tắt thay đổi giá ở trang con";
            }
            
           
            return Json(new { result = result });
        }
        public ActionResult UpdateTime(string id)
        {
            var result = string.Empty;
            if (ClsCheckRole.CheckQuyen(4, 2, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
            {
                int idp = int.Parse(id);
                var Product = db.tblProducts.Find(idp);
                Product.DateCreate = DateTime.Now;
                db.SaveChanges();
                result = "Làm mới thành công";
            }
            else
                result = "Bạn không có quyền thay đổi chức năng này !";
            return Json(new { result = result });
        }

        public ActionResult Create(string id)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }

            if (Session["Thongbao"] != null && Session["Thongbao"] != "")
            {

                ViewBag.thongbao = Session["Thongbao"].ToString();
                Session["Thongbao"] = "";
            }
            if (ClsCheckRole.CheckQuyen(4, 1, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
            {
                var menuModel = db.tblGroupProducts.Where(m => m.ParentID == null).OrderBy(m => m.id).ToList();
                carlist.Clear();
                string strReturn = "---";
                foreach (var item in menuModel)
                {
                    carlist.Add(new SelectListItem { Text = item.Name, Value = item.id.ToString() });
                    StringClass.DropDownListFor(item.id, carlist, strReturn);
                    strReturn = "---";
                }

                if (id != "")
                {
                    int ids = int.Parse(id);

                    var pro = db.tblProducts.Where(p => p.idCate == ids).OrderByDescending(p => p.Ord).Take(1).ToList();

                    ViewBag.drMenu = new SelectList(carlist, "Value", "Text", id);
                    int idcate = int.Parse(id.ToString());
                    pro = db.tblProducts.Where(p => p.idCate == idcate).OrderByDescending(p => p.Ord).Take(1).ToList();
                    if (pro.Count > 0)
                        ViewBag.Ord = pro[0].Ord + 1;
                    else
                        ViewBag.Ord = "1";
                    int idCates = int.Parse(id);
                    var Listconnectcre = db.tblGroupCriterias.Where(p => p.idCate == idCates).ToList();
                    List<int> Mang = new List<int>();
                    for (int i = 0; i < Listconnectcre.Count; i++)
                    {
                        Mang.Add(int.Parse(Listconnectcre[i].idCri.ToString()));
                    }

                    var listCre = db.tblCriterias.Where(p => Mang.Contains(p.id)).ToList();
                    string chuoi = "";
                    for (int i = 0; i < listCre.Count; i++)
                    {

                        chuoi += "<tr>";
                        chuoi += "<td class=\"key\">" + listCre[i].Name + "";
                        chuoi += "</td>";
                        chuoi += "<td>";
                        int idcre = int.Parse(listCre[i].id.ToString());
                        var listInfo = db.tblInfoCriterias.Where(p => p.idCri == idcre && p.Active == true).OrderBy(p => p.Ord).ToList();
                        for (int j = 0; j < listInfo.Count; j++)
                        {

                            chuoi += " <div class=\"boxcheck\">";
                            chuoi += "<label><input type=\"checkbox\" name=\"chkCre_" + listInfo[j].id + "\" id=\"chkCre_" + listInfo[j].id + "\" />" + listInfo[j].Name + "</label>";
                            chuoi += "</div>";
                        }


                        chuoi += "</td>";
                        chuoi += "</tr>";
                    }
                    ViewBag.chuoi = chuoi;
                }
                else
                {
                    ViewBag.drMenu = carlist;
                    var pro = db.tblProducts.OrderByDescending(p => p.Ord).Take(1).ToList();
                    if (pro.Count > 0)
                        ViewBag.Ord = pro[0].Ord + 1;
                }
                

                    return View();
            }
            else
            {
                return Redirect("/Users/Erro");

            }
        }
        [HttpPost]
        [ValidateInput(false)]

        public ActionResult Create(tblProduct tblproduct, FormCollection Collection, string id, List<HttpPostedFileBase> uploadFile)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }

           
            string nidCate = Collection["drMenu"];
            if (nidCate != "")
            {
                tblproduct.idCate = int.Parse(nidCate);
                int idcate = int.Parse(nidCate);
                tblproduct.DateCreate = DateTime.Now;
                tblproduct.Tag = StringClass.NameToTag(tblproduct.Name);
                tblproduct.DateCreate = DateTime.Now;
                tblproduct.Visit = 0;
                string capacity = Collection["Capacity"];     
                string[] listarray = tblproduct.ImageLinkDetail.Split('/');
                string ImageLinkDetail = Collection["ImageLinkDetail"];
                string imagethum = listarray[listarray.Length - 1];
                tblproduct.ImageLinkThumb = "/Images/_thumbs/Images/" + imagethum;
                db.tblProducts.Add(tblproduct);
                db.SaveChanges();
                var listprro = db.tblProducts.OrderByDescending(p => p.id).Take(1).ToList();
                clsSitemap.CreateSitemap("1/" + tblproduct.Tag, listprro[0].id.ToString(), "Product");
                #region[Updatehistory]
                Updatehistoty.UpdateHistory("Create Product", Request.Cookies["Username"].Values["Username"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                #endregion
                var listproduct = db.tblProducts.OrderByDescending(p => p.id).Take(1).ToList();
                int idp = 0;
                if (listproduct.Count > 0)
                    idp = int.Parse(listproduct[0].id.ToString());
                TempData["Msg"] = "";
                string abc = "";
                string def = "";
                if (uploadFile != null)
                {
                    foreach (var item in uploadFile)
                    {
                        if (item != null)
                        {
                            string filename = item.FileName;
                            string path = System.IO.Path.Combine(Server.MapPath("~/Images/ImagesList"), System.IO.Path.GetFileName(item.FileName));
                            item.SaveAs(path);
                            abc = string.Format("Upload {0} file thành công", uploadFile.Count);
                            def += item.FileName + "; ";
                            tblImageProduct imgp = new tblImageProduct();
                            imgp.idProduct = idp;
                            imgp.Images = "/Images/ImagesList/" + System.IO.Path.GetFileName(item.FileName);
                            db.tblImageProducts.Add(imgp);
                            db.SaveChanges();
                        }

                    }
                    TempData["Msg"] = abc + "</br>" + def;
                }

             
                //Thêm thuộc tính
                foreach (string key in Request.Form.Keys)
                {
                    var checkbox = "";
                    if (key.StartsWith("chkCre_"))
                    {
                        checkbox = Request.Form["" + key];
                        if (checkbox != "false")
                        {
                            int idCri = Convert.ToInt32(key.Remove(0, 7));
                            tblConnectCriteria tblconnectcre = new tblConnectCriteria();
                            tblconnectcre.idCre = idCri;
                            tblconnectcre.idpd = idp;
                            db.tblConnectCriterias.Add(tblconnectcre);
                            db.SaveChanges();

                        }
                    }
                }
                //thêm mới tabs
                string nkeyword = tblproduct.Tabs ;
                string[] mangKeyword = nkeyword.Split(',');
                for (int i = 0; i < mangKeyword.Length; i++)
                {
                    if (mangKeyword[i] != null && mangKeyword[i] != "")
                    {
                        tblProductTag tblproductTag = new tblProductTag();
                        tblproductTag.idp = idp;
                        tblproductTag.Name = mangKeyword[i];
                        tblproductTag.Tag = StringClass.NameToTag(mangKeyword[i]);
                        db.tblProductTags.Add(tblproductTag);
                        db.SaveChanges();
                    }
                }
                if (Collection["btnSave"] != null)
                {
                    Session["Thongbao"] = "<div  class=\"alert alert-info alert1\">Bạn đã thêm sản phẩm thành công !<button class=\"close\" data-dismiss=\"alert\">×</button></div>";
                    return Redirect("/Productad/index?idCate=" + nidCate + "");
                }
                if (Collection["btnSaveCreate"] != null)
                {
                    Session["Thongbao"] = "<div  class=\"alert alert-info\">Bạn đã thêm sản phẩm thành công, mời bạn thêm sản phẩm mới !<button class=\"close\" data-dismiss=\"alert\">×</button></div>";
                    return Redirect("/Productad/Create?id=" + nidCate + "");
                }
               
            }
            return View();
        }
        public PartialViewResult ListImages(int id)
        {
            var listImages = db.tblImageProducts.Where(p => p.idProduct == id).ToList();
            string chuoi = "";
            for (int i = 0; i < listImages.Count; i++)
            {
                chuoi += " <div class=\"Tear_Images\">";
                chuoi += " <img src=\"" + listImages[i].Images + "\" alt=\"\"/>";
                chuoi += " <input type=\"checkbox\" name=\"chek-" + listImages[i].id + "\" id=\"chek-" + listImages[i].id + "\" /> Xóa";
                chuoi += "</div>";

            }
            ViewBag.chuoi = chuoi;
            return PartialView();

        }
        public ActionResult AutoOrd(string idCate)
        {

            int id = int.Parse(idCate);
            var ListProduct = db.tblProducts.Where(p => p.idCate == id).OrderByDescending(p => p.Ord).Take(1).ToList();
            var result = string.Empty;
            if (ListProduct.Count > 0)
            {
                int stt = int.Parse(ListProduct[0].Ord.ToString()) + 1;
                result = stt.ToString();
            }
            else
            {
                result = "0";

            }
            return Json(new { result = result });
        }
        public ActionResult AutoCriteria(string idCate)
        {
            var result = string.Empty;

            int idCates = int.Parse(idCate);
            var Listconnectcre = db.tblGroupCriterias.Where(p => p.idCate == idCates).ToList();
            List<int> Mang = new List<int>();
            for (int i = 0; i < Listconnectcre.Count; i++)
            {
                Mang.Add(int.Parse(Listconnectcre[i].idCri.ToString()));
            }

            var listCre = db.tblCriterias.Where(p => Mang.Contains(p.id)).ToList();
            string chuoi = "";
            for (int i = 0; i < listCre.Count; i++)
            {

                chuoi += "<tr>";
                chuoi += "<td class=\"key\">" + listCre[i].Name + "";
                chuoi += "</td>";
                chuoi += "<td>";
                int idcre = int.Parse(listCre[i].id.ToString());
                var listInfo = db.tblInfoCriterias.Where(p => p.idCri == idcre && p.Active == true).OrderBy(p => p.Ord).ToList();
                for (int j = 0; j < listInfo.Count; j++)
                {

                    chuoi += " <div class=\"boxcheck\">";
                    chuoi += "<input type=\"checkbox\" name=\"chkCre_" + listInfo[j].id + "\" id=\"chkCre_" + listInfo[j].id + "\" />" + listInfo[j].Name + "";
                    chuoi += "</div>";
                }


                chuoi += "</td>";
                chuoi += "</tr>";
            }
             result = chuoi;
            return Json(new { result = result });
        }
        public ActionResult AutoCriteriaEdit(string idCate,string id)
        {
            var result = string.Empty;

            int idCates = int.Parse(idCate);
            var Listconnectcre = db.tblGroupCriterias.Where(p => p.idCate == idCates).ToList();
            List<int> Mang = new List<int>();
            for (int i = 0; i < Listconnectcre.Count; i++)
            {
                Mang.Add(int.Parse(Listconnectcre[i].idCri.ToString()));
            }
            int idp = int.Parse(id);
            var listCre = db.tblCriterias.Where(p => Mang.Contains(p.id)).ToList();
            string chuoi = "";
            for (int i = 0; i < listCre.Count; i++)
            {

                chuoi += "<tr>";
                chuoi += "<td class=\"key\">" + listCre[i].Name + "";
                chuoi += "</td>";
                chuoi += "<td>";
                int idcre = int.Parse(listCre[i].id.ToString());
                var listInfo = db.tblInfoCriterias.Where(p => p.idCri == idcre && p.Active == true).OrderBy(p => p.Ord).ToList();
                for (int j = 0; j < listInfo.Count; j++)
                {

                    int idchk = int.Parse(listInfo[j].id.ToString());

                    var listCheck = db.tblConnectCriterias.Where(p => p.idCre == idchk && p.idpd == idp).ToList();
                    chuoi += " <div class=\"boxcheck\">";
                    if (listCheck.Count > 0)
                    {
                        chuoi += "<input type=\"checkbox\" name=\"chkCre_" + listInfo[j].id + "\" checked=\"checked\" id=\"chkCre_" + listInfo[j].id + "\" />" + listInfo[j].Name + "";
                    }
                    else
                    {
                        chuoi += "<input type=\"checkbox\" name=\"chkCre_" + listInfo[j].id + "\"   id=\"chkCre_" + listInfo[j].id + "\" />" + listInfo[j].Name + "";
                    }
                    chuoi += "</div>";
                }


                chuoi += "</td>";
                chuoi += "</tr>";
            }
            result = chuoi;
            return Json(new { result = result });
        } 
        public string CheckProduct(string text)
        {
            string chuoi = "";
            var listProduct = db.tblProducts.Where(p => p.Name == text).ToList();
            if (listProduct.Count > 0)
            {
                chuoi = "Sản phẩm bị trùng lặp !";

            }
            Session["Check"] = listProduct.Count;
            return chuoi;
        }
        public ActionResult Edit(int? id)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            if (ClsCheckRole.CheckQuyen(4, 2, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
            {
                Session["id"] = id.ToString();
                Int32 ids = Int32.Parse(id.ToString());
                tblProduct tblproduct = db.tblProducts.Find(ids);
                if (tblproduct == null)
                {
                    return HttpNotFound();
                }
                var menuModel = db.tblGroupProducts.Where(m => m.ParentID == null).OrderBy(m => m.id).ToList();
                carlist.Clear();
                string strReturn = "---";
                foreach (var item in menuModel)
                {
                    carlist.Add(new SelectListItem { Text = item.Name, Value = item.id.ToString() });
                    StringClass.DropDownListFor(item.id, carlist, strReturn);
                    strReturn = "---";
                }
                int idGroups = 0;
                if (tblproduct.idCate != null)
                {
                    idGroups = int.Parse(tblproduct.idCate.ToString());
                }

                ViewBag.drMenu = new SelectList(carlist, "Value", "Text", idGroups);
                int idCates = int.Parse(tblproduct.idCate.ToString());
                var Listconnectcre = db.tblGroupCriterias.Where(p => p.idCate == idCates).ToList();
                List<int> Mang = new List<int>();
                for (int i = 0; i < Listconnectcre.Count; i++)
                {
                    Mang.Add(int.Parse(Listconnectcre[i].idCri.ToString()));
                }

                var listCre = db.tblCriterias.Where(p => Mang.Contains(p.id)).ToList();
                string chuoi = "";
                for (int i = 0; i < listCre.Count; i++)
                {

                    chuoi += "<tr>";
                    chuoi += "<td class=\"key\">" + listCre[i].Name + "";
                    chuoi += "</td>";
                    chuoi += "<td>";
                    int idcre = int.Parse(listCre[i].id.ToString());
                    var listInfo = db.tblInfoCriterias.Where(p => p.idCri == idcre && p.Active == true).OrderBy(p => p.Ord).ToList();
                    for (int j = 0; j < listInfo.Count; j++)
                    {
                        int idchk=int.Parse(listInfo[j].id.ToString());
                        var listCheck = db.tblConnectCriterias.Where(p => p.idCre == idchk && p.idpd == ids).ToList();
                        chuoi += " <div class=\"boxcheck\">";
                        if(listCheck.Count>0)
                        {
                            chuoi += "<label><input type=\"checkbox\" name=\"chkCre_" + listInfo[j].id + "\" checked=\"checked\" id=\"chkCre_" + listInfo[j].id + "\" />" + listInfo[j].Name + "</label>";
                        }
                        else
                        {
                            chuoi += "<label><input type=\"checkbox\" name=\"chkCre_" + listInfo[j].id + "\"   id=\"chkCre_" + listInfo[j].id + "\" />" + listInfo[j].Name + "</label>";
                        }
                        chuoi += "</div>";
                    }


                    chuoi += "</td>";
                    chuoi += "</tr>";
                }
                ViewBag.chuoi = chuoi;
                
                return View(tblproduct);
            }
            else
            {
                return Redirect("/Users/Erro"); 

            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(tblProduct tblproduct, FormCollection collection, int? id, List<HttpPostedFileBase> uploadFile)
        {

            if (ModelState.IsValid)
            {
                if (collection["drMenu"] != "" || collection["drMenu"] != null)
                {
                    if (id == null)
                    {
                        id = int.Parse(collection["idProduct"]);
                        tblproduct = db.tblProducts.Find(id);
                    }
                    else
                    {
                        tblproduct = db.tblProducts.Find(id);
                    }
                    ViewBag.id = id;
                    int idCate = int.Parse(collection["drMenu"]);
                    tblproduct.idCate = idCate;
                    tblproduct.DateCreate = DateTime.Now;
                    string tag = tblproduct.Tag;
                    string Name = collection["Name"];
                    string Code = collection["Code"];
                    string Size = collection["Size"];
                    string Description = collection["Description"];
                    string Info = collection["Info"];

                    string Content = collection["Content"];
                    string Parameter = collection["Parameter"];
                    string ImageLinkDetail = collection["ImageLinkDetail"];
                    string[] listarray = ImageLinkDetail.Split('/');
                    string imagethum = listarray[listarray.Length - 1];
                    tblproduct.ImageLinkThumb = "/Images/_thumbs/Images/" + imagethum;
                    string ImageSale = collection["ImageSale"];
                    if (collection["Price"] != null)
                    {
                        float Price = float.Parse(collection["Price"]);
                        tblproduct.Price = Price;

                    }
                    if (collection["PriceSale"] != null)
                    {
                        float PriceSale = float.Parse(collection["PriceSale"]);
                        tblproduct.PriceSale = PriceSale;
                    }
                    string Warranty = collection["Warranty"];
                    string Address = collection["Address"];
                    string Sale = collection["Sale"];
                    int Ord=0;
                    if (collection["Ord"] != null)
                    {
                         Ord= int.Parse(collection["Ord"]);
                        tblproduct.Ord = Ord;
                    }
                    bool URL = (collection["URL"] == "false") ? false : true;// 
                    bool ProductSale = (collection["ProductSale"] == "false") ? false : true;//
                    bool Vat = (collection["Vat"] == "false") ? false : true;//
                    bool Status = (collection["Status"] == "false") ? false : true;//
                    bool Active = (collection["Active"] == "false") ? false : true;
                    bool New = (collection["New"] == "false") ? false : true;//
                    bool ViewHomes = (collection["ViewHomes"] == "false") ? false : true;//
                    bool Priority = (collection["Priority"] == "false") ? false : true;//
                    string Title = collection["Title"];
                    string Video = collection["Video"];
                    string Keyword = collection["Keyword"];
                    if (tblproduct.Visit != null)
                        tblproduct.Visit = tblproduct.Visit;
                    tblproduct.PriceNote = collection["PriceNote"];
                    tblproduct.NoteInfo = collection["NoteInfo"];
                    tblproduct.Tabs = collection["Tabs"];
                    tblproduct.Time = collection["Time"];
                    tblproduct.id = int.Parse(tblproduct.id.ToString());
                    tblproduct.Name = Name;
                    tblproduct.Code = Code;
                    tblproduct.ImageSale = ImageSale;
                    tblproduct.Description = Description;
                    tblproduct.ProductSale = ProductSale;
                    tblproduct.Content = Content;
                    tblproduct.Info = Info;
                    tblproduct.Size = Size;
                    tblproduct.Parameter = Parameter;
                    tblproduct.ImageLinkDetail = ImageLinkDetail;
                    tblproduct.Vat = Vat;
                    tblproduct.Warranty = Warranty;
                    tblproduct.Sale = Sale;
                    tblproduct.Active = Active;
                    tblproduct.New = New;
                    tblproduct.Priority = Priority;
                    tblproduct.Status = Status;
                    tblproduct.DateCreate = DateTime.Now;
                    tblproduct.ViewHomes = ViewHomes;
                    tblproduct.Video = Video;
                    tblproduct.Title = Title;
                    tblproduct.Keyword = Keyword;
                    string urls = db.tblGroupProducts.Find(idCate).Tag;
                    if (URL == true)
                    {
                        tblproduct.Tag = StringClass.NameToTag(tblproduct.Name);
                        var GroupProduct = db.tblGroupProducts.Find(idCate);
                        clsSitemap.UpdateSitemap("1/" + StringClass.NameToTag(tblproduct.Name), id.ToString(), "Product");

                    }
                    else
                    {
                        tblproduct.Tag = collection["NameURL"];
                        var GroupProduct = db.tblGroupProducts.Find(idCate);
                        clsSitemap.UpdateSitemap("1/" + StringClass.NameToTag(tblproduct.Name), id.ToString(), "Product");
                    }
                    if (Session["Price"] == "1")
                    {
                        var getHtmlWeb = new HtmlWeb();
                         var listconnectweb = db.tblConnectWebs.Where(p => p.idCate == idCate).ToList();
                        List<int> Mang = new List<int>();
                        for (int i = 0; i < listconnectweb.Count; i++)
                        {
                            Mang.Add(int.Parse(listconnectweb[i].idWeb.ToString()));
                        }
                        var listweb = db.tblWebs.Where(p => Mang.Contains(p.id)).ToList();
                        for (int i = 0; i < listweb.Count; i++)
                        {
                            getHtmlWeb.Load("" + listweb[i].Url + "/Productad/UpdateProduct?Code=" + tblproduct.Code + "&Price=" + tblproduct.Price + "&PriceSale=" + tblproduct.PriceSale + "&Active=" + tblproduct.Active + "&Status=" + tblproduct.Status + "");

 
                        }
                    }
                    var Kiemtra = db.tblProducts.Where(p => p.Ord == Ord && p.idCate == idCate && p.id != id).ToList();
                    if (Kiemtra.Count > 0)
                    {
                        var listProduct = db.tblProducts.Where(p => p.Ord >= Ord && p.idCate == idCate).ToList();
                        for (int i = 0; i < listProduct.Count; i++)
                        {
                            int idp = int.Parse(listProduct[i].id.ToString());
                            var productUpdate = db.tblProducts.Find(idp);
                            productUpdate.Ord = productUpdate.Ord + 1;
                            db.SaveChanges();
                        }
                    }
                    db.SaveChanges();
                    //thêm tabs
                    var listProductTag = db.tblProductTags.Where(p => p.idp == id).ToList();
                    for (int i = 0; i < listProductTag.Count; i++)
                    {
                        int ids = listProductTag[i].id;
                        tblProductTag tblproducttag = db.tblProductTags.Find(ids);
                        db.tblProductTags.Remove(tblproducttag);
                        db.SaveChanges();

                    }
                    string nkeyword = collection["Tabs"]; ;
                    string[] mangKeyword = nkeyword.Split(',');
                    for (int i = 0; i < mangKeyword.Length; i++)
                    {
                        if (mangKeyword[i] != null && mangKeyword[i] != "")
                        {
                            tblProductTag tblproductTag = new tblProductTag();
                            tblproductTag.idp = id;
                            tblproductTag.Name = mangKeyword[i];
                            tblproductTag.Tag = StringClass.NameToTag(mangKeyword[i]);
                            db.tblProductTags.Add(tblproductTag);
                            db.SaveChanges();
                        }
                    }
                    foreach (string key in Request.Form.Cast<string>().Where(key => key.StartsWith("chek-")))
                    {
                        var checkbox = "";
                        checkbox = Request.Form["" + key];
                        if (checkbox != "false")
                        {
                            Int32 idchk = Convert.ToInt32(key.Remove(0, 5));
                            tblImageProduct image = db.tblImageProducts.Find(idchk);
                            db.tblImageProducts.Remove(image);
                            db.SaveChanges();
                        }
                    }
                    TempData["Msg"] = "";
                 
                    string abc = "";
                    string def = "";
                    if (uploadFile != null)
                    {
                        foreach (var item in uploadFile)
                        {
                            if (item != null)
                            {
                                string filename = item.FileName;
                                string path = System.IO.Path.Combine(Server.MapPath("~/Images/ImagesList"), System.IO.Path.GetFileName(item.FileName));
                                item.SaveAs(path);
                                abc = string.Format("Upload {0} file thành công", uploadFile.Count);
                                def += item.FileName + "; ";
                                tblImageProduct imgp = new tblImageProduct();
                                imgp.idProduct = id;
                                imgp.Images = "/Images/ImagesList/" + System.IO.Path.GetFileName(item.FileName);
                                db.tblImageProducts.Add(imgp);
                                db.SaveChanges();
                            }

                        }
                        TempData["Msg"] = abc + "</br>" + def;
                    }
                    //Thêm các thuộc tính
                    var listconnectCre = db.tblConnectCriterias.Where(p => p.idpd == id).ToList();
                    for(int i=0;i<listconnectCre.Count;i++)
                    {
                        db.tblConnectCriterias.Remove(listconnectCre[i]);
                        db.SaveChanges();
                    }

                    foreach (string key in Request.Form.Keys)
                    {
                        var checkbox = "";
                        if (key.StartsWith("chkCre_"))
                        {
                            checkbox = Request.Form["" + key];
                            if (checkbox != "false")
                            {
                                int idCri = Convert.ToInt32(key.Remove(0, 7));
                                tblConnectCriteria tblconnectcre = new tblConnectCriteria();
                                tblconnectcre.idCre = idCri;
                                tblconnectcre.idpd = id;
                                db.tblConnectCriterias.Add(tblconnectcre);
                                db.SaveChanges();

                            }
                        }
                    }
                }
                if (collection["btnSave"] != null)
                {
                    Session["Thongbao"] = "<div  class=\"alert alert-info alert1\">Bạn đã sửa  sản phẩm thành công !<button class=\"close\" data-dismiss=\"alert\">×</button></div>";

                    return Redirect("/Productad/Index?idCate=" + int.Parse(collection["drMenu"]));
                }
                if (collection["btnSaveCreate"] != null)
                {
                    Session["Thongbao"] = "<div  class=\"alert alert-info\">Bạn đã thêm   thành công, mời bạn thêm  sản phẩm mới !<button class=\"close\" data-dismiss=\"alert\">×</button></div>";
                    return Redirect("/Productad/Create?id=" + +int.Parse(collection["drMenu"]) + "");
                }
                #region[Updatehistory]
                Updatehistoty.UpdateHistory("Update Product", Request.Cookies["Username"].Values["Username"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                #endregion
            }
            return View(tblproduct);
        }
        public ActionResult DeleteProduct(int id)
        {
            if (ClsCheckRole.CheckQuyen(4, 3, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
            {
                tblProduct tblproduct = db.tblProducts.Find(id);
                clsSitemap.DeteleSitemap(id.ToString(), "Product");
                var result = string.Empty;
                int ord = int.Parse(tblproduct.Ord.ToString());
                int idCate = int.Parse(tblproduct.idCate.ToString());
                var kiemtra = db.tblProducts.Where(p => p.Ord > ord && p.idCate == idCate).ToList();
                if(kiemtra.Count>0)
                {
                    var listproduct = db.tblProducts.Where(p => p.Ord > ord && p.idCate == idCate).ToList();
                    for(int i=0;i<listproduct.Count;i++)
                    {
                        int idp = int.Parse(listproduct[i].id.ToString());
                        var ProductUpdate = db.tblProducts.Find(idp);
                        ProductUpdate.Ord = ProductUpdate.Ord - 1;
                        db.SaveChanges();
                    }
                }

                db.tblProducts.Remove(tblproduct);
                db.SaveChanges();
 
                result = "Bạn đã xóa thành công.";
                return Json(new { result = result });
            }
            else
            {
                var result = string.Empty;

                result = "Bạn không có quyền thay đổi tính năng này !.";
                return Json(new { result = result });

            }

        }
 
        
        public JsonResult ListTag(string q)
        {
            var listTag = db.tblProducts.Where(p => p.Active == true).ToList();
            List<string> Mang = new List<string>();
            for (int i = 0; i < listTag.Count; i++)
            {
                string[] tag = listTag[i].Keyword.Split(',');
                for (int j = 0; j < tag.Length; j++)
                {
                    if (tag[j].ToUpper().Contains(q.ToUpper()))
                        Mang.Add(tag[j].ToString());
                }
            }
            var ListName = Mang.Take(15);
            return Json(new
            {
                data = ListName,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateProduct(string Code, string Price, string PriceSale, string Active)
        {
            int nPrice = int.Parse(Price);
            int nPriceSale = int.Parse(PriceSale);
            bool nActive = bool.Parse(Active);
            var tblproduct = db.tblProducts.First(p => p.Code == Code);
            tblproduct.Price = nPrice;
            tblproduct.PriceSale = nPriceSale;
            tblproduct.Active = nActive;
            db.SaveChanges();

            var result = string.Empty;
            return Json(new { result = result });
        }
        public ActionResult commandTag()
        {
            var listProduct = db.tblProducts.ToList();
            for(int i=0;i<listProduct.Count;i++)
            {
                int id = listProduct[i].id;
                try
                {
                    if (listProduct[i].Tabs != null && listProduct[i].Tabs != "")
                    {
                        string[] tabs = listProduct[i].Tabs.Split(',');
                        for (int j = 0; j < tabs.Length; j++)
                        {
                            tblProductTag productTag = new tblProductTag();
                            if (tabs[j] != null && tabs[j] != "")
                            {
                                productTag.Name = tabs[j];
                                productTag.Tag = StringClass.NameToTag(tabs[j]);
                                productTag.idp = id;
                                db.tblProductTags.Add(productTag);
                                db.SaveChanges();
                            }

                        }


                    }
                }
                catch(Exception ex)
                {

                }

            }
            return View();
        }
    }
}
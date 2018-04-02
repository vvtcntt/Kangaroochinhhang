using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kangaroochinhhang.Models;
using PagedList;
using PagedList.Mvc;
using System.Globalization;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.IO;
using System.Xml;
using System.Data.Entity;
namespace Kangaroochinhhang.Controllers.Admin
{
    public class ManufacturesadController : Controller
    {
        List<SelectListItem> carlist = new List<SelectListItem>();

        private KangaroochinhhangContext db = new KangaroochinhhangContext();

        // GET: Manufacturesad
        public ActionResult Index(string idCate, FormCollection collection)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            if (ClsCheckRole.CheckQuyen(8, 0, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
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
                                if (ClsCheckRole.CheckQuyen(7, 3, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
                                {
                                    int id = Convert.ToInt32(key.Remove(0, 4));

                                    var tblManu = db.SelectListItem.Find(id);
                                    db.SelectListItem.Remove(tblManu);
                                    db.SaveChanges();
                                    var ListManu = db.tblConnectManuProducts.Where(p => p.idManu == id).ToList();
                                    for (int i = 0; i < ListManu.Count; i++)
                                    {
                                        db.tblConnectManuProducts.Remove(ListManu[i]);
                                        db.SaveChanges();
                                    }
                                    clsSitemap.DeteleSitemap(id.ToString(), "Manufactures");

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
                if (Session["Thongbao"] != null && Session["Thongbao"] != "")
                {

                    ViewBag.thongbao = Session["Thongbao"].ToString();
                    Session["Thongbao"] = "";
                }
                return View();
            }
            else
            {
                return Redirect("/Users/Erro");

            }
        }
        public PartialViewResult partialManufactures(int? page, string text, string idCate, string pageSizes)
        {
            var ListManu = db.SelectListItem.OrderByDescending(p => p.Ord).ToList();

            int pageSize = 20;
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
            if (pageSizes != null)
            {
                ViewBag.pageSizes = pageSizes;
                pageSize = int.Parse(pageSizes.ToString());
                ViewBag.chuoicout = "<span style='color: #A52A2A;'>" + pageSize + "</span> / <span style='color: #333;'>" + ListManu.Count.ToString() + "</span>";
                return PartialView(ListManu.ToPagedList(pageNumber, pageSize));

            }
            if (idCate != "" && idCate != null)
            {
                int idmenu = int.Parse(idCate);
                List<int> mang = new List<int>();
                var ListManus = db.tblConnectManuProducts.Where(p => p.idCate == idmenu).ToList();
                for (int i = 0; i < ListManus.Count; i++)
                {
                    mang.Add(int.Parse(ListManus[i].idManu.ToString()));
                }
                ListManu = db.SelectListItem.Where(p => mang.Contains(p.id)).OrderBy(m => m.Ord).ToList();
                ViewBag.Idcha = idCate;
                return PartialView(ListManu.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                ViewBag.Idcha = 0;
            }

            return PartialView(ListManu.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult UpdateManufactures(string id, string Ord, string cbIsActive,string Priority)
        {
            if (ClsCheckRole.CheckQuyen(8, 2, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
            {
                int ids = int.Parse(id);
                var tblManu = db.SelectListItem.Find(ids);
                tblManu.Ord = int.Parse(Ord);
                tblManu.Active = bool.Parse(cbIsActive);
                tblManu.Priority = bool.Parse(Priority);
                db.SaveChanges();
                var result = string.Empty;
                result = "Thành công";
                return Json(new { result = result });
            }
            else
            {
                var result = string.Empty;
                result = "Bạn không có quyền thay đổi tính năng này";
                return Json(new { result = result });
            }

        }
        public ActionResult DeleteManufactures(int id)
        {
            if (ClsCheckRole.CheckQuyen(8, 3, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
            {
                tblManufacture tblmanu = db.SelectListItem.Find(id);
                var result = string.Empty;
                db.SelectListItem.Remove(tblmanu);
                db.SaveChanges();
                var ListManu = db.tblConnectManuProducts.Where(p => p.idManu == id).ToList();
                for (int i = 0; i < ListManu.Count; i++)
                {
                    db.tblConnectManuProducts.Remove(ListManu[i]);
                    db.SaveChanges();
                }
                clsSitemap.DeteleSitemap( id.ToString(), "Manufactures");

                result = "Bạn đã xóa thành công.";
                return Json(new { result = result });
            }
            else
            {
                var result = string.Empty;
                result = "Bạn không có quyền thay đổi tính năng này";
                return Json(new { result = result });
            }

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
            if (ClsCheckRole.CheckQuyen(8, 1, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
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
                ViewBag.MutilMenu = new SelectList(carlist, "Value", "Text", id);
                var pro = db.SelectListItem.OrderByDescending(p => p.Ord).Take(1).ToList();
                if (pro.Count > 0)
                    ViewBag.Ord = pro[0].Ord + 1;
                else
                    ViewBag.Ord = "1";
                return View();
            }
            else
            {
                return Redirect("/Users/Erro");

            }
        }

        [HttpPost]
        [ValidateInput(false)]

        public ActionResult Create(tblManufacture tblmanu, FormCollection collection, int[] MutilMenu)
        {
            tblmanu.Tag = StringClass.NameToTag(tblmanu.Name);
            db.SelectListItem.Add(tblmanu);
            db.SaveChanges();
            var ListManu = db.SelectListItem.OrderByDescending(p => p.id).Take(1).ToList();
            int idManu = int.Parse(ListManu[0].id.ToString());
            if (MutilMenu != null)
            {
                foreach (var idCate in MutilMenu)
                {
                    tblConnectManuProduct SelectListItem = new tblConnectManuProduct();
                    SelectListItem.idCate = idCate;
                    SelectListItem.idManu = idManu;
                    db.tblConnectManuProducts.Add(SelectListItem);
                    db.SaveChanges();

                }
            }
            var ListManufac = db.SelectListItem.OrderByDescending(p => p.id).Take(1).ToList();
            clsSitemap.CreateSitemap("hang-san-xuat/" + ListManufac[0].Tag, ListManufac[0].id.ToString(), "Manufactures");

            Updatehistoty.UpdateHistory("Add SelectListItem", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            if (collection["btnSave"] != null)
            {
                Session["Thongbao"] = "<div  class=\"alert alert-info alert1\">Bạn đã thêm thành công !<button class=\"close\" data-dismiss=\"alert\">×</button></div>";
                return Redirect("/Manufacturesad/Index");
            }
            if (collection["btnSaveCreate"] != null)
            {
                Session["Thongbao"] = "<div  class=\"alert alert-info\">Bạn đã thêm thành công, mời bạn thêm mới !<button class=\"close\" data-dismiss=\"alert\">×</button></div>";
                return Redirect("/Manufacturesad/Create");
            }
            return Redirect("Index");


        }
        public ActionResult Edit(int id = 0)
        {


            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            if (ClsCheckRole.CheckQuyen(8, 2, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
            {
                tblManufacture tblmanu = db.SelectListItem.Find(id);

                //int idCate = int.Parse(tblcriteria.idCate.ToString());
                var menuModel = db.tblGroupProducts.Where(m => m.ParentID == null).OrderBy(m => m.id).ToList();
                carlist.Clear();
                string strReturn = "---";
                foreach (var item in menuModel)
                {
                    carlist.Add(new SelectListItem { Text = item.Name, Value = item.id.ToString() });
                    StringClass.DropDownListFor(item.id, carlist, strReturn);
                    strReturn = "---";
                }
                var ListManu = db.tblConnectManuProducts.Where(p => p.idManu == id).ToList();
                List<int> mang = new List<int>();
                for (int i = 0; i < ListManu.Count; i++)
                {

                    mang.Add(int.Parse(ListManu[i].idCate.ToString()));

                }
                ViewBag.MutilMenu = new MultiSelectList(carlist, "Value", "Text", mang);
                if (tblmanu == null)
                {
                    return HttpNotFound();
                }
                return View(tblmanu);
            }
            else
            {
                return Redirect("/Users/Erro");


            }
        }
        [HttpPost]
        [ValidateInput(false)]

        public ActionResult Edit(tblManufacture tblmanufacture, int id, FormCollection collection, int[] MutilMenu)
        {
            if (ModelState.IsValid)
            {
                tblmanufacture.Tag = StringClass.NameToTag(tblmanufacture.Name);
                db.Entry(tblmanufacture).State = EntityState.Modified;
                db.SaveChanges();
                var ListManu = db.tblConnectManuProducts.Where(p => p.idManu == id).ToList();
                for (int i = 0; i < ListManu.Count; i++)
                {
                    db.tblConnectManuProducts.Remove(ListManu[i]);
                    db.SaveChanges();
                }
                if (MutilMenu != null)
                {
                    foreach (var idCates in MutilMenu)
                    {
                        tblConnectManuProduct SelectListItem = new tblConnectManuProduct();
                        SelectListItem.idCate = idCates;
                        SelectListItem.idManu = id;
                        db.tblConnectManuProducts.Add(SelectListItem);
                        db.SaveChanges();
                    }
                }
                clsSitemap.UpdateSitemap("hang-san-xuat/" + tblmanufacture.Tag, id.ToString(), "Manufactures");
                #region[Updatehistory]
                Updatehistoty.UpdateHistory("Edit SelectListItem", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                #endregion
                if (collection["btnSave"] != null)
                {
                    Session["Thongbao"] = "<div  class=\"alert alert-info alert1\">Bạn đã sửa  thành công !<button class=\"close\" data-dismiss=\"alert\">×</button></div>";

                    return Redirect("/Manufacturesad/Index");
                }
                if (collection["btnSaveCreate"] != null)
                {
                    Session["Thongbao"] = "<div  class=\"alert alert-info\">Bạn đã thêm thành công, mời bạn thêm mới !<button class=\"close\" data-dismiss=\"alert\">×</button></div>";
                    return Redirect("/Manufacturesad/Create");
                }
            }
            return View(tblmanufacture);
        }
    }
}
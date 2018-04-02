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
namespace Kangaroochinhhang.Controllers.Admin.WebSites
{
    public class WebController : Controller
    {
        private KangaroochinhhangContext db = new KangaroochinhhangContext();
        List<SelectListItem> carlist = new List<SelectListItem>();

        // GET: Web
        public ActionResult Index(string idCate, FormCollection collection)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            if (ClsCheckRole.CheckQuyen(7, 0, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
            {
                var ListCriteria = db.tblCriterias.OrderBy(m => m.Ord).ToList();
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

                                     var tblweb = db.tblWebs.Find(id);
                                     db.tblWebs.Remove(tblweb);
                                    db.SaveChanges();
                                    var ListWeb = db.tblConnectWebs.Where(p => p.idWeb == id).ToList();
                                    for (int i = 0; i < ListWeb.Count; i++)
                                    {
                                        db.tblConnectWebs.Remove(ListWeb[i]);
                                        db.SaveChanges();
                                    }
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
        public PartialViewResult partialWeb(int? page, string text, string idCate, string pageSizes)
        {
            var Listwebs = db.tblWebs.OrderByDescending(p => p.Ord).ToList();

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
                ViewBag.chuoicout = "<span style='color: #A52A2A;'>" + pageSize + "</span> / <span style='color: #333;'>" + Listwebs.Count.ToString() + "</span>";
                return PartialView(Listwebs.ToPagedList(pageNumber, pageSize));

            }
            if (idCate != "" && idCate != null)
            {
                int idmenu = int.Parse(idCate);
                List<int> mang = new List<int>();
                var ListWeb = db.tblConnectWebs.Where(p => p.idCate == idmenu).ToList();
                for (int i = 0; i < ListWeb.Count; i++)
                {
                    mang.Add(int.Parse(ListWeb[i].idWeb.ToString()));
                }
                Listwebs = db.tblWebs.Where(p => mang.Contains(p.id)).OrderBy(m => m.Ord).ToList();
                ViewBag.Idcha = idCate;
                return PartialView(Listwebs.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                ViewBag.Idcha = 0;
            }

            return PartialView(Listwebs.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult UpdateWeb(string id, string Ord)
        {
            if (ClsCheckRole.CheckQuyen(7, 2, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
            {
                int ids = int.Parse(id);
                var tblweb = db.tblWebs.Find(ids);
                tblweb.Ord = int.Parse(Ord);
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
        public ActionResult DeleteWeb(int id)
        {
            if (ClsCheckRole.CheckQuyen(7, 3, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
            {
                tblWeb tblweb = db.tblWebs.Find(id);
                var result = string.Empty;
                db.tblWebs.Remove(tblweb);
                db.SaveChanges();
                var Listwebs = db.tblConnectWebs.Where(p => p.idWeb == id).ToList();
                for (int i = 0; i < Listwebs.Count; i++)
                {
                    db.tblConnectWebs.Remove(Listwebs[i]);
                    db.SaveChanges();
                }
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
            if (ClsCheckRole.CheckQuyen(7, 1, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
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
                var pro = db.tblWebs.OrderByDescending(p => p.Ord).Take(1).ToList();
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
        public ActionResult Create(tblWeb tblweb, FormCollection collection, int[] MutilMenu)
        {

            db.tblWebs.Add(tblweb);
            db.SaveChanges();
            var Listwebs = db.tblWebs.OrderByDescending(p => p.id).Take(1).ToList();
            int idweb = int.Parse(Listwebs[0].id.ToString());
            if (MutilMenu != null)
            {
                foreach (var idCate in MutilMenu)
                {
                    tblConnectWeb tblconnectwebs = new tblConnectWeb();
                    tblconnectwebs.idCate = idCate;
                    tblconnectwebs.idWeb = idweb;
                    db.tblConnectWebs.Add(tblconnectwebs);
                    db.SaveChanges();

                }
            }
            Updatehistoty.UpdateHistory("Add tblWeb", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            if (collection["btnSave"] != null)
            {
                Session["Thongbao"] = "<div  class=\"alert alert-info alert1\">Bạn đã thêm thành công !<button class=\"close\" data-dismiss=\"alert\">×</button></div>";
                return Redirect("/Web/Index");
            }
            if (collection["btnSaveCreate"] != null)
            {
                Session["Thongbao"] = "<div  class=\"alert alert-info\">Bạn đã thêm thành công, mời bạn thêm mới !<button class=\"close\" data-dismiss=\"alert\">×</button></div>";
                return Redirect("/Web/Create");
            }
            return Redirect("Index");


        }
        public ActionResult Edit(int id = 0)
        {


            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            if (ClsCheckRole.CheckQuyen(7, 2, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
            {
                tblWeb tblweb = db.tblWebs.Find(id);

                 var menuModel = db.tblGroupProducts.Where(m => m.ParentID == null).OrderBy(m => m.id).ToList();
                carlist.Clear();
                string strReturn = "---";
                foreach (var item in menuModel)
                {
                    carlist.Add(new SelectListItem { Text = item.Name, Value = item.id.ToString() });
                    StringClass.DropDownListFor(item.id, carlist, strReturn);
                    strReturn = "---";
                }
                var Listweb = db.tblConnectWebs.Where(p => p.idWeb == id).ToList();
                List<int> mang = new List<int>();
                for (int i = 0; i < Listweb.Count; i++)
                {

                    mang.Add(int.Parse(Listweb[i].idCate.ToString()));

                }
                ViewBag.MutilMenu = new MultiSelectList(carlist, "Value", "Text", mang);
                if (tblweb == null)
                {
                    return HttpNotFound();
                }
                return View(tblweb);
            }
            else
            {
                return Redirect("/Users/Erro");


            }
        } 
        [HttpPost]
        public ActionResult Edit(tblWeb tblweb, int id, FormCollection collection, int[] MutilMenu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblweb).State = EntityState.Modified;
                db.SaveChanges();
                var listwebs = db.tblConnectWebs.Where(p => p.idWeb == id).ToList();
                for (int i = 0; i < listwebs.Count; i++)
                {
                    db.tblConnectWebs.Remove(listwebs[i]);
                    db.SaveChanges();
                }
                if (MutilMenu != null)
                {
                    foreach (var idCates in MutilMenu)
                    {
                        tblConnectWeb tblconnectweb = new tblConnectWeb();
                        tblconnectweb.idCate = idCates;
                        tblconnectweb.idWeb = id;
                        db.tblConnectWebs.Add(tblconnectweb);
                        db.SaveChanges();
                    }
                }
                #region[Updatehistory]
                Updatehistoty.UpdateHistory("Edit web", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                #endregion
                if (collection["btnSave"] != null)
                {
                    Session["Thongbao"] = "<div  class=\"alert alert-info alert1\">Bạn đã sửa  thành công !<button class=\"close\" data-dismiss=\"alert\">×</button></div>";

                    return Redirect("/Web/Index");
                }
                if (collection["btnSaveCreate"] != null)
                {
                    Session["Thongbao"] = "<div  class=\"alert alert-info\">Bạn đã thêm thành công, mời bạn thêm mới !<button class=\"close\" data-dismiss=\"alert\">×</button></div>";
                    return Redirect("/Web/Create");
                }
            }
            return View(tblweb);
        }
    }
}
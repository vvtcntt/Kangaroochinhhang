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
namespace Kangaroochinhhang.Controllers.Admin.Criteria
{
    public class CriteriaController : Controller
    {
        private KangaroochinhhangContext db = new KangaroochinhhangContext();

        // GET: Criteria
        List<SelectListItem> carlist = new List<SelectListItem>();

        public ActionResult Index(string idCate, FormCollection collection)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            if (ClsCheckRole.CheckQuyen(6, 0, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
            {
                var ListCriteria = db.tblCriterias.OrderBy(m => m.Ord).ToList();
                var listpage = new List<SelectListItem>();
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
                                if (ClsCheckRole.CheckQuyen(6, 3, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
                                {
                                    int id = Convert.ToInt32(key.Remove(0, 4));
                                    tblCriteria tblcriteria = db.tblCriterias.Find(id);
                                    db.tblCriterias.Remove(tblcriteria);
                                    db.SaveChanges();
                                    var listGroupcri = db.tblGroupCriterias.Where(p => p.idCri == id).ToList();
                                    for(int i=0;i<listGroupcri.Count;i++)
                                    {
                                        db.tblGroupCriterias.Remove(listGroupcri[i]);
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
        public PartialViewResult partialCriteria(int? page, string text, string idCate, string pageSizes)
        {
            var ListCriteria = db.tblCriterias.OrderByDescending(p => p.Ord).ToList();

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
                ViewBag.chuoicout = "<span style='color: #A52A2A;'>" + pageSize + "</span> / <span style='color: #333;'>" + ListCriteria.Count.ToString() + "</span>";
                return PartialView(ListCriteria.ToPagedList(pageNumber, pageSize));

            }
            if (idCate != "" && idCate != null)
            {
                int idmenu = int.Parse(idCate);
                 List<int> mang = new List<int>();
                 var listCri = db.tblGroupCriterias.Where(p => p.idCate == idmenu).ToList();
                for (int i = 0; i < listCri.Count; i++)
                {
                    mang.Add(int.Parse(listCri[i].idCri.ToString()));
                }
                ListCriteria = db.tblCriterias.Where(p => mang.Contains(p.id)).OrderByDescending(m => m.Ord).ToList();
                 ViewBag.Idcha = idCate;
                return PartialView(ListCriteria.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                ViewBag.Idcha = 0;
            }

            return PartialView(ListCriteria.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult UpdateCriteria(string id, string Ord, string cbIsActive, string Priority,string Style)
        {
            if (ClsCheckRole.CheckQuyen(6, 2, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
            {
                int ids = int.Parse(id);
                var tblcriteria = db.tblCriterias.Find(ids);
                tblcriteria.Ord = int.Parse(Ord);
                tblcriteria.Active = bool.Parse(cbIsActive);
                tblcriteria.Priority = bool.Parse(Priority);
                tblcriteria.Style=bool.Parse(Style);
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
        public ActionResult DeleteCriteria(int id)
        {
            if (ClsCheckRole.CheckQuyen(6, 3, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
            {
                tblCriteria tblcriteria = db.tblCriterias.Find(id);
                var result = string.Empty;
                db.tblCriterias.Remove(tblcriteria);
                db.SaveChanges();
                var listGroupcri = db.tblGroupCriterias.Where(p => p.idCri == id).ToList();
                for (int i = 0; i < listGroupcri.Count; i++)
                {
                    db.tblGroupCriterias.Remove(listGroupcri[i]);
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
            if (ClsCheckRole.CheckQuyen(6, 1, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
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
                var pro = db.tblCriterias.OrderByDescending(p => p.Ord).Take(1).ToList();
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
        public ActionResult Create(tblCriteria tblcriteria, FormCollection collection, int[] MutilMenu)
        {

            db.tblCriterias.Add(tblcriteria);
            db.SaveChanges();
            var listcri = db.tblCriterias.OrderByDescending(p => p.id).Take(1).ToList();
            int idCri = int.Parse(listcri[0].id.ToString());
            if (MutilMenu != null)
            {
                foreach (var idCate in MutilMenu)
                {
                    tblGroupCriteria tblgroupcrieria = new tblGroupCriteria();
                    tblgroupcrieria.idCate = idCate;
                    tblgroupcrieria.idCri = idCri;
                    db.tblGroupCriterias.Add(tblgroupcrieria);
                    db.SaveChanges();

                }
            }
            Updatehistoty.UpdateHistory("Add tblcriteria", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            if (collection["btnSave"] != null)
            {
                Session["Thongbao"] = "<div  class=\"alert alert-info alert1\">Bạn đã thêm thành công !<button class=\"close\" data-dismiss=\"alert\">×</button></div>";
                return Redirect("/Criteria/Index");
            }
            if (collection["btnSaveCreate"] != null)
            {
                Session["Thongbao"] = "<div  class=\"alert alert-info\">Bạn đã thêm thành công, mời bạn thêm tiêu trí  mới !<button class=\"close\" data-dismiss=\"alert\">×</button></div>";
                return Redirect("/Criteria/Create");
            }
            return Redirect("Index");
          

        }
        public ActionResult Edit(int id = 0)
        {


            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            if (ClsCheckRole.CheckQuyen(6, 2, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
            {
                tblCriteria tblcriteria = db.tblCriterias.Find(id);

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

                var listcri = db.tblGroupCriterias.Where(p => p.idCri == id).ToList();
                 List<int> mang = new List<int>();
                for (int i = 0; i < listcri.Count;i++ )
                { 

                    mang.Add(int.Parse(listcri[i].idCate.ToString()));
                    
                }
                ViewBag.MutilMenu = new MultiSelectList(carlist, "Value", "Text", mang);
                if (tblcriteria == null)
                {
                    return HttpNotFound();
                }
                return View(tblcriteria);
            }
            else
            {
                return Redirect("/Users/Erro");


            }
        }

        //
        // POST: /Users/Edit/5

        [HttpPost]
        public ActionResult Edit(tblCriteria tblcriteria, int id, FormCollection collection, int[] MutilMenu)
        {
            if (ModelState.IsValid)
            {
                 db.Entry(tblcriteria).State = EntityState.Modified; 
                db.SaveChanges();
                var listcri = db.tblGroupCriterias.Where(p => p.idCri == id).ToList();
                for (int i = 0; i < listcri.Count;i++ )
                {
                    db.tblGroupCriterias.Remove(listcri[i]);
                    db.SaveChanges();
                }
                    if (MutilMenu != null)
                    {
                        foreach (var idCates in MutilMenu)
                        {
                            tblGroupCriteria tblgroupcrieria = new tblGroupCriteria();
                            tblgroupcrieria.idCate = idCates;
                            tblgroupcrieria.idCri = id;
                            db.tblGroupCriterias.Add(tblgroupcrieria);
                            db.SaveChanges();
                        }
                    }
                #region[Updatehistory]
                Updatehistoty.UpdateHistory("Edit tblcriteria", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                #endregion
                if (collection["btnSave"] != null)
                {
                    Session["Thongbao"] = "<div  class=\"alert alert-info alert1\">Bạn đã sửa  thành công !<button class=\"close\" data-dismiss=\"alert\">×</button></div>";

                    return Redirect("/Criteria/Index");
                }
                if (collection["btnSaveCreate"] != null)
                {
                    Session["Thongbao"] = "<div  class=\"alert alert-info\">Bạn đã thêm thành công, mời bạn thêm mới !<button class=\"close\" data-dismiss=\"alert\">×</button></div>";
                    return Redirect("/Criteria/Create");
                }
            }
            return View(tblcriteria);
        }
    }
}
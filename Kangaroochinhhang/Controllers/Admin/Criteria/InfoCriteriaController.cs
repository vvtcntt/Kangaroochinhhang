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
    public class InfoCriteriaController : Controller
    {
        private KangaroochinhhangContext db = new KangaroochinhhangContext();
        List<SelectListItem> carlists = new List<SelectListItem>();
        // GET: InfoCriteria
        public ActionResult Index(string id, FormCollection collection)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            if (ClsCheckRole.CheckQuyen(6, 0, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
            {
                var ListCriteria = db.tblInfoCriterias.OrderBy(m => m.Ord).ToList();
                var menuModel = db.tblGroupProducts.Where(m => m.ParentID == null).OrderBy(m => m.id).ToList();
                carlists.Clear();
                string strReturn = "---";
                foreach (var item in menuModel)
                {
                    carlists.Add(new SelectListItem { Text = item.Name, Value = item.id.ToString() });
                    StringClass.DropDownListFor(item.id, carlists, strReturn);
                    strReturn = "---";
                }
                if (id != "")
                {

                    ViewBag.drMenu = carlists;
                    ViewBag.idCate = id;
                    ViewBag.idMenu = id;
                }
                else
                {
                    ViewBag.drMenu = carlists;
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
                                    int ids = Convert.ToInt32(key.Remove(0, 4));
                                    tblInfoCriteria tblcriteria = db.tblInfoCriterias.Find(ids);
                                    db.tblInfoCriterias.Remove(tblcriteria);
                                    db.SaveChanges();
                                     
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
        public PartialViewResult partialCriteria(int? page, string text, string id, string idCate, string pageSizes)
        {
            var ListCriteria = db.tblInfoCriterias.OrderBy(p => p.Ord).ToList();

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
            if (id != "" && id != null)
            {
                int idmenu = int.Parse(id);

                ListCriteria = db.tblInfoCriterias.Where(p => p.idCri == idmenu).OrderBy(m => m.Ord).ToList();
                if(idCate!=null && idCate!="")
                {
                    List<string> Mang = new List<string>();
                    int idCates=int.Parse(idCate);
                    var ListConnect = db.tblGroupCriterias.Where(p => p.idCate == idCates).ToList();
                    for(int i=0;i<ListConnect.Count;i++)
                    {
                        Mang.Add(ListConnect[i].idCri.ToString());
                    }
                    ListCriteria = db.tblInfoCriterias.Where(p => Mang.Contains(p.idCri.ToString())).OrderBy(m => m.Ord).ToList();
                }
                ViewBag.Idcha = id;
                return PartialView(ListCriteria.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                ViewBag.Idcha = 0;
            }

            return PartialView(ListCriteria.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult UpdateCriteria(string id, string Ord, string cbIsActive)
        {
            if (ClsCheckRole.CheckQuyen(6, 2, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
            {
                int ids = int.Parse(id);
                var tblcriteria = db.tblInfoCriterias.Find(ids);
                tblcriteria.Active = bool.Parse(cbIsActive);
                tblcriteria.Ord = int.Parse(Ord);
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
                tblInfoCriteria tblcriteria = db.tblInfoCriterias.Find(id);
                var result = string.Empty;
                db.tblInfoCriterias.Remove(tblcriteria);
                db.SaveChanges();
                 
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
        public ActionResult AutoOrd(string idCate)
        {

            int id = int.Parse(idCate);
            var ListOrd = db.tblInfoCriterias.Where(p => p.idCri == id).OrderByDescending(p => p.Ord).Take(1).ToList();
            var result = string.Empty;
            if (ListOrd.Count > 0)
            {
                int stt = int.Parse(ListOrd[0].Ord.ToString()) + 1;
                result = stt.ToString();
            }
            else
            {
                result = "0";

            }
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
            if (ClsCheckRole.CheckQuyen(6, 1, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
            {
                 var menuModel = db.tblCriterias.OrderBy(m => m.id).ToList();
                List<SelectListItem> carlist = new List<SelectListItem>(); 
                foreach (var item in menuModel)
                {
                    carlist.Add(new SelectListItem { Text = item.Name, Value = item.id.ToString() });
                }
                if (id != null && id != "")
                    ViewBag.drMenu = new SelectList(carlist, "Value", "Text", id);
                else
                    ViewBag.drMenu = carlist;
                var pro = db.tblInfoCriterias.OrderByDescending(p => p.Ord).Take(1).ToList();
                if (id != null && id != "")
                {
                    int idcre = int.Parse(id);
                      pro = db.tblInfoCriterias.Where(p => p.idCri == idcre).OrderByDescending(p => p.Ord).Take(1).ToList();
                }
                
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
        public ActionResult Create(tblInfoCriteria tblcriteria, FormCollection collection)
        {
            int idcate = int.Parse(collection["drMenu"]);
            tblcriteria.idCri = idcate;
            db.tblInfoCriterias.Add(tblcriteria);
            db.SaveChanges();
            
             
            Updatehistoty.UpdateHistory("Add tblcriteria", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            if (collection["btnSave"] != null)
            {
                Session["Thongbao"] = "<div  class=\"alert alert-info alert1\">Bạn đã thêm thành công !<button class=\"close\" data-dismiss=\"alert\">×</button></div>";
                return Redirect("/InfoCriteria/Index?id="+idcate+"");
            }
            if (collection["btnSaveCreate"] != null)
            {
                Session["Thongbao"] = "<div  class=\"alert alert-info\">Bạn đã thêm thành công, mời bạn thêm tiêu trí  mới !<button class=\"close\" data-dismiss=\"alert\">×</button></div>";
                return Redirect("/InfoCriteria/Create?id="+idcate+"");
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
                tblInfoCriteria tblcriteria = db.tblInfoCriterias.Find(id);

                var menuModel = db.tblCriterias.OrderBy(m => m.id).ToList();
                List<SelectListItem> carlist = new List<SelectListItem>();
                foreach (var item in menuModel)
                {
                    carlist.Add(new SelectListItem { Text = item.Name, Value = item.id.ToString() });
                }


                ViewBag.drMenu = new SelectList(carlist, "Value", "Text", tblcriteria.idCri);
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
        public ActionResult Edit(tblInfoCriteria tblcriteria, int id, FormCollection collection )
        {
            if (ModelState.IsValid)
            {
                int idcate = int.Parse(collection["drMenu"]);
                tblcriteria.idCri = idcate;
                db.Entry(tblcriteria).State = EntityState.Modified;
                db.SaveChanges();
                
                #region[Updatehistory]
                Updatehistoty.UpdateHistory("Edit tblcriteria", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                #endregion
                if (collection["btnSave"] != null)
                {
                    Session["Thongbao"] = "<div  class=\"alert alert-info alert1\">Bạn đã sửa  thành công !<button class=\"close\" data-dismiss=\"alert\">×</button></div>";

                    return Redirect("/InfoCriteria/Index?id="+idcate+"");
                }
                if (collection["btnSaveCreate"] != null)
                {
                    Session["Thongbao"] = "<div  class=\"alert alert-info\">Bạn đã thêm thành công, mời bạn thêm mới !<button class=\"close\" data-dismiss=\"alert\">×</button></div>";
                    return Redirect("/InfoCriteria/Create?id=" + idcate + "");
                }
            }
            return View(tblcriteria);
        }

    }
}
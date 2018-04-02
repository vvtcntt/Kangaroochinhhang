using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kangaroochinhhang.Models;
using PagedList;
using PagedList.Mvc;
using System.Globalization;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using Kangaroochinhhang.models;

namespace Kangaroochinhhang.Controllers.Admin.Address
{
    public class addressController : Controller
    {
        // GET: address
        private KangaroochinhhangContext db = new KangaroochinhhangContext();
        public ActionResult Index(int? page, string id, FormCollection collection)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            if (ClsCheckRole.CheckQuyen(10, 0, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
            {
                var listAddress = db.TblAddress.ToList();

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
                                if (ClsCheckRole.CheckQuyen(10, 3, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
                                {
                                    int ids = Convert.ToInt32(key.Remove(0, 4));
                                    TblAddress TblAddress = db.TblAddress.Find(ids);
                                    db.TblAddress.Remove(TblAddress);
                                    db.SaveChanges();
                                    return RedirectToAction("Index");
                                }
                                else
                                {
                                    return Redirect("/Users/Erro");

                                }
                            }
                        }
                    }
                }
                return View(listAddress.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                return Redirect("/Users/Erro");

            }
        }
        public ActionResult UpdateAddress(string id, string Active, string Ord)
        {
            if (ClsCheckRole.CheckQuyen(10, 2, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
            {

                int ids = int.Parse(id);
                var TblAddress = db.TblAddress.Find(ids);
                TblAddress.Active = bool.Parse(Active);
                TblAddress.Ord = int.Parse(Ord);
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
        public ActionResult DeleteAddress(int id)
        {
            if (ClsCheckRole.CheckQuyen(10, 3, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
            {
                TblAddress TblAddress = db.TblAddress.Find(id);
                var result = string.Empty;
                db.TblAddress.Remove(TblAddress);
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
        public ActionResult Create()
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
            if (ClsCheckRole.CheckQuyen(10, 1, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
            {
                var pro = db.TblAddress.OrderByDescending(p => p.Ord).ToList();
                if (pro.Count > 0)
                    ViewBag.Ord = pro[0].Ord + 1;
                else
                { ViewBag.Ord = "0"; }
                var Manufacture = db.SelectListItem.Where(m => m.Active == true).OrderBy(m => m.Ord).ToList();
                var lstmanu = new List<System.Web.Mvc.SelectListItem>();

                foreach (var item in Manufacture)
                {
                    lstmanu.Add(new SelectListItem { Text = item.Name, Value = item.id.ToString() });
                }
                ViewBag.mutilManu = new SelectList(lstmanu, "Value", "Text");



                var groupAddress = db.TblGroupAddress.Where(m => m.Active == true).OrderBy(m => m.Ord).ToList();
                var listGroup = new List<SelectListItem>();

                foreach (var item in groupAddress)
                {
                    listGroup.Add(new SelectListItem { Text = item.Name, Value = item.id.ToString() });
                }
                 ViewBag.drAddress = new SelectList(listGroup, "Value", "Text");
                return View();
            }
            else
            {
                return Redirect("/Users/Erro");
            }
        }

        [HttpPost]
        public ActionResult Create(TblAddress TblAddress, FormCollection collection, int[] mutilManu)
        {
            string drAddress = collection["drAddress"];

            if (drAddress == "" && drAddress!=null)
            {
                TblAddress.idCate = 0;
            }
            else
            {

                TblAddress.idCate = int.Parse(drAddress);
            }
            db.TblAddress.Add(TblAddress);
            db.SaveChanges();
            var ListManu = db.TblAddress.OrderByDescending(p => p.id).Take(1).ToList();
            int id = int.Parse(ListManu[0].id.ToString());
            if (mutilManu != null)
            {
                foreach (var idMenu in mutilManu)
                {
                    tblConnectManuToAddress connect = new tblConnectManuToAddress();
                    connect.idManu = idMenu;
                    connect.idAdress = id;
                    db.tblConnectManuToAddresses.Add(connect);
                    db.SaveChanges();
                }
            }
            #region[Updatehistory]
            Updatehistoty.UpdateHistory("Add TblAddress", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
            #endregion
            if (collection["btnSave"] != null)
            {
                Session["Thongbao"] = "<div  class=\"alert alert-info alert1\">Bạn đã thêm thành công !<button class=\"close\" data-dismiss=\"alert\">×</button></div>";

                return Redirect("/address/Index");
            }
            if (collection["btnSaveCreate"] != null)
            {
                Session["Thongbao"] = "<div  class=\"alert alert-info\">Bạn đã thêm thành công, mời bạn thêm mới !<button class=\"close\" data-dismiss=\"alert\">×</button></div>";
                return Redirect("/address/Create");
            }
            return Redirect("Index");


        }
        public ActionResult Edit(int id = 0)
        {

            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            if (ClsCheckRole.CheckQuyen(10, 2, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
            {
                TblAddress Hotline = db.TblAddress.Find(id);
                if (Hotline == null)
                {
                    return HttpNotFound();
                }
                var Manufacture = db.SelectListItem.Where(m => m.Active == true).OrderBy(m => m.Ord).ToList();
                var listIdManu = db.tblConnectManuToAddresses.Where(p => p.idAdress == id).Select(p => p.idManu).ToList();
                var lstmanu = new List<SelectListItem>();

                foreach (var item in Manufacture)
                {
                    lstmanu.Add(new SelectListItem { Text = item.Name, Value = item.id.ToString() });
                }
                ViewBag.mutilManu = new MultiSelectList(lstmanu, "Value", "Text", listIdManu);

                var groupAddress = db.tblGroupAddresses.Where(m => m.Active == true).OrderBy(m => m.Ord).ToList();
                var listGroup = new List<SelectListItem>();

                foreach (var item in groupAddress)
                {
                    listGroup.Add(new SelectListItem { Text = item.Name, Value = item.id.ToString() });
                }
                string idCate = Hotline.idCate.ToString();
                if(idCate!=null && idCate!="")
                ViewBag.drAddress = new SelectList(listGroup, "Value", "Text", int.Parse(idCate));
                else
                    ViewBag.drAddress = new SelectList(listGroup, "Value", "Text");
                return View(Hotline);
            }
            else
            {
                return Redirect("/Users/Erro");


            }
        }

        //
        // POST: /Users/Edit/5

        [HttpPost]
        public ActionResult Edit(TblAddress TblAddress, int id, FormCollection collection,int[] mutilManu)
        {
            if (ModelState.IsValid)
            {
                string drAddress = collection["drAddress"];

                if (drAddress == "" && drAddress != null)
                {
                    TblAddress.idCate = 0;
                }
                else
                {

                    TblAddress.idCate = int.Parse(drAddress);
                }
                db.Entry(TblAddress).State = EntityState.Modified;
                db.SaveChanges();
                var listIdManu = db.tblConnectManuToAddresses.Where(p => p.idAdress == id).ToList();
                for (int i = 0; i < listIdManu.Count; i++)
                {
                    db.tblConnectManuToAddresses.Remove(listIdManu[i]);
                    db.SaveChanges();
                }
                if (mutilManu != null)
                {
                    foreach (var idMenu in mutilManu)
                    {
                        tblConnectManuToAddress connectimage = new tblConnectManuToAddress();
                        connectimage.idManu = idMenu;
                        connectimage.idAdress = id;
                        db.tblConnectManuToAddresses.Add(connectimage);
                        db.SaveChanges();
                    }
                }
                #region[Updatehistory]
                Updatehistoty.UpdateHistory("Edit Hotline", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                #endregion
                if (collection["btnSave"] != null)
                {
                    Session["Thongbao"] = "<div  class=\"alert alert-info alert1\">Bạn đã sửa  thành công !<button class=\"close\" data-dismiss=\"alert\">×</button></div>";

                    return Redirect("/address/Index");
                }
                if (collection["btnSaveCreate"] != null)
                {
                    Session["Thongbao"] = "<div  class=\"alert alert-info\">Bạn đã thêm thành công, mời bạn thêm mới !<button class=\"close\" data-dismiss=\"alert\">×</button></div>";
                    return Redirect("/address/Create");
                }
            }
            return View(TblAddress);
        }
    }
}
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
namespace Kangaroochinhhang.Controllers.Admin.User
{
    public class UsersController : Controller
    {
        // GET: Users
        private KangaroochinhhangContext db = new KangaroochinhhangContext();
        public ActionResult Index(int? page, string id, FormCollection collection)
        {

            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            if (ClsCheckRole.CheckQuyen(2, 0, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
            {
                var ListUser = db.tblUsers.ToList();
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
                                if (ClsCheckRole.CheckQuyen(2, 3, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
                                {
                                    int ids = Convert.ToInt32(key.Remove(0, 4));
                                    tblUser tbluser = db.tblUsers.Find(ids);
                                    db.tblUsers.Remove(tbluser);
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
                return View(ListUser.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                return Redirect("/Users/Erro");

            }
        }
        public ActionResult UpdateUsers(string id, string Active)
        {
            if (ClsCheckRole.CheckQuyen(2, 2, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
            {
                int ids = int.Parse(id);
                var tbluser = db.tblUsers.Find(ids);
                tbluser.Active = bool.Parse(Active);
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
        public ActionResult DeleteUsers(int id)
        {
            if (ClsCheckRole.CheckQuyen(2, 3, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
            {
                tblUser tblusers = db.tblUsers.Find(id);
                var result = string.Empty;
                db.tblUsers.Remove(tblusers);
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
            if (ClsCheckRole.CheckQuyen(2, 1, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
            {
                return View();
            }
            else
            {
                return Redirect("/Users/Erro");

            }
        }

        [HttpPost]
        public ActionResult Create(tblUser tbluser, FormCollection collection)
        {
            string username = tbluser.UserName;
            var kiemtra = db.tblUsers.Where(p => p.UserName == username).ToList();
            if (kiemtra.Count > 0)
            {
                ViewBag.thongbao = "<div  class=\"alert alert-info\">Tài khoản bạn đăng đã tồn tại, vui lòng nhập tên tài khoản khác !<button class=\"close\" data-dismiss=\"alert\">×</button></div>";

            }
            else
            {
                tbluser.Password = EncryptandDecrypt.Encrypt(tbluser.Password);
                tbluser.DateCreate = DateTime.Now;
                tbluser.Gender = int.Parse(collection["ddlGender"]);
                string idUser = Request.Cookies["Username"].Values["UserID"];
                tbluser.idUser = int.Parse(idUser);
                db.tblUsers.Add(tbluser);
                db.SaveChanges();
                #region[Updatehistory]
                Updatehistoty.UpdateHistory("Add User", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                #endregion
                if (collection["btnSave"] != null)
                {
                    Session["Thongbao"] = "<div  class=\"alert alert-info alert1\">Bạn đã thêm tài khoản thành công !<button class=\"close\" data-dismiss=\"alert\">×</button></div>";

                    return Redirect("/Users/Index");
                }
                if (collection["btnSaveCreate"] != null)
                {
                    Session["Thongbao"] = "<div  class=\"alert alert-info\">Bạn đã thêm tài khoản thành công, mời bạn thêm tài khoản mới !<button class=\"close\" data-dismiss=\"alert\">×</button></div>";
                    return Redirect("/Users/Create");
                }
                return Redirect("Index");
            }
            return View();

        }
        public ActionResult Edit(int id = 0)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            if (ClsCheckRole.CheckQuyen(2, 2, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
            {
                tblUser tbluser = db.tblUsers.Find(id);
                if (tbluser == null)
                {
                    return HttpNotFound();
                }
                return View(tbluser);
            }
            else
            {
                return Redirect("/Users/Erro");


            }
        }

        //
        // POST: /Users/Edit/5

        [HttpPost]
        public ActionResult Edit(tblUser tbluser, int id, FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbluser).State = EntityState.Modified;
                var user = db.tblUsers.First(p => p.id == id);
                string pass = tbluser.Password;
                if (pass == user.Password)
                {
                    tbluser.Password = EncryptandDecrypt.Encrypt(tbluser.Password);
                }
                else
                {

                    tbluser.Password = EncryptandDecrypt.Encrypt(tbluser.Password);
                }
                tbluser.UserName = user.UserName;
                tbluser.DateCreate = DateTime.Now;
                string idUser = Request.Cookies["Username"].Values["UserID"];
                tbluser.idUser = int.Parse(idUser);
                db.SaveChanges();
                #region[Updatehistory]
                Updatehistoty.UpdateHistory("Edit User", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                #endregion
                if (collection["btnSave"] != null)
                {
                    Session["Thongbao"] = "<div  class=\"alert alert-info alert1\">Bạn đã sửa tài khoản thành công !<button class=\"close\" data-dismiss=\"alert\">×</button></div>";

                    return Redirect("/Users/Index");
                }
                if (collection["btnSaveCreate"] != null)
                {
                    Session["Thongbao"] = "<div  class=\"alert alert-info\">Bạn đã thêm tài khoản thành công, mời bạn thêm tài khoản mới !<button class=\"close\" data-dismiss=\"alert\">×</button></div>";
                    return Redirect("/Users/Create");
                }
            }
            return View(tbluser);
        }
        public ActionResult Role(string id)
        {
            if (Session["Thongbao"] != null && Session["Thongbao"] != "")
            {

                ViewBag.thongbao = Session["Thongbao"].ToString();
                Session["Thongbao"] = "";
            }
            if (ClsCheckRole.CheckQuyen(3, 2, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
            {
                int idUser = int.Parse(id);
                ViewBag.Users = " <span style=\"margin:5px 0px; display:block; color:#3e3e3e\">Bạn đang phân quyền cho tài khoản : <span style=\"font-weight:bold\">" + db.tblUsers.Find(idUser).UserName + "</span></span>";

                var ListModule = db.tblModules.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
                string chuoi = "";
                for (int i = 0; i < ListModule.Count; i++)
                {
                    chuoi += " <script language=\"JavaScript\" type=\"text/javascript\">";
                    chuoi += "function toggle_" + ListModule[i].id + "(source) {";
                    chuoi += "checkboxes = document.getElementsByClassName('checkitem+" + ListModule[i].id + "');";
                    chuoi += " for (var i = 0, n = checkboxes.length; i < n; i++) {";
                    chuoi += "checkboxes[i].checked = source.checked;";
                    chuoi += " }";
                    chuoi += " }";
                    chuoi += "</script>";
                    chuoi += "<tr class=\"row0\"> ";
                    chuoi += " <td> <a title=\"" + ListModule[i].Name + "\" href=\"\">" + ListModule[i].Name + "</a> </td>";
                    chuoi += " <td style=\"width:85px;\" class=\"Active text-center\"> <input name=\"chkitem+" + ListModule[i].id + "\" type=\"checkbox\" value=\"\"   onclick=\"toggle_" + ListModule[i].id + "(this)\" class=\"\"></td>";

                    int idModule = int.Parse(ListModule[i].id.ToString());
                    for (int j = 0; j < 4; j++)
                    {
                        var ListRight = db.tblRights.Where(p => p.idModule == idModule && p.Role == j && p.idUser == idUser).ToList();

                        if (ListRight.Count > 0)
                        {

                            for (int k = 0; k < ListRight.Count; k++)
                            {
                                if (ListRight[k].Role == j)
                                {
                                    chuoi += " <td class=\"Active text-center \" align=\"center\" style=\"width:45px;\"><input type=\"checkbox\" checked=\"checked\" id=\"Active\" class=\"checkitem+" + ListModule[i].id + "\" name=\"chk_" + ListModule[i].id + "-" + j + "-" + idUser + "\" value=\"" + j + "\" /> </td>  ";
                                }
                                else
                                {
                                    chuoi += " <td class=\"Active text-center  \" align=\"center\" style=\"width:45px;\"><input type=\"checkbox\" class=\"checkitem+" + ListModule[i].id + "\" id=\"Active\" name=\"chk_" + ListModule[i].id + "-" + j + "-" + idUser + "\" value=\"" + j + "\" /> </td>  ";
                                }
                            }
                        }
                        else
                        {
                            chuoi += " <td class=\"Active text-center  \" align=\"center\" style=\"width:45px;\"><input type=\"checkbox\"  class=\"checkitem+" + ListModule[i].id + "\" id=\"Active\" name=\"chk_" + ListModule[i].id + "-" + j + "-" + idUser + "\" value=\"" + j + "\" /> </td>  ";
                        }

                    }
                    chuoi += "</tr>";
                }
                ViewBag.Role = chuoi;

                return View();
            }
            else
            {
                return Redirect("/Users/Erro");

            }
        }
        [HttpPost]
        public ActionResult Role(string idUsers, string id, FormCollection collection)
        {

            int ids = int.Parse(collection["txtidUser"]);
            var Right = db.tblRights.Where(p => p.idUser == ids).ToList();
            for (int i = 0; i < Right.Count; i++)
            {
                int iddel = int.Parse(Right[i].id.ToString());
                db.tblRights.Remove(db.tblRights.Find(iddel));
                db.SaveChanges();
            }
            foreach (string key in Request.Form.Keys)
            {
                var checkbox = "";
                if (key.StartsWith("chk_"))
                {
                    checkbox = Request.Form["" + key];
                    if (checkbox != "false")
                    {
                        string Checkkey = key.Remove(0, 4);
                        string[] listkey = Checkkey.Split('-');
                        int idModule = int.Parse(listkey[0]);
                        int Role = int.Parse(listkey[1]);
                        int idUser = int.Parse(listkey[2]);

                        tblRight tblright = new tblRight();
                        tblright.idModule = idModule;
                        tblright.Role = Role;
                        tblright.idUser = idUser;
                        db.tblRights.Add(tblright);
                        db.SaveChanges();

                    }

                }
            }
            Session["id"] = "";
            Session["Thongbao"] = "<div  class=\"alert alert-info\">Bạn đã cấp quyền thành công !<button class=\"close\" data-dismiss=\"alert\">×</button></div>";

            return Redirect("/Users/Role?id=" + ids + "");
        }
        public ActionResult Erro()
        {
            return View();
        }
    }
}
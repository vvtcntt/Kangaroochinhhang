using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kangaroochinhhang.Models;
using System.Globalization;
namespace Kangaroochinhhang.Controllers.Admin
{
    public class LoginController : Controller
    {
        // GET: Login
        private KangaroochinhhangContext db = new KangaroochinhhangContext();
        public ActionResult LoginIndex()
        {
            if (Request.Cookies["Username"] != null)
            {
                return RedirectToAction("Index", "Productad");
            }


            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult LoginIndex(tblUser tblusers, tblHistoryLogin tblhistorylogin)
        {


            string pass = EncryptandDecrypt.Encrypt(tblusers.Password);
            var list = db.tblUsers.Where(p => p.UserName == tblusers.UserName && p.Password == pass && p.Active == true).ToList();
            if (list.Count > 0)
            {
                
                var userCookie = new HttpCookie("Username");

                var id = list[0].id.ToString(CultureInfo.InvariantCulture);
                var username = list[0].UserName;
                var fullname = list[0].FullName;
                userCookie.Values["UserID"] = id;
                userCookie.Values["FullName"] = Server.HtmlEncode(fullname.Trim());
                userCookie.Values["fullname"] = fullname;
                userCookie.Values["UserName"] = Server.UrlEncode(username.Trim());
                userCookie.Values["username"] = username;
                tblhistorylogin.FullName = fullname;
                tblhistorylogin.Task = "Login hệ thống";
                tblhistorylogin.idUser = int.Parse(id);
                tblhistorylogin.DateCreate = DateTime.Now;
                tblhistorylogin.Active = true;
                db.tblHistoryLogins.Add(tblhistorylogin);
                db.SaveChanges();

                userCookie.Expires = DateTime.Now.AddHours(22);
                Response.Cookies.Add(userCookie); Session["Count"] = "";
                return Redirect("/Productad/Index");

            }
            else
            {


                ViewBag.Note = "Tài khoản của bạn nhập không đúng, kiểm tra lại tên đăng nhập hoặc mật khẩu. Có thể tài khoản bị khóa  !";
                return View();
            }
        }

        public ActionResult Logout()
        {

            HttpCookie usercookie = new HttpCookie("Username");
            usercookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(usercookie);
            return RedirectToAction("LoginIndex");
        }

    }
}
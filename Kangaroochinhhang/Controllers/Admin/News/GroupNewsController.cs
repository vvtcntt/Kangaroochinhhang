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
namespace Kangaroochinhhang.Controllers.Admin.News
{
    public class GroupNewsController : Controller
    {
        List<SelectListItem> carlist = new List<SelectListItem>();
        //public void DropDownListNews(int cateid)
        //{
        //    KangaroochinhhangContext db = new KangaroochinhhangContext();
        //    var cars = db.tblGroupNews.Where(p => p.ParentID == cateid).ToList();
        //    foreach (var item in cars)
        //    {
        //        carlist.Add(new SelectListItem { Text = StringClass.ShowNameLevel(int.Parse(item.Level.ToString())) + " " + item.Name, Value = item.id.ToString() });
        //        DropDownListNews(item.id);
        //    }
        //}
        // GET: GroupNews
        private KangaroochinhhangContext db = new KangaroochinhhangContext();
        public ActionResult Index(string idCate, FormCollection collection)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            if (ClsCheckRole.CheckQuyen(5, 0, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
            {
                var menuModel = db.tblGroupNews.Where(m => m.ParentID == null).OrderBy(m => m.id).ToList();
                carlist.Clear();
                string strReturn = "---";
                foreach (var item in menuModel)
                {
                    carlist.Add(new SelectListItem { Text = item.Name, Value = item.id.ToString() }); 
                    StringClass.DropDownListNews(item.id, carlist, strReturn);
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
                                if (ClsCheckRole.CheckQuyen(5, 3, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
                                {
                                    int id = Convert.ToInt32(key.Remove(0, 4));
                                    tblGroupNew tblgroupnews = db.tblGroupNews.Find(id);
                                    db.tblGroupNews.Remove(tblgroupnews);
                                    db.SaveChanges();
                                    var listnews = db.tblNews.Where(p => p.idCate == id).ToList();
                                    for(int i=0;i<listnews.Count;i++)
                                    {
                                        db.tblNews.Remove(listnews[i]);
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
        public PartialViewResult PartialGroupNews(int? page, string text, string idCate, string pageSizes)
        {
            var ListNews = db.tblGroupNews.Where(p => p.ParentID==null && p.Active==true).OrderBy(p => p.Ord).ToList();

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
                ViewBag.chuoicout = "<span style='color: #A52A2A;'>" + pageSize + "</span> / <span style='color: #333;'>" + ListNews.Count.ToString() + "</span>";
                return PartialView(ListNews.ToPagedList(pageNumber, pageSize));

            }
            if (idCate != "" && idCate != null)
            {
                int idmenu = int.Parse(idCate);
                var menucha = db.tblGroupNews.Find(idmenu);
              
                ListNews = db.tblGroupNews.Where(p =>p.ParentID==idmenu).OrderBy(p => p.Ord).ToList();
                ViewBag.Idcha = idCate;
                return PartialView(ListNews.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                ViewBag.Idcha = 0;
            }

            return PartialView(ListNews.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult UpdateGroupNews(string id, string Active, string order, string idCate, string Priority)
        {
            if (ClsCheckRole.CheckQuyen(5, 2, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
            {
                int ids = int.Parse(id);
                var GroupNews = db.tblGroupNews.Find(ids);
                int idcate1 = GroupNews.id;
                GroupNews.Active = bool.Parse(Active);
                GroupNews.Ord = int.Parse(order);
                 if(idCate=="" || idCate==null)
                {
                    GroupNews.ParentID = int.Parse(idCate);
                    int idCates = int.Parse(idCate);
                    if (idcate1 != idCates)
                    {
                        var listord = db.tblGroupNews.Where(p => p.ParentID == idCates).OrderByDescending(p => p.id).Take(1).ToList();
                        GroupNews.Ord = int.Parse(listord[0].Ord.ToString()) + 1;

                    }
                }
                else
                {
                    GroupNews.ParentID = null;
 
                } 
                  
                db.SaveChanges();
                 var result = string.Empty;
                result = "Thành công";
                return Json(new { result = result });
            }
            else
            {
                var result = string.Empty;
                result = "Bạn không có quyền truy cập tính năng này";
                return Json(new { result = result });
            }
        }
        public ActionResult Create(string id)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            if (ClsCheckRole.CheckQuyen(5, 1, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
            {
                 var pro = db.tblGroupNews.OrderByDescending(p => p.Ord).Take(1).ToList();
                 var menuModel = db.tblGroupNews.Where(m => m.ParentID == null).OrderBy(m => m.id).ToList();
                 carlist.Clear();
                 string strReturn = "---";
                 foreach (var item in menuModel)
                 {
                     carlist.Add(new SelectListItem { Text = item.Name, Value = item.id.ToString() });
                     StringClass.DropDownListNews(item.id, carlist, strReturn);
                     strReturn = "---";
                 }
                 ViewBag.drMenu = new SelectList(carlist, "Value", "Text", id);
                ViewBag.Ord = pro[0].Ord + 1;
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

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(tblGroupNew tblgroupnews, FormCollection collection)
        {

            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            string drMenu = collection["drMenu"];
            string nLevel;

            if (drMenu == "")
            {
                tblgroupnews.ParentID = null;
             }
            else
            {

                var dbLeve = db.tblGroupNews.Find(int.Parse(drMenu));
                tblgroupnews.ParentID = dbLeve.id;
             }

            tblgroupnews.DateCreate = DateTime.Now;
            string idUser = Request.Cookies["Username"].Values["UserID"];
            tblgroupnews.idUser = int.Parse(idUser);
            tblgroupnews.Tag = StringClass.NameToTag(tblgroupnews.Name);
            db.tblGroupNews.Add(tblgroupnews);
            db.SaveChanges();
            Updatehistoty.UpdateHistory("Add Group Product", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());


            var Groups = db.tblGroupNews.Where(p => p.Active == true).OrderByDescending(p => p.id).Take(1).ToList();
            string id = Groups[0].id.ToString();

            clsSitemap.CreateSitemap(StringClass.NameToTag(tblgroupnews.Name), id, "GroupNews");
            if (collection["btnSave"] != null)
            {
                Session["Thongbao"] = "<div  class=\"alert alert-info alert1\">Bạn đã thêm danh mục thành công !<button class=\"close\" data-dismiss=\"alert\">×</button></div>";

                return Redirect("/GroupNews/Index?idCate=" + drMenu);
            }
            if (collection["btnSaveCreate"] != null)
            {
                Session["Thongbao"] = "<div  class=\"alert alert-info\">Bạn đã thêm danh mục thành công, mời bạn thêm danh mục mới !<button class=\"close\" data-dismiss=\"alert\">×</button></div>";
                return Redirect("/GroupNews/Create?id=" + drMenu + "");
            }
            return Redirect("/GroupNews/Index?idCate=" + drMenu);


        }
        public ActionResult AutoOrd(string idCate)
        {
            var result = string.Empty;

            if (idCate != "")
            {
                int id = int.Parse(idCate);
 
                var GroupNews = db.tblGroupNews.Where(p => p.ParentID==id).OrderByDescending(p => p.Ord).Take(1).ToList();

                if (GroupNews.Count > 0)
                {
                    int stt = int.Parse(GroupNews[0].Ord.ToString()) + 1;
                    result = stt.ToString();
                }
                else
                {
                    result = "0";

                }
            }
            else
            {
                var GroupNews = db.tblGroupNews.Where(p => p.ParentID == null).OrderByDescending(p => p.Ord).Take(1).ToList();

                int stt = int.Parse(GroupNews[0].Ord.ToString()) + 1;
                result = stt.ToString();
            }


            return Json(new { result = result });
        }
        public ActionResult Edit(int id)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            if (ClsCheckRole.CheckQuyen(5, 2, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
            {
                tblGroupNew tblgroupnews = db.tblGroupNews.First(p => p.id == id);
                if (tblgroupnews == null)
                {
                    return HttpNotFound();
                }
                ViewBag.id = id;
                var menuName = db.tblGroupNews.ToList();
                var pro = db.tblGroupNews.OrderByDescending(p => p.Ord).Take(1).ToList();
                var menuModel = db.tblGroupNews.Where(m => m.ParentID == null).OrderBy(m => m.id).ToList();
                carlist.Clear();
                string strReturn = "---";
                foreach (var item in menuModel)
                {
                    carlist.Add(new SelectListItem { Text = item.Name, Value = item.id.ToString() });
                    StringClass.DropDownListNews(item.id, carlist, strReturn);
                    strReturn = "---";
                }
                ViewBag.drMenu = new SelectList(carlist, "Value", "Text", id);

                return View(tblgroupnews);
            }
            else
            {
                return Redirect("/Users/Erro");

            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(tblGroupNew tblgroupnews, FormCollection collection, int id)
        {
            
                string drMenu = collection["drMenu"];
                string nLevel = "";

                if (drMenu == "")
                {
                    tblgroupnews.ParentID = null;
                     var counts = db.tblGroupNews.Where(p => p.ParentID == null).OrderByDescending(p => p.Ord).Take(1).ToList();
                 }
                else
                {
                     if (drMenu != id.ToString())
                    {
                        var dbLeve = db.tblGroupNews.Find(int.Parse(drMenu));
                        tblgroupnews.ParentID = dbLeve.id;
                     }
                     
                }
                string idUser = Request.Cookies["Username"].Values["UserID"];
                tblgroupnews.idUser = int.Parse(idUser);

                bool URL = (collection["URL"] == "false") ? false : true;
                if (URL == true)
                {
                    tblgroupnews.Tag = StringClass.NameToTag(tblgroupnews.Name);
                }
                else
                {
                    tblgroupnews.Tag = collection["NameURL"];
                }
                 clsSitemap.CreateSitemap(tblgroupnews.Tag, id.ToString(), "GroupNews");

                tblgroupnews.DateCreate = DateTime.Now;
                db.Entry(tblgroupnews).State = EntityState.Modified;
                db.SaveChanges();
                #region[Updatehistory]
                Updatehistoty.UpdateHistory("Edit tblgroupnews", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                #endregion
                if (collection["btnSave"] != null)
                {

                    if (drMenu == "")
                    {
                        Session["Thongbao"] = "<div  class=\"alert alert-info alert1\">Bạn đã sửa danh mục thành công !<button class=\"close\" data-dismiss=\"alert\">×</button></div>";

                        return Redirect("/GroupNews/Index?id=" + drMenu + "");
                    }
                    else
                    {
                        Session["Thongbao"] = "<div  class=\"alert alert-info alert1\">Bạn đã sửa danh mục thành công  !<button class=\"close\" data-dismiss=\"alert\">×</button></div>";

                        var checkgroup = db.tblGroupNews.Where(p => p.id==int.Parse(drMenu)).ToList();
                        if (checkgroup.Count > 0)
                            return Redirect("/GroupNews/Index?idCate=" + checkgroup[0].id);

                    }
                }
                if (collection["btnSaveCreate"] != null)
                {
                    Session["Thongbao"] = "<div  class=\"alert alert-info\">Bạn đã sửa danh mục thành công, mời bạn thêm danh mục  mới !<button class=\"close\" data-dismiss=\"alert\">×</button></div>";
                    return Redirect("/GroupNews/Create?id=" + drMenu + "");
                }
            
            return Redirect("/GroupNews/");
        }
        public ActionResult DeleteGroupNews(int id)
        {
            if (ClsCheckRole.CheckQuyen(5, 3, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
            {
                tblGroupNew tblgroupnews = db.tblGroupNews.Find(id);
                clsSitemap.DeteleSitemap(id.ToString(), "GroupNews");
                var result = string.Empty;
                db.tblGroupNews.Remove(tblgroupnews);
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
    }
}
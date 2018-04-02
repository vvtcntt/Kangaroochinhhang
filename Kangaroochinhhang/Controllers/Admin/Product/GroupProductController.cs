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
namespace Kangaroochinhhang.Controllers.Admin.Product
{
    public class GroupProductController : Controller
    {
        // GET: GroupProduct
        private KangaroochinhhangContext db = new KangaroochinhhangContext();
         List<SelectListItem> carlist = new List<SelectListItem>();
 
          
        public ActionResult Index(string idCate, FormCollection collection)
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            if (ClsCheckRole.CheckQuyen(4, 0, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
            {
                var menuModel = db.tblGroupProducts.Where(m => m.ParentID == null).OrderBy(m => m.id).ToList();
                carlist.Clear();
                string strReturn ="---";
                foreach (var item in menuModel)
                {
                    carlist.Add(new SelectListItem { Text = item.Name, Value = item.id.ToString() });


                    StringClass.DropDownListFor(item.id, carlist, strReturn);
                    strReturn = "---";

                 }
                //ViewBag.drMenu = carlist; 
                ViewBag.drMenu = new SelectList(carlist, "Value", "Text", 8);
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
                                if (ClsCheckRole.CheckQuyen(4, 3, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
                                {
                                    int id = Convert.ToInt32(key.Remove(0, 4));
                                    tblGroupProduct tblproduct = db.tblGroupProducts.Find(id);
                                    db.tblGroupProducts.Remove(tblproduct);
                                    db.SaveChanges();
                                    var listProduct = db.tblProducts.Where(p => p.idCate == id).ToList();
                                    for(int i=0;i<listProduct.Count;i++)
                                    {
                                        db.tblProducts.Remove(listProduct[i]);
                                        db.SaveChanges();
                                    }
                                    clsSitemap.DeteleSitemap(id.ToString(), "GroupProduct");

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
        void UpdateOrder(int idCate,int Ord1, int Ord2)
        {
            if(Ord1!=Ord2)
            {
                var Kiemtra = db.tblGroupProducts.Where(p => p.Active == true && p.ParentID == idCate && p.Ord == Ord2).ToList();
                if(Kiemtra.Count>0)
                {
                    var listGroup = db.tblGroupProducts.Where(p => p.Active == true && p.ParentID == idCate && p.Ord >= Ord2).ToList();

                    if (Ord2 < Ord1)
                    {
                        listGroup = db.tblGroupProducts.Where(p => p.Active == true && p.ParentID == idCate && p.Ord >= Ord2 && p.Ord < Ord1).ToList();

                        for (int i = 0; i < listGroup.Count; i++)
                        {
                            int idMenu = listGroup[i].id;
                            tblGroupProduct tblgroup = db.tblGroupProducts.Find(idMenu);
                            tblgroup.Ord = tblgroup.Ord + 1;
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        listGroup = db.tblGroupProducts.Where(p => p.Active == true && p.ParentID == idCate && p.Ord >Ord1 && p.Ord <= Ord2).ToList();

                        for (int i = 0; i < listGroup.Count; i++)
                        {
                            int idMenu = listGroup[i].id;
                            tblGroupProduct tblgroup = db.tblGroupProducts.Find(idMenu);
                            tblgroup.Ord = tblgroup.Ord - 1;
                            db.SaveChanges();
                        }
                    }
                }
                
            }
         }
        public PartialViewResult PartialGroupProduct(int? page, string text, string idCate, string pageSizes)
        {
            var listProduct = db.tblGroupProducts.Where(p => p.ParentID == null).OrderBy(p => p.Ord).ToList();
            
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
                ViewBag.chuoicout = "<span style='color: #A52A2A;'>" + pageSize + "</span> / <span style='color: #333;'>" + listProduct.Count.ToString() + "</span>";
                return PartialView(listProduct.ToPagedList(pageNumber, pageSize));

            }
            if (text != null && text != "")
            {
                listProduct = db.tblGroupProducts.Where(p => p.Name.ToUpper().Contains(text.ToUpper()) && p.Active == true).OrderByDescending(p => p.DateCreate).ToList();
                ViewBag.Text = text; 
                return PartialView(listProduct.ToPagedList(pageNumber, pageSize));
                
            }
            if (Request.IsAjaxRequest())
            {
                if (text != null && text != "")
                { 
                        listProduct = db.tblGroupProducts.Where(p => p.Name.ToUpper().Contains(text.ToUpper()) && p.Active == true).OrderByDescending(p => p.DateCreate).ToList();
                        ViewBag.Text = text;
 
                    return PartialView(listProduct.ToPagedList(pageNumber, pageSize));
                }
                 
                if (idCate != "" && idCate != null)
                {
                    int idmenu = int.Parse(idCate);
                    var menucha = db.tblGroupProducts.Find(idmenu);
                     listProduct = db.tblGroupProducts.Where(p => p.ParentID == idmenu).OrderBy(p => p.Ord).ToList();
                    ViewBag.Idcha = idCate;
                    return PartialView(listProduct.ToPagedList(pageNumber, pageSize));
                }
                else
                {
                    ViewBag.Idcha = 0;
                }
            }
            if (idCate != "" && idCate != null)
            {
                int idmenu = int.Parse(idCate);
                var menucha = db.tblGroupProducts.Find(idmenu);
                 listProduct = db.tblGroupProducts.Where(p => p.ParentID == idmenu).OrderBy(p => p.Ord).ToList();
                ViewBag.Idcha = idCate;
                return PartialView(listProduct.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                ViewBag.Idcha = 0;
            }
            return PartialView(listProduct.ToPagedList(pageNumber, pageSize));
        }
        public void UpdateLevel(int idCate,int level)
        {
            var listord = db.tblGroupProducts.Where(p => p.ParentID == idCate).ToList();
            for(int i=0;i<listord.Count;i++)
            {
                int id = listord[i].id;
                var GroupProduct = db.tblGroupProducts.Find(id);
                 db.SaveChanges();
                 int idca=GroupProduct.id;
             }
        }
        public ActionResult UpdateGroupProduct(string id, string Active, string order, string idCate, string Priority)
        {
            if (ClsCheckRole.CheckQuyen(4, 2, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
            {
                int ids = int.Parse(id);
                var GroupProduct = db.tblGroupProducts.Find(ids);
                int idcate1=int.Parse(GroupProduct.id.ToString());
                int Ord1=int.Parse(GroupProduct.Ord.ToString());
                int Ord2 = int.Parse(order);
                GroupProduct.Active = bool.Parse(Active);
                GroupProduct.Ord = int.Parse(order);

                if (idCate != "")
                {
                    UpdateOrder(int.Parse(idCate), Ord1, Ord2);

                    int idCates=int.Parse(idCate);
                    if(idcate1!=idCates)
                    {
                        var grouppd=db.tblGroupProducts.Find(idCates);
                        var listord = db.tblGroupProducts.Where(p => p.ParentID == idCates).OrderByDescending(p => p.id).Take(1).ToList();
                        
                        if(listord.Count>0)
                        {
                            string idParent = GroupProduct.ParentID.ToString();
                            if(idParent!=null || idParent!="")
                            {
                                int idPa = int.Parse(idParent);
                                if(idPa!=idCates)
                                {
                                    GroupProduct.Ord = listord[0].Ord + 1;
 
                                } 
                            }
                            else
                            {
                                listord = db.tblGroupProducts.Where(p => p.ParentID == null).OrderByDescending(p => p.id).Take(1).ToList();
                                GroupProduct.Ord = listord[0].Ord + 1;
                            } 
                        }
                        else
                        {
                            GroupProduct.Ord = 1;
                        }
                        GroupProduct.ParentID = int.Parse(idCate);
                     }
                    else
                    {
                        var listord = db.tblGroupProducts.Where(p => p.ParentID == idCates).OrderByDescending(p => p.id).Take(1).ToList();

                        if (listord.Count > 0)
                        {
                            string idParent = GroupProduct.ParentID.ToString();
                            if (idParent == null || idParent == "")
                            {
                                int idPa = int.Parse(idParent);
                                if (idPa == idCates)
                                {
                                    GroupProduct.Ord = int.Parse(order);

                                }
                                else
                                {
                                    GroupProduct.Ord = listord[0].Ord + 1;
                                }

                            }


                        }
                    }
                    
                }
                else
                {
 
                     
                   
                    GroupProduct.ParentID = null;


                }
                GroupProduct.Priority = bool.Parse(Priority);
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
            carlist.Clear();
            if (ClsCheckRole.CheckQuyen(4, 1, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
            {
                var menuModel = db.tblGroupProducts.Where(m => m.ParentID == null).OrderBy(m => m.id).ToList();
                string strReturn = "---";
                foreach (var item in menuModel)
                {
                    carlist.Add(new SelectListItem { Text = item.Name, Value = item.id.ToString() });
                    StringClass.DropDownListFor(item.id, carlist, strReturn);
                    strReturn = "---";
                }
                ViewBag.drMenu = new SelectList(carlist, "Value", "Text", id);
                
                var pro = db.tblGroupProducts.OrderByDescending(p => p.Ord).Take(1).ToList();
                if(id=="")
                {
                    pro = db.tblGroupProducts.Where(p=>p.ParentID==null).OrderByDescending(p => p.Ord).Take(1).ToList();
                }
                else
                {
                    int ids = int.Parse(id);
                     pro = db.tblGroupProducts.Where(p =>p.ParentID==ids).OrderByDescending(p => p.Ord).ToList();
                }
                if(pro.Count>0)
                {
                    ViewBag.Ord = pro[0].Ord + 1;

                }
                
                else
                {
                    ViewBag.Ord = "1";
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

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(tblGroupProduct tblgroupproduct, FormCollection collection)
        {
             
                if ((Request.Cookies["Username"] == null))
                {
                    return RedirectToAction("LoginIndex", "Login");
                }
                string drMenu = collection["drMenu"];
 
                if (drMenu == "")
                {
                    tblgroupproduct.ParentID = null;
                 }
                else
                {
                    var dbLeve = db.tblGroupProducts.Find(int.Parse(drMenu));
                    tblgroupproduct.ParentID = dbLeve.id;
                 }

                 tblgroupproduct.DateCreate = DateTime.Now;
                string idUser = Request.Cookies["Username"].Values["UserID"];
                tblgroupproduct.idUser = int.Parse(idUser); 
                tblgroupproduct.Tag = StringClass.NameToTag(tblgroupproduct.Name);
                db.tblGroupProducts.Add(tblgroupproduct);
                db.SaveChanges();
                Updatehistoty.UpdateHistory("Add Group Product", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString()); 
                var Groups = db.tblGroupProducts.Where(p => p.Active == true).OrderByDescending(p => p.id).Take(1).ToList();
                string id = Groups[0].id.ToString(); 
                clsSitemap.CreateSitemap("0/"+StringClass.NameToTag(tblgroupproduct.Name), id, "GroupProduct"); 
                if (collection["btnSave"] != null)
                {
                    Session["Thongbao"] = "<div  class=\"alert alert-info alert1\">Bạn đã thêm danh mục thành công !<button class=\"close\" data-dismiss=\"alert\">×</button></div>";

                    return Redirect("/GroupProduct/Index?idCate=" + drMenu);
                }
                if (collection["btnSaveCreate"] != null)
                {
                    Session["Thongbao"] = "<div  class=\"alert alert-info\">Bạn đã thêm danh mục thành công, mời bạn thêm danh mục sản phẩm mới !<button class=\"close\" data-dismiss=\"alert\">×</button></div>";
                    return Redirect("/GroupProduct/Create?id=" + drMenu + "");
                }
               return   Redirect("/GroupProduct/Index?idCate=" + drMenu);
          

         }
        public ActionResult AutoOrd(string idCate)
        {             var result = string.Empty;

            if(idCate!="")
            {
            int id = int.Parse(idCate);
              
            var ListProduct = db.tblGroupProducts.Where(p => p.ParentID==id ).OrderByDescending(p => p.Ord).Take(1).ToList();
          
            if (ListProduct.Count > 0)
            {
                int stt = int.Parse(ListProduct[0].Ord.ToString()) + 1;
                result = stt.ToString();
            }
            else
            {
                result = "0";

            }
            }
            else
            {
                var ListProduct = db.tblGroupProducts.Where(p => p.ParentID==null).OrderByDescending(p => p.Ord).Take(1).ToList();

                int stt = int.Parse(ListProduct[0].Ord.ToString()) + 1;
                result = stt.ToString();
            }
            
             
            return Json(new { result = result });
        }
        public ActionResult Edit(int id)
        {
            carlist.Clear();
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            if (ClsCheckRole.CheckQuyen(4, 2, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
            {
                tblGroupProduct tblgroupproduct = db.tblGroupProducts.First(p => p.id == id);
                if (tblgroupproduct == null)
                {
                    return HttpNotFound();
                }
                ViewBag.id = id;
                var menuName = db.tblGroupProducts.ToList();
                var pro = db.tblGroupProducts.OrderByDescending(p => p.Ord).Take(1).ToList();
                var menuModel = db.tblGroupProducts.Where(m => m.ParentID == null).OrderBy(m => m.id).ToList();
                string strReturn = "---";
                carlist.Clear();
                foreach (var item in menuModel)
                {
                    carlist.Add(new SelectListItem { Text = item.Name, Value = item.id.ToString() });
                    StringClass.DropDownListFor(item.id, carlist, strReturn);
                    strReturn = "---";
                }
                 int idParent=0;
                string kiemtra = tblgroupproduct.ParentID.ToString();
                if (kiemtra!=null && kiemtra!="")
                {
                    idParent = int.Parse(tblgroupproduct.ParentID.ToString());
                     ViewBag.drMenu = new SelectList(carlist, "Value", "Text", idParent);
                } 
                  else 
                      ViewBag.drMenu = carlist;
           
                    return View(tblgroupproduct);
            }
            else
            {
                return Redirect("/Users/Erro");

            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(tblGroupProduct tblgroupproduct, FormCollection collection, int id )
        {
            if (ModelState.IsValid)
            {
                ////id = int.Parse(collection["idProduct"]);
                ////tblgroupproduct.id = id;
                ////tblgroupproduct = db.tblGroupProducts.Find(id); 
                string drMenu = collection["drMenu"];
                string levelhiden=collection["Level"];

                if (drMenu == "")
                {
                    tblgroupproduct.ParentID = null;
                     }
                else
                {
                    if (drMenu != id.ToString())
                    {
                        var dbLeve = db.tblGroupProducts.Find(int.Parse(drMenu));
                        tblgroupproduct.ParentID = dbLeve.id;
                        
                    }
                }
                string idUser = Request.Cookies["Username"].Values["UserID"];
                tblgroupproduct.idUser = int.Parse(idUser);

                bool URL = (collection["URL"] == "false") ? false : true;
                if (URL == true)
                {
                    tblgroupproduct.Tag = StringClass.NameToTag(tblgroupproduct.Name);
                }
                else
                {
                    tblgroupproduct.Tag = collection["NameURL"];
                }
                clsSitemap.UpdateSitemap("0/" + tblgroupproduct.Tag , id.ToString(), "GroupProduct");

                tblgroupproduct.DateCreate = DateTime.Now;
                db.Entry(tblgroupproduct).State = EntityState.Modified;
                 db.SaveChanges();
                #region[Updatehistory]
                Updatehistoty.UpdateHistory("Edit GroupsProduct", Request.Cookies["Username"].Values["FullName"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                #endregion
                if (collection["btnSave"] != null)
                {
              
                    if (drMenu=="")
                    {
                        Session["Thongbao"] = "<div  class=\"alert alert-info alert1\">Bạn đã sửa danh mục thành công !<button class=\"close\" data-dismiss=\"alert\">×</button></div>";

                        return Redirect("/GroupProduct/Index?id=" + drMenu + "");
                    } 
                    else
                    {
                        Session["Thongbao"] = "<div  class=\"alert alert-info alert1\">Bạn đã sửa danh mục thành công  !<button class=\"close\" data-dismiss=\"alert\">×</button></div>";
 
                        return Redirect("/GroupProduct/Index?idCate=" + drMenu);
                        
                    }
                }
                if (collection["btnSaveCreate"] != null)
                {
                    Session["Thongbao"] = "<div  class=\"alert alert-info\">Bạn đã sửa danh mục thành công, mời bạn thêm danh mục sản phẩm mới !<button class=\"close\" data-dismiss=\"alert\">×</button></div>";
                    return Redirect("/GroupProduct/Create?id=" + drMenu + "");
                }
            }
            return Redirect("/GroupProduct/");
        }
        public ActionResult DeleteGroupProduct(int id)
        {
            if (ClsCheckRole.CheckQuyen(4, 3, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
            {
                tblGroupProduct tblproduct = db.tblGroupProducts.Find(id);
                clsSitemap.DeteleSitemap(id.ToString(), "GroupProduct");
                var result = string.Empty;
                db.tblGroupProducts.Remove(tblproduct);
                db.SaveChanges();
                var listProduct = db.tblProducts.Where(p => p.idCate == id).ToList();
                for (int i = 0; i < listProduct.Count; i++)
                {
                    db.tblProducts.Remove(listProduct[i]);
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
    }
}
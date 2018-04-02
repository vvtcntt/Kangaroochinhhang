using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kangaroochinhhang.Models;
using System.Data.Entity;
namespace Kangaroochinhhang.Controllers.Admin.Maps
{
    public class MapsadController : Controller
    {
        KangaroochinhhangContext db = new KangaroochinhhangContext();
        // GET: Mapsad
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Edit()
        {
            if ((Request.Cookies["Username"] == null))
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            if (ClsCheckRole.CheckQuyen(5, 2, int.Parse(Request.Cookies["Username"].Values["UserID"])) == true)
            {

                tblMap tblmaps = db.tblMaps.First();

                if (tblmaps == null)
                {
                    return HttpNotFound();
                }

                return View(tblmaps);
            }
            else
            {
                return Redirect("/Users/Erro");

            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(tblMap tblmaps, FormCollection collection)
        {

            if (ModelState.IsValid)
            {       string idUser = Request.Cookies["Username"].Values["UserID"];
                    tblmaps.UserID = int.Parse(idUser);
                    tblmaps.DateCreate = DateTime.Now;

                    int id = int.Parse(collection["idMaps"]);
                    tblmaps.id = id;
                    db.Entry(tblmaps).State = EntityState.Modified;
                    db.SaveChanges();

                    
                   
               
                #region[Updatehistory]
                Updatehistoty.UpdateHistory("Update Maps", Request.Cookies["Username"].Values["Username"].ToString(), Request.Cookies["Username"].Values["UserID"].ToString());
                #endregion
            }
            return View(tblmaps);
        }
    }
}
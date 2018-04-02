using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kangaroochinhhang.Models;
namespace Kangaroochinhhang.Models
{
    public class Updatehistoty
    {
        public static void UpdateHistory(string task,string FullName,string UserID)
        {         KangaroochinhhangContext db = new KangaroochinhhangContext();


             tblHistoryLogin tblhistorylogin = new tblHistoryLogin();
            tblhistorylogin.FullName = FullName;
            tblhistorylogin.Task = task;
            tblhistorylogin.idUser = int.Parse(UserID);
            tblhistorylogin.DateCreate = DateTime.Now;
            tblhistorylogin.Active = true;
            
            db.tblHistoryLogins.Add(tblhistorylogin);
            db.SaveChanges();
           
        }
    }
}
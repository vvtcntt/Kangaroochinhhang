using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kangaroochinhhang.Models;
namespace Kangaroochinhhang.Models
{

    public class clsManufactures
    {
        public static KangaroochinhhangContext db = new KangaroochinhhangContext();

        public static int idManu(int id)
        {
            var listConnect = db.tblConnectManuProducts.Where(p => p.idCate == id).Select(p => p.idManu).Take(1).ToList();
            int idManu = int.Parse(listConnect[0].Value.ToString());

            return idManu;
        }
    }
}
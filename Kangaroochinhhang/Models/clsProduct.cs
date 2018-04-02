using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kangaroochinhhang.Models
{
    public class clsProduct
    {
        public int id
        {
            get;
            set;
        }
        public int idMenu
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public string Tag
        {
            get;
            set;
        }
        public string Images
        {
            get;
            set;
        }
        public int Ord
        {
            get;
            set;
        }
        public int Price
        {
            get;
            set;
        }
        public int SumPrice
        {
            get;
            set;
        }
    }
}
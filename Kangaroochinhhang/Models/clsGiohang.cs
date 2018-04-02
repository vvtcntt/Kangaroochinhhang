using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kangaroochinhhang.Models
{
    public class clsGiohang
    { public clsGiohang()
        { 
        this.CartItem=new List<clsProduct>();
        }
        public List<clsProduct> CartItem
        {
            get;
            set;
        }

        public float CartTotal {
            get;
            set;
        }
    }
}
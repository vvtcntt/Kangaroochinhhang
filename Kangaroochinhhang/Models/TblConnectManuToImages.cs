using System;
using System.Collections.Generic;

namespace Kangaroochinhhang.models
{
    public partial class TblConnectManuToImages
    {
        public int Id { get; set; }
        public int? IdImage { get; set; }
        public int? IdManu { get; set; }
    }
}

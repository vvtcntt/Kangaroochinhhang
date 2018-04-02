using System;
using System.Collections.Generic;

namespace Kangaroochinhhang.models
{
    public partial class TblConnectManuToNews
    {
        public int Id { get; set; }
        public int? IdManu { get; set; }
        public int? IdNews { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace Kangaroochinhhang.models
{
    public partial class TblRight
    {
        public int Id { get; set; }
        public int? IdModule { get; set; }
        public int? IdUser { get; set; }
        public int? Role { get; set; }
    }
}

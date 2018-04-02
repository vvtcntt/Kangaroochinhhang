using System;
using System.Collections.Generic;

namespace Kangaroochinhhang.models
{
    public partial class TblModule
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Ord { get; set; }
        public bool? Active { get; set; }
    }
}

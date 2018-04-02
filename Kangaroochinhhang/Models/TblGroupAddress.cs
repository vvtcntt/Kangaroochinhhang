using System;
using System.Collections.Generic;

namespace Kangaroochinhhang.models
{
    public partial class TblGroupAddress
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }
        public string Description { get; set; }
        public bool? Active { get; set; }
        public int? Ord { get; set; }
    }
}

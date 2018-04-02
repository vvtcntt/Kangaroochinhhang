using System;
using System.Collections.Generic;

namespace Kangaroochinhhang.models
{
    public partial class TblBanks
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string NameBank { get; set; }
        public string NumberBank { get; set; }
        public string Images { get; set; }
        public bool? Active { get; set; }
        public int? Ord { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace Kangaroochinhhang.models
{
    public partial class TblCriteria
    {
        public int Id { get; set; }
        public int? IdCate { get; set; }
        public string Name { get; set; }
        public bool? Active { get; set; }
        public bool? Priority { get; set; }
        public bool? Style { get; set; }
        public int? Ord { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace Kangaroochinhhang.models
{
    public partial class TblInfoCriteria
    {
        public int Id { get; set; }
        public int? IdCri { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int? Type { get; set; }
        public int? Ord { get; set; }
        public bool? Active { get; set; }
    }
}

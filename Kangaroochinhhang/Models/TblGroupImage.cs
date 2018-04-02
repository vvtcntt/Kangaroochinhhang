using System;
using System.Collections.Generic;

namespace Kangaroochinhhang.models
{
    public partial class TblGroupImage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Ord { get; set; }
        public bool? Active { get; set; }
        public int? IdUser { get; set; }
        public DateTime? DateCreate { get; set; }
    }
}

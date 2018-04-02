using System;
using System.Collections.Generic;

namespace Kangaroochinhhang.models
{
    public partial class TblVideo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int? Ord { get; set; }
        public bool? AutoPlay { get; set; }
        public bool? Active { get; set; }
        public DateTime? DateCreate { get; set; }
        public int? IdUser { get; set; }
    }
}

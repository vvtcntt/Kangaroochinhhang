using System;
using System.Collections.Generic;

namespace Kangaroochinhhang.models
{
    public partial class TblUrl
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int? Ord { get; set; }
        public bool? Active { get; set; }
        public string Rel { get; set; }
        public DateTime? DateCreate { get; set; }
        public int? IdUser { get; set; }
    }
}

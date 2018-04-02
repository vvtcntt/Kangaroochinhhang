using System;
using System.Collections.Generic;

namespace Kangaroochinhhang.models
{
    public partial class TblImage
    {
        public int Id { get; set; }
        public int? IdCate { get; set; }
        public int? IdManu { get; set; }
        public string Name { get; set; }
        public string Images { get; set; }
        public string Url { get; set; }
        public int? Ord { get; set; }
        public bool? Link { get; set; }
        public string Color { get; set; }
        public string ImagesMobile { get; set; }
        public bool? Active { get; set; }
        public DateTime? DateCreate { get; set; }
        public int? IdUser { get; set; }
    }
}

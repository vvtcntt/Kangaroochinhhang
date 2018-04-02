using System;
using System.Collections.Generic;

namespace Kangaroochinhhang.models
{
    public partial class TblLoiloc
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Age { get; set; }
        public int? Ord { get; set; }
        public bool? Active { get; set; }
    }
}

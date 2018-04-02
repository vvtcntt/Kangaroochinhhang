using System;
using System.Collections.Generic;

namespace Kangaroochinhhang.models
{
    public partial class TblImageProduct
    {
        public int Id { get; set; }
        public int? IdProduct { get; set; }
        public string Name { get; set; }
        public int? Type { get; set; }
        public string Images { get; set; }
    }
}

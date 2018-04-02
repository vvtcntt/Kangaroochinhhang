using System;
using System.Collections.Generic;

namespace Kangaroochinhhang.models
{
    public partial class TblAddress
    {
        public int Id { get; set; }
        public int? IdCate { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Hotline { get; set; }
        public string Maps { get; set; }
        public string Images { get; set; }
        public int? Ord { get; set; }
        public bool? Active { get; set; }
        public int? IdUser { get; set; }
    }
}

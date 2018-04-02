using System;
using System.Collections.Generic;

namespace Kangaroochinhhang.models
{
    public partial class TblAgency
    {
        public int Id { get; set; }
        public int? IdMenu { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Images { get; set; }
        public string Certificate { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string People { get; set; }
        public string Email { get; set; }
        public string Tag { get; set; }
        public bool? Active { get; set; }
        public int? Ord { get; set; }
        public string Tabs { get; set; }
        public DateTime? DateCreate { get; set; }
        public int? IdUser { get; set; }
    }
}

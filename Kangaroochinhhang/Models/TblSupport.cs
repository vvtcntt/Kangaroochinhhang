using System;
using System.Collections.Generic;

namespace Kangaroochinhhang.models
{
    public partial class TblSupport
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Yahoo { get; set; }
        public string Skyper { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string Mission { get; set; }
        public string Images { get; set; }
        public int? Ord { get; set; }
        public bool? Active { get; set; }
        public int? IdUser { get; set; }
        public DateTime? DateCreate { get; set; }
    }
}

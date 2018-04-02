using System;
using System.Collections.Generic;

namespace Kangaroochinhhang.models
{
    public partial class TblRegister
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameMachine { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public DateTime? DateTimebyy { get; set; }
        public DateTime? DateTime { get; set; }
        public int? Ord { get; set; }
        public bool? Active { get; set; }
    }
}

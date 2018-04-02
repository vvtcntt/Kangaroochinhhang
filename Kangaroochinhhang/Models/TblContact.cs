using System;
using System.Collections.Generic;

namespace Kangaroochinhhang.models
{
    public partial class TblContact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Content { get; set; }
        public DateTime? DateCreate { get; set; }
        public bool? Active { get; set; }
        public int? IdUser { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace Kangaroochinhhang.models
{
    public partial class TblMaps
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public DateTime? DateCreate { get; set; }
        public int? UserId { get; set; }
    }
}

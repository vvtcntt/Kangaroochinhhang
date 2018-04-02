using System;
using System.Collections.Generic;

namespace Kangaroochinhhang.models
{
    public partial class TblCountOnline
    {
        public int Id { get; set; }
        public int? Online { get; set; }
        public int? Sum { get; set; }
        public DateTime? DateCreate { get; set; }
    }
}

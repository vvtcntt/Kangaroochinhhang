using System;
using System.Collections.Generic;

namespace Kangaroochinhhang.models
{
    public partial class TblTags
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Tag { get; set; }
        public bool? Price { get; set; }
        public string Title { get; set; }
        public string Keyword { get; set; }
        public int? Ord { get; set; }
        public bool? Active { get; set; }
        public DateTime? DateCreate { get; set; }
        public int? IdUser { get; set; }
    }
}

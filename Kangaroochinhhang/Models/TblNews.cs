using System;
using System.Collections.Generic;

namespace Kangaroochinhhang.models
{
    public partial class TblNews
    {
        public int Id { get; set; }
        public int? IdCate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Images { get; set; }
        public int? Ord { get; set; }
        public bool? Active { get; set; }
        public bool? ViewHomes { get; set; }
        public string Tag { get; set; }
        public bool? Style { get; set; }
        public string Title { get; set; }
        public string Keyword { get; set; }
        public string Tabs { get; set; }
        public int? Visit { get; set; }
        public DateTime? DateCreate { get; set; }
        public int? IdUser { get; set; }
        public string Meta { get; set; }
    }
}
